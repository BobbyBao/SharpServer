using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class Log : IService
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Init()
        {
        }

        public void Shutdown()
        {
            NLog.LogManager.Shutdown();
        }

        public static void Info(string msg, params object[] args) => logger.Info(msg, args);
        public static void Info(Exception e, string msg, params object[] args) => logger.Info(e, msg, args);
        public static void Debug(string msg, params object[] args) => logger.Debug(msg, args);
        public static void Debug(Exception e, string msg, params object[] args) => logger.Debug(e, msg, args);
        public static void Warn(string msg, params object[] args) => logger.Warn(msg, args);
        public static void Warn(Exception e, string msg, params object[] args) => logger.Warn(e, msg, args);
        public static void Error(string msg, params object[] args) => logger.Error(msg, args);
        public static void Error(Exception e, string msg, params object[] args) => logger.Error(e, msg, args);
        public static void Fatal(string msg, params object[] args) => logger.Fatal(msg, args);
        public static void Fatal(Exception e, string msg, params object[] args) => logger.Fatal(e, msg, args);

    }
}
