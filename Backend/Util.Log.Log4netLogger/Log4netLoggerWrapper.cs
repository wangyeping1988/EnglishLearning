using log4net.Config;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JasonWang.Util.Log.Log4netLogger
{
    /// <summary>
    /// 基于Log4net的本地日志功能封装类
    /// </summary>
    public class Log4netLoggerWrapper
    {
        private log4net.ILog log = null;

        /// <summary>
        /// 用于锁对象
        /// </summary>
        private static object lockObj = new object();

        private static string defaultLoggerName = "DefaultLogger";

        private static readonly Dictionary<string, Log4netLoggerWrapper> loggerDic = new Dictionary<string, Log4netLoggerWrapper>();

        public static string MessageFormat = "Action:{0},Input:{1},OutPut:{2},Mesage:{3}";

        /// <summary>
        /// 默认的日志实例
        /// </summary>
        /// <returns>日志实例对象</returns>
        public static Log4netLoggerWrapper Instance()
        {
            return Instance(defaultLoggerName);
        }

        public static Log4netLoggerWrapper Instance(string loggerName)
        {
            Log4netLoggerWrapper logger = null;

            lock (lockObj)
            {
                if (loggerDic.ContainsKey(loggerName))
                {
                    logger = loggerDic[loggerName];
                }
                else
                {
                    if (!loggerDic.ContainsKey(loggerName))
                    {
                        logger = new Log4netLoggerWrapper(loggerName);
                        loggerDic.Add(loggerName, logger);
                    }
                }
            }
            return logger;
        }

        static Log4netLoggerWrapper()
        {
            //string configFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\log4net.config";
            //string configFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\log4net.config";
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// 私有化默认构造函数
        /// </summary>
        private Log4netLoggerWrapper(string loggerName)
        {
            if (String.IsNullOrEmpty(loggerName))
            {
                log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            }
            else
            {
                log = log4net.LogManager.GetLogger(loggerName);
            }
        }

        public void Log(object messageObj)
        {
            log.Info(messageObj.ToString());
        }

        public void LogError(object messageObj)
        {
            log.Error(messageObj);
        }

        public void LogError(Exception ex, object messageObj)
        {
            log.Error(messageObj, ex);
        }

        public void LogWarn(object messageObj)
        {
            log.Warn(messageObj);
        }
    }
}