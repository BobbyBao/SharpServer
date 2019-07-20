using System;
using System.Collections.Generic;
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
        public ConfigSection GetSection(string name)
        {
            return GetValue<ConfigSection>(name);
        }

    }
}
