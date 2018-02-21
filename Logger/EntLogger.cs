using log4net;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace EnterpriseLogger
{

    /// <summary>
    /// Log4net wrapper.  Invoke Initialize method to get a loggingInstance. Use or inject this instance 
    /// into each method / class to log exceptions or debug messages to a database.  
    /// </summary>
    public static class EntLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EntLogger));

        internal static bool isInitialized;


        public static ILog Initialize()
        {
            if (isInitialized) { return Log; }

            GlobalContext.Properties["ApplicationName"] = MethodBase.GetCurrentMethod().DeclaringType.ToString();  //AppDomain.CurrentDomain.FriendlyName;
            //Debated whether to use LogicalThreadContext.Properties["CorrelationId"] or custom GlobalContext property
            GlobalContext.Properties["CorrelationId"] = Guid.NewGuid();
            log4net.Config.XmlConfigurator.Configure();
            isInitialized = true;
            return Log;
        }
        /// <summary>
        /// Logs Exception, no need to supply optional CallerXXX parameters, (memberName etc.) See CompilerServices namespace
        /// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=netframework-4.7.1
        /// </summary>
        /// <param name="entLogger"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void LogException(
            ILog entLogger,
            Object message,
            Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)

        {
            //If Not initialized, throw exception.  
            if (entLogger == null || !isInitialized)
            {
                throw new NullReferenceException("Entlogger not initialized, need to call initialize method one time before calling LogException");
            }

            GlobalContext.Properties["MemberName"] = memberName;
            GlobalContext.Properties["SourceFilePath"] = sourceFilePath;
            GlobalContext.Properties["SourceLineNumber"] = sourceLineNumber;
            entLogger.Error(message, exception);

        }

        /// <summary>
        /// Logs Debug data, no need to supply optional CallerXXX parameters, (memberName etc.) See CompilerServices namespace
        /// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=netframework-4.7.1
        /// </summary>
        /// <param name="entLogger"></param>
        /// <param name="message"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void LogDebug(
            ILog entLogger,
            Object message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)

        {
            //If Not initialized, throw exception.  
            if (entLogger == null || !isInitialized)
            {
                throw new NullReferenceException("Entlogger not initialized, need to call initialize method one time before calling LogException");
            }

            GlobalContext.Properties["MemberName"] = memberName;
            GlobalContext.Properties["SourceFilePath"] = sourceFilePath;
            GlobalContext.Properties["SourceLineNumber"] = sourceLineNumber;
            entLogger.Debug(message);
        }
    }

}

