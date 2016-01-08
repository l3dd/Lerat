using System;
using System.Reflection;
using log4net;
using log4net.Config;

namespace LeratHarborMap.TechnicalCore
{
    public static class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static Logger()
        {
            XmlConfigurator.Configure(); //only once
        }

        public static void Debug(string message)
        {
            Log.Debug(message);
        }

        public static void Debug(string message, params object[] args)
        {
            Log.DebugFormat(message, args);
        }

        public static void Info(string message)
        {
            Log.Info(message);
        }

        public static void Info(string message, params object[] args)
        {
            Log.Info(string.Format(message, args));
        }

        public static void Warning(string mesxsage)
        {
            Log.Warn(mesxsage);
        }

        public static void Error(string message)
        {
            Log.Error(message);
        }

        public static void Error(string message, Exception exception)
        {
            Log.Error(message, exception);
        }
    }
}
