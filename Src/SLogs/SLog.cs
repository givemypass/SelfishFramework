using System.Collections.Generic;

namespace SelfishFramework.Src.SLogs
{
    public static class SLog
    {
        private static readonly List<ILogger> _loggers = new();
        
        static SLog()
        {
            // Register the default logger
            RegisterLogger(new UnityLogger());
        }
        
        public static void RegisterLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }
        
        public static void Log(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(message);
            }
        }
        
        public static void LogWarning(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.LogWarning(message);
            }
        }
        
        public static void LogError(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.LogError(message);
            }
        }
        
        public static void LogException(System.Exception exception)
        {
            foreach (var logger in _loggers)
            {
                logger.LogException(exception);
            }
        }
    }
}