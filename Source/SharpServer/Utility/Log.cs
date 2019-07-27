using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class Log : Subsystem
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public Log()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var assembly = System.Reflection.Assembly.GetEntryAssembly();
            var name = assembly.GetName().Name;

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget(name)
            {
                FileName = "${basedir}/logs/${logger}/${shortdate}.log",
                Layout = "${longdate} ${aspnet-request:servervariable=URL}[${uppercase:${level}}] ${message} ${exception:format=toString,Data:maxInnerExceptionLevel=10}"
            };

            var logconsole = new NLog.Targets.ConsoleTarget("logconsole")
            {
                Layout = "${longdate} ${aspnet-request:servervariable=URL}[${uppercase:${level}}] ${message} ${exception:format=toString,Data:maxInnerExceptionLevel=10}"
            };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
           
        }

        public override void Shutdown()
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
