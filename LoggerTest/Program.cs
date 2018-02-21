using EnterpriseLogger;
using log4net;
using System;


namespace LoggerTest
{
    public class Program
    {

        static void Main(string[] args)
        {
            //Initialize Logger at startup
            var loggingInstance = EntLogger.Initialize();
            var logger = new LoggerTest();
            logger.Logit(loggingInstance);
        }

        public class LoggerTest
        {

            //Note that loggInstance is injected as a dependency of Logit          
            public void Logit(ILog loggingInstance)
            {
                EntLogger.LogDebug(loggingInstance, "Entering LogIt");
                try
                {
                    throw new Exception("Test Message Text", new Exception("Inner Exception Text"));
                }
                catch (Exception ex)
                {
                    EntLogger.LogException(loggingInstance,ex.Message, ex);
                }

                EntLogger.LogDebug(loggingInstance,"Exiting LogIt");

                ChildClass childClass = new ChildClass();
                childClass.LoggingFromChildClass(loggingInstance);
            }
        }

        public class ChildClass 
        {
            //Note that loggInstance is injected as a dependency of LoggingFromChildClass 
            public void LoggingFromChildClass(ILog loggingInstance)
            {
                EntLogger.LogDebug(loggingInstance, "Entering LoggingFromChildClass");
                try
                {
                    throw new Exception("Test Message Text", new Exception("ChildClass Exception"));
                }
                catch (Exception ex)
                {
                    EntLogger.LogException(loggingInstance, ex.Message, ex);
                }

                EntLogger.LogDebug(loggingInstance, "Exiting LoggingFromChildClass");
            }
        }
    }
}
