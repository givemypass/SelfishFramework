using System;

namespace SelfishFramework.Src.SLogs
{
    public class UnityLogger : ILogger
    {
        public void Log(string message)
        {
            UnityEngine.Debug.Log(message);    
        }

        public void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public void LogError(string message)
        {
            UnityEngine.Debug.LogError(message);
        }

        public void LogException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception), "Exception cannot be null");
            }
            
            UnityEngine.Debug.LogException(exception);
        }
    }
}