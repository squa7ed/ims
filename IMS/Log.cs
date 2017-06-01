using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IMS
{
    public static class Log
    {
        private static ILogger logger;

        public static void Register()
        {
            logger = new LogHelper();
        }

        public static void Critical(string message, params object[] args)
        {
            logger.Critical(message, args);
        }

        public static void Verbose(string message, params object[] args)
        {
            logger.Verbose(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            logger.Error(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            logger.Info(message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            logger.Warning(message, args);
        }
    }

    class LogHelper : ILogger
    {
        private readonly TraceSource source;

        public LogHelper()
        {
            source = new TraceSource("IMS")
            {
                Switch = new SourceSwitch("LogSwitch", "Verbose")
            };
        }

        public void Critical(string message, params object[] args)
        {
            lock (source) { source.TraceEvent(TraceEventType.Critical, 1, message, args); }
        }

        public void Error(string message, params object[] args)
        {
            lock (source) { source.TraceEvent(TraceEventType.Error, 2, message, args); }
        }

        public void Warning(string message, params object[] args)
        {
            lock (source) { source.TraceEvent(TraceEventType.Warning, 4, message, args); }
        }

        public void Info(string message, params object[] args)
        {
            lock (source) { source.TraceEvent(TraceEventType.Information, 8, message, args); }
        }

        public void Verbose(string message, params object[] args)
        {
            lock (source) { source.TraceEvent(TraceEventType.Verbose, 16, message, args); }
        }
    }
}
