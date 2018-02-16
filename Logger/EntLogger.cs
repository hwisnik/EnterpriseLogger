using log4net;
using System;

namespace EnterpriseLogger
{
    public static class EntLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EntLogger));

        public static ILog LoggingInstance
        {
            get { return Log; }
        }

        static EntLogger()
        {
            GlobalContext.Properties["ApplicationName"] = AppDomain.CurrentDomain.FriendlyName;
            log4net.Config.XmlConfigurator.Configure();
        }

        //TODO:  Need to implement a correlationId so that you can group multiple logfile entries
    }
}

