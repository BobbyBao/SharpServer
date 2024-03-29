﻿using Hocon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SharpServer
{
    public class Config : HoconRoot
    {
        public static string DataPath { get; set; }

        static Config global;
        public static Config Global
        {
            get
            {
                if (global == null)
                {
                    global = Load("Config/Global.cfg");
                }
                return global;
            }
        }

        static Config appConf;
        public static Config App
        {
            get
            {
                if (appConf == null)
                {
                    var assembly = System.Reflection.Assembly.GetEntryAssembly();
                    var name = assembly.GetName().Name;
                    appConf = Load($"Config/{name}.cfg");
                }
                return appConf;
            }
        }

        public static Config Load(string path)
        {
            try
            {
                var text = File.ReadAllText(Path.Combine(DataPath, path));
                return new Config(Hocon.Parser.Parse(text));
            }
            catch (Exception e)
            {
                Log.Info(e, "");
                return null;
            }
        }

        public static Config ParseString(string text) => new Config(Hocon.Parser.Parse(text));
        /// <summary>
        /// The configuration used as a secondary source.
        /// </summary>
        public Config Fallback { get; private set; }

        /// <summary>
        /// The root node of this configuration section
        /// </summary>
        public HoconValue Root => Value;

        /// <inheritdoc/>
        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        private Config()
        { }

        /// <inheritdoc cref="Config()"/>
        /// <param name="root">The root node to base this configuration.</param>
        /// <exception cref="T:System.ArgumentNullException">"The root value cannot be null."</exception>
        public Config(HoconRoot root) : base(root?.Value, root?.Substitutions ?? Enumerable.Empty<HoconSubstitution>())
        { }

        /// <inheritdoc cref="Config()"/>
        /// <param name="source">The configuration to use as the primary source.</param>
        /// <param name="fallback">The configuration to use as a secondary source.</param>
        /// <exception cref="ArgumentNullException">The source configuration cannot be null.</exception>
        public Config(HoconRoot source, Config fallback) : base(source?.Value, source?.Substitutions ?? Enumerable.Empty<HoconSubstitution>())
        {
            Fallback = fallback;
        }

        /// <summary>
        /// Generates a deep clone of the current configuration.
        /// </summary>
        /// <returns>A deep clone of the current configuration</returns>
        protected Config Copy()
        {
            //deep clone
            return new Config
            {
                Fallback = Fallback?.Copy(),
                Value = (HoconValue)Value.Clone(null)
            };
        }

        protected override HoconValue GetNode(HoconPath path)
        {
            HoconValue result;
            try
            {
                result = Root.GetObject().GetValue(path);
            }
            catch
            {
                result = Fallback?.GetNode(path);
            }

            return result;
        }

        /// <summary>
        /// Retrieves a new configuration from the current configuration
        /// with the root node being the supplied path.
        /// </summary>
        /// <param name="path">The path that contains the configuration to retrieve.</param>
        /// <returns>A new configuration with the root node being the supplied path.</returns>
        public Config GetConfig(string path)
            => GetConfig(HoconPath.Parse(path));

        public virtual Config GetConfig(HoconPath path)
        {
            var value = GetNode(path);
            if (Fallback != null)
            {
                var f = Fallback.GetConfig(path);
                return value == null ? f : new Config(new HoconRoot(value)).WithFallback(f);
            }

            return value == null ? null : new Config(new HoconRoot(value));
        }

        /// <summary>
        /// Configure the current configuration with a secondary source.
        /// </summary>
        /// <param name="fallback">The configuration to use as a secondary source.</param>
        /// <returns>The current configuration configured with the specified fallback.</returns>
        /// <exception cref="ArgumentException">Config can not have itself as fallback.</exception>
        public virtual Config WithFallback(Config fallback)
        {
            if (fallback == this)
                throw new ArgumentException("Config can not have itself as fallback", nameof(fallback));

            var clone = Copy();

            var current = clone;
            while (current.Fallback != null)
            {
                current = current.Fallback;
            }
            current.Fallback = fallback;

            return clone;
        }

        /// <summary>
        /// Adds the supplied configuration string as a fallback to the supplied configuration.
        /// </summary>
        /// <param name="config">The configuration used as the source.</param>
        /// <param name="fallback">The string used as the fallback configuration.</param>
        /// <returns>The supplied configuration configured with the supplied fallback.</returns>
        public static Config operator +(Config config, string fallback)
            => config.WithFallback(ParseString(fallback));

        /// <summary>
        /// Adds the supplied configuration as a fallback to the supplied configuration string.
        /// </summary>
        /// <param name="configHocon">The configuration string used as the source.</param>
        /// <param name="fallbackConfig">The configuration used as the fallback.</param>
        /// <returns>A configuration configured with the supplied fallback.</returns>
        public static Config operator +(string configHocon, Config fallbackConfig)
            => ParseString(configHocon).WithFallback(fallbackConfig);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Config"/>.
        /// </summary>
        /// <param name="str">The string that contains a configuration.</param>
        /// <returns>A configuration based on the supplied string.</returns>
        public static implicit operator Config(string str) => ParseString(str);

        /// <inheritdoc />
        public override IEnumerable<KeyValuePair<string, HoconField>> AsEnumerable()
        {
            var used = new HashSet<string>();
            var current = this;
            while (current != null)
            {
                foreach (var kvp in current.Root.GetObject())
                {
                    if (used.Contains(kvp.Key))
                        continue;

                    yield return kvp;
                    used.Add(kvp.Key);
                }
                current = current.Fallback;
            }
        }
    }

    /// <summary>
    /// This class contains convenience methods for working with <see cref="Config"/>.
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Retrieves the current configuration or the fallback
        /// configuration if the current one is null.
        /// </summary>
        /// <param name="config">The configuration used as the source.</param>
        /// <param name="fallback">The configuration to use as a secondary source.</param>
        /// <returns>The current configuration or the fallback configuration if the current one is null.</returns>
        public static Config SafeWithFallback(this Config config, Config fallback)
        {
            return config == null
                ? fallback
                : ReferenceEquals(config, fallback)
                    ? config
                    : config.WithFallback(fallback);
        }

        /// <summary>
        /// Determines if the supplied configuration has any usable content period.
        /// </summary>
        /// <param name="config">The configuration used as the source.</param>
        /// <returns><c>true></c> if the <see cref="Config" /> is null or <see cref="HoconRoot.IsEmpty" />; otherwise <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this Config config)
            => config == null || config.IsEmpty;
    }

}
