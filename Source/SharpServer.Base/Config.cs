using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace SharpServer
{
    public class Config : Dictionary<string, object>
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
                Log.Info(e, "");
                return null;
            }
        }

        public Dictionary<string, object> GetSection(string name)
        {
            return this.GetValue<Dictionary<string, object>>(name);
        }

    }

    public static class ConfigHelper
    {
        public static T GetValue<T>(this Dictionary<string, object> section, string key, T defValue = default)
        {
            if (section.TryGetValue(key, out object val))
            {
                return (T)val;
            }

            return defValue;
        }

        public static sbyte GetValue(this Dictionary<string, object> section, string key, sbyte defValue = default)
            => (sbyte)section.GetValue(key, (double)defValue);
        public static byte GetValue(this Dictionary<string, object> section, string key, byte defValue = default)
            => (byte)section.GetValue(key, (double)defValue);

        public static short GetValue(this Dictionary<string, object> section, string key, short defValue = default)
            => (short)section.GetValue(key, (double)defValue);
        public static ushort GetValue(this Dictionary<string, object> section, string key, ushort defValue = default)
            => (ushort)section.GetValue(key, (double)defValue);

        public static int GetValue(this Dictionary<string, object> section, string key, int defValue = default)
            => (int)section.GetValue(key, (double)defValue);
        public static uint GetValue(this Dictionary<string, object> section, string key, uint defValue = default)
            => (uint)section.GetValue(key, (double)defValue);

        public static long GetValue(this Dictionary<string, object> section, string key, long defValue = default)
            => (long)section.GetValue(key, (double)defValue);
        public static ulong GetValue(this Dictionary<string, object> section, string key, ulong defValue = default)
            => (ulong)section.GetValue(key, (double)defValue);

        public static void SetValue<T>(this Dictionary<string, object> section, string key, T val)
        {
            section[key] = val;
        }

    }

}
