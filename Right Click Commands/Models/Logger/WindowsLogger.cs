using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Logger
{
    public class WindowsLogger
    {
        //  Variables
        //  =========

        private readonly ILog log;

        private const string DEBUG_FORMAT = "[{0}] [DEBUG] {1}";
        private const string INFO_FORMAT = "[{0}] [INFO] {1}";
        private const string WARN_FORMAT = "[{0}] [WARN] {1}";
        private const string ERROR_FORMAT = "[{0}] [ERROR] {1}";
        private const string FATAL_FORMAT = "[{0}] [FATAL] {1}";
        private const string SCRIPT_FORMAT = "[{0}] [{1}] {2}";
        private const string DATETIME_FORMAT = "dd/MM/yyyy HH:mm:ss";

        //  Constructors
        //  ============

        static WindowsLogger()
        {
            log4net.Util.LogLog.InternalDebugging = true;
            PatternLayout p = new PatternLayout()
            {
                ConversionPattern = "%message%newline"
            };
            p.ActivateOptions();
            RollingFileAppender a = new RollingFileAppender()
            {
                Layout = p,
                File = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Right-Click Commands", "Logs" , "Output.log"),
                RollingStyle = RollingFileAppender.RollingMode.Date,
                MaxSizeRollBackups = 5,
                ImmediateFlush = true,
                PreserveLogFileNameExtension = true,
                StaticLogFileName = true,
            };
            a.ActivateOptions();
            BasicConfigurator.Configure(a);
        }

        public WindowsLogger(Type type)
        {
            log = LogManager.GetLogger(type);
        }

        //  Methods
        //  =======

        public void Debug(string message)
        {
            log.Debug(string.Format(DEBUG_FORMAT, DateTime.Now.ToString(DATETIME_FORMAT), message));
        }

        public void Info(string message)
        {
            log.Info(string.Format(INFO_FORMAT, DateTime.Now.ToString(DATETIME_FORMAT), message));
        }

        public void Warn(string message)
        {
            log.Warn(string.Format(WARN_FORMAT, DateTime.Now.ToString(DATETIME_FORMAT), message));
        }

        public void Error(string message)
        {
            log.Error(string.Format(ERROR_FORMAT, DateTime.Now.ToString(DATETIME_FORMAT), message));
        }

        public void Fatal(string message)
        {
            log.Fatal(string.Format(FATAL_FORMAT, DateTime.Now.ToString(DATETIME_FORMAT), message));
        }

        public void Script(string script, string message)
        {
            log.Info(string.Format(SCRIPT_FORMAT, DateTime.Now.ToString(DATETIME_FORMAT), script, message));
        }
    }
}
