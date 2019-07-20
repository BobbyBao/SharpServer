using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpServer
{

    public class AppBase
    {
        public void Start()
        {
            /*
            var cfg = new Config
            {
                ["IP"] = "127.0.0.1",
                ["Port"] = 2239,

                ["Log"] = new ConfigSection
                {
                    ["LogFile"] = "Log.log"
                }

            };


            var t = Utf8Json.JsonSerializer.Serialize(cfg);
            var json = Utf8Json.JsonSerializer.PrettyPrint(t);
            File.WriteAllText("test.cfg", json);*/

            OnInit();

            OnRun();

            OnShutdown();
        }


        protected virtual void OnInit()
        {
        }

        protected virtual void OnIdle()
        {
        }

        protected virtual void OnRun()
        {
        }

        protected virtual void OnShutdown()
        {
        }


    }
}
