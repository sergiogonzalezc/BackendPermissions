using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using NLog;
using static BackendPermissions.Common.Enum;

namespace BackendPermissions.Common
{
    /// <summary>
    /// Clase que maneja el Log
    /// </summary>
    public class ServiceLog
    {
        public static void Write(LogType logType, Exception ex, string category, string message)
        {
            NLog.Logger l = null;

            NLog.LogManager.Configuration.Variables["category"] = category;

            switch (logType)
            {
                case LogType.WebSite:
                    {
                        l = LogManager.GetLogger("WebSite");

                        break;
                    }
                case LogType.ConsoleTask:
                    {
                        l = LogManager.GetLogger("ConsoleTask");

                        break;
                    }
                default:
                    {
                        l = LogManager.GetLogger("WebSite");

                        break;
                    }
            }

            message += " - MSG --> " + ex.Message;

            message = string.Format("[{0}] {1}", category, message);

            if (ex.InnerException != null)
            {
                message += " - INNER --> " + ex.InnerException.ToString();
            }

            if (ex.StackTrace != null)
            {
                message += " - STACK --> " + ex.StackTrace.ToString();
            }

            l.Error(message);
        }

        public static void Write(LogType logType, TraceLevel traceLevel, string category, string message)
        {
            NLog.Logger l = null;

            switch (logType)
            {
                case LogType.WebSite:
                    {
                        l = LogManager.GetLogger("WebSite");

                        break;
                    }
                case LogType.ConsoleTask:
                    {
                        l = LogManager.GetLogger("ConsoleTask");

                        break;
                    }
                default:
                    {
                        l = LogManager.GetLogger("WebSite");

                        break;
                    }
            }

            message = string.Format("[{0}] {1}", category, message);

            switch (traceLevel)
            {
                case TraceLevel.Info:
                    {
                        l.Info(message);

                        break;
                    }
                case TraceLevel.Verbose:
                    {
                        l.Info(message);

                        break;
                    }
                case TraceLevel.Warning:
                    {
                        l.Warn(message);

                        break;
                    }
                case TraceLevel.Error:
                    {
                        l.Error(message);

                        break;
                    }
                default:
                    {
                        l.Info(message);

                        break;
                    }
            }
        }
    }
}
