using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using System.Threading;

namespace Microservices.Common
{
    public class Log
    {
        public static readonly Log Logger = new Log();
        private ILoggerRepository repository;
        private Log()
        {
            var file = new FileInfo(Path.Combine(Environment.CurrentDirectory, "log4net.config"));
            if (!file.Exists)
            {
                var asm = typeof(Log).Assembly;
                using (Stream stream = asm.GetManifestResourceStream(asm.GetName().Name + ".log4net.config"))
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(file.FullName))
                    writer.Write(reader.ReadToEnd());
            }
            repository = LogManager.CreateRepository("Moons");
            XmlConfigurator.Configure(repository, file);
        }

        private ILog logger
        {
            get
            {
                return LogManager.GetLogger(repository.Name, Thread.CurrentThread.Name ?? "unnamed");
            }
        }

        public static void SetName(string name)
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = name;
        }

        public void Info(string format, params object[] args) { logger.InfoFormat(format, args); }
        public void Info(string message, Exception exception) { logger.Info(message, exception); }
        public void Warn(string format, params object[] args) { logger.WarnFormat(format, args); }
        public void Warn(string message, Exception exception) { logger.Warn(message, exception); }
        public void Error(string format, params object[] args) { logger.ErrorFormat(format, args); }
        public void Error(string message, Exception exception) { logger.Error(message, exception); }
        public void Fatal(string format, params object[] args) { logger.FatalFormat(format, args); }
        public void Fatal(string message, Exception exception) { logger.Fatal(message, exception); }
    }
}
