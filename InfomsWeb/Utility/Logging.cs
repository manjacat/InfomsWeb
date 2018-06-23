using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace InfomsWeb.Utility
{
    public class Logging
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void TestLogger()
        {
            int k = 42;
            int l = 100;

            logger.Trace("Sample trace message, k={0}, l={1}", k, l);
            logger.Debug("Sample debug message, k={0}, l={1}", k, l);
            logger.Info("Sample informational message, k={0}, l={1}", k, l);
            logger.Warn("Sample warning message, k={0}, l={1}", k, l);
            logger.Error("Sample error message, k={0}, l={1}", k, l);
            logger.Fatal("Sample fatal error message, k={0}, l={1}", k, l);
            logger.Log(LogLevel.Info, "Sample informational message, k={0}, l={1}", k, l);
        }

        public void Error(Exception ex, string message)
        {
            logger.Error("{0}; SQL: {1}.", message, ex.ToString());
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(Exception ex)
        {
            logger.Error("{0}; SQL: {1}", ex.Message, ex.ToString());
        }

        public void Trace(string message)
        {
            logger.Trace(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }
    }
}