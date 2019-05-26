using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using log4net;
using log4net.Config;

namespace SKS.ParcelLogistics.BusinessLogic
{
    public class BlLogger
    {
        private ILog logger;
        public BlLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var bllogconfig = new XmlDocument();
            bllogconfig.Load(File.OpenRead("log4net.config"));
            XmlConfigurator.Configure(logRepository, bllogconfig["log4net"]);

            logger = LogManager.GetLogger(typeof(BlLogger));

            logger.Info("works");
        }

        public void logError(string message)
        {
            logger.Error("ERROR: "+message);
        }

        public void logWarning(string message)
        {
            logger.Warn("WARNING: "+message);
        }

        public void logInfo(string message)
        {
            logger.Info("INFO: "+message);
        }

        public void logFatal(string message)
        {
            logger.Fatal("FATAL: "+message);
        }


    }
}
