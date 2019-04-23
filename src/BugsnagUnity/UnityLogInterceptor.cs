using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BugsnagUnity
{
    class UnityLogInterceptor : ILogger
    {
        private ILogger defaultLogger;
        private Client Client;

        public UnityLogInterceptor(Client client)
        {
            Client = client;
            defaultLogger = UnityEngine.Debug.unityLogger;
        }

        public void Install()
        {
            var field = typeof(UnityEngine.Debug).GetField("s_Logger",
                            BindingFlags.Static |
                            BindingFlags.NonPublic);

            field.SetValue(null, this);
        }

        public void Uninstall()
        {
            if (defaultLogger != null)
            {
                var field = typeof(UnityEngine.Debug).GetField("s_Logger",
                            BindingFlags.Static |
                            BindingFlags.NonPublic);

                field.SetValue(null, defaultLogger);
                defaultLogger = null;
            }
        }

        /// <summary>
        ///   <para>Set Logger.ILogHandler.</para>
        /// </summary>
        public ILogHandler logHandler
        {
            get { return defaultLogger.logHandler; }
            set { defaultLogger.logHandler = value; }
        }

        /// <summary>
        ///   <para>To runtime toggle debug logging [ON/OFF].</para>
        /// </summary>
        public bool logEnabled
        {
            get { return defaultLogger.logEnabled; }
            set { defaultLogger.logEnabled = value; }
        }

        /// <summary>
        ///   <para>To selective enable debug log message.</para>
        /// </summary>
        public LogType filterLogType
        {
            get { return defaultLogger.filterLogType; }
            set { defaultLogger.filterLogType = value; }
        }

        /// <summary>
        ///   <para>Check logging is enabled based on the LogType.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <returns>
        ///   <para>Retrun true in case logs of LogType will be logged otherwise returns false.</para>
        /// </returns>
        public bool IsLogTypeAllowed(LogType logType)
        {
            return defaultLogger.IsLogTypeAllowed(logType);
        }

        /// <summary>
        ///   <para>Logs message to the Unity Console using default logger.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        public void Log(LogType logType, object message)
        {
            processLog(logType, message.ToString());
            defaultLogger.Log(logType, message);
        }

        /// <summary>
        ///   <para>Logs message to the Unity Console using default logger.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        public void Log(LogType logType, object message, UnityEngine.Object context)
        {
            processLog(logType, message.ToString());
            defaultLogger.Log(logType, message, context);
        }

        /// <summary>
        ///   <para>Logs message to the Unity Console using default logger.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        public void Log(LogType logType, string tag, object message)
        {
            processLog(logType, message.ToString());
            defaultLogger.Log(logType, tag, message);
        }

        /// <summary>
        ///   <para>Logs message to the Unity Console using default logger.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        public void Log(LogType logType, string tag, object message, UnityEngine.Object context)
        {
            processLog(logType, message.ToString());
            defaultLogger.Log(logType, tag, message, context);
        }

        /// <summary>
        ///   <para>Logs message to the Unity Console using default logger.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        public void Log(object message)
        {
            processLog(LogType.Log, message.ToString());
            defaultLogger.Log(message);
        }

        /// <summary>
        ///   <para>Logs message to the Unity Console using default logger.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        public void Log(string tag, object message)
        {
            processLog(LogType.Log, message.ToString());
            defaultLogger.Log(tag, message);
        }

        /// <summary>
        ///   <para>Logs message to the Unity Console using default logger.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        public void Log(string tag, object message, UnityEngine.Object context)
        {
            processLog(LogType.Log, message.ToString());
            defaultLogger.Log(tag, message, context);
        }

        /// <summary>
        ///   <para>A variant of Logger.Log that logs an warning message.</para>
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void LogWarning(string tag, object message)
        {
            processLog(LogType.Warning, message.ToString());
            defaultLogger.LogWarning(tag, message);
        }

        /// <summary>
        ///   <para>A variant of Logger.Log that logs an warning message.</para>
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void LogWarning(string tag, object message, UnityEngine.Object context)
        {
            processLog(LogType.Warning, message.ToString());
            defaultLogger.LogWarning(tag, message, context);
        }

        /// <summary>
        ///   <para>A variant of ILogger.Log that logs an error message.</para>
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void LogError(string tag, object message)
        {
            processLog(LogType.Error, message.ToString());
            defaultLogger.LogError(tag, message);
        }

        /// <summary>
        ///   <para>A variant of ILogger.Log that logs an error message.</para>
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void LogError(string tag, object message, UnityEngine.Object context)
        {
            processLog(LogType.Error, message.ToString());
            defaultLogger.LogError(tag, message, context);
        }

        /// <summary>
        ///   <para>Logs a formatted message.</para>
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void LogFormat(LogType logType, string format, params object[] args)
        {
            processLog(logType, String.Format(format, args));
            defaultLogger.LogFormat(logType, format, args);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            processLog(logType, String.Format(format, args));
            defaultLogger.LogFormat(logType, context, format, args);
        }

        /// <summary>
        ///   <para>A variant of ILogger.Log that logs an exception message.</para>
        /// </summary>
        /// <param name="exception"></param>
        public void LogException(System.Exception exception)
        {
            processLog(LogType.Exception, exception);
            defaultLogger.LogException(exception);
        }

        public void LogException(System.Exception exception, UnityEngine.Object context)
        {
            processLog(LogType.Exception, exception);
            defaultLogger.LogException(exception, context);
        }

        private void processLog(LogType logType, object logContent)
        {
            string message = "";
            string stack = "";
            if (logContent is System.Exception)
            {
                message = logContent.GetType().Name;
                stack = UnityEngine.StackTraceUtility.ExtractStringFromException((Exception)logContent);
            }
            else
            {
                message = logContent.ToString();
                stack = UnityEngine.StackTraceUtility.ExtractStackTrace();
            }
            var config = Client.Configuration;
            if (config.AutoNotify && logType.IsGreaterThanOrEqualTo(config.NotifyLevel))
            {
                var logMessage = new UnityLogMessage(message, stack, logType);

                if (Client.UniqueCounter.ShouldSend(logMessage))
                {
                    if (Client.LogTypeCounter.ShouldSend(logMessage))
                    {
                        var severity = config.LogTypeSeverityMapping.Map(logType);
                        var backupStackFrames = new System.Diagnostics.StackTrace(1, true).GetFrames();
                        var forceUnhandled = logType == LogType.Exception && !config.ReportUncaughtExceptionsAsHandled;
                        var exception = Payload.Exception.FromUnityLogMessage(logMessage, backupStackFrames, severity, forceUnhandled);
                        Client.Notify(new Payload.Exception[] { exception }, exception.HandledState, null, logType);
                    }
                }
            }
            else
            {
                Client.Breadcrumbs.Leave(logType.ToString(), Payload.BreadcrumbType.Log, new Dictionary<string, string> {
          { "message", message },
        });
            }
        }
    }
}
