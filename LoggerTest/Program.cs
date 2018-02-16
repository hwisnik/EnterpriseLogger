using System;
using EnterpriseLogger;
using log4net;

namespace LoggerTest
{
    public class Program
    {

        static void Main(string[] args)
        {
            var logger = new LoggerTest();
            logger.Logit();
        }



        public class LoggerTest
        {
            private readonly ILog _log = EntLogger.LoggingInstance;
            public void Logit()
            {
                try
                {
                    throw new Exception("Test Message Text",new Exception("Inner Exception Text"));
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message,ex);
                }
            }
        }
    }
}
