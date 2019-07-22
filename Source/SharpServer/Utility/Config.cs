using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace SharpServer
{

    public class ConfigSection : Dictionary<string, object>
    {
        public T GetValue<T>(string key, T defValue = default)
        {
            if (TryGetValue(key, out object val))
            {
                return (T)val;
            }

            return defValue;
        }

        public void SetValue<T>(string key, T val)
        {
            this[key] = val;
        }

    }

    public class Config : ConfigSection
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
                using (FileStream stream = File.OpenRead(Path.Combine(DataPath, path)))
                {
                    Config cfg = null;
                    cfg = Utf8Json.JsonSerializer.Deserialize<Config>(stream);
                    return cfg;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ConfigSection GetSection(string name)
        {
            return GetValue<ConfigSection>(name);
        }

    }
}
