using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CaloriesCounter.BLL
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app.log");

        static Logger()
        {

            Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
        }

        public static void LogInfo(string message)
        {
            Log("INFO", message);
        }

        public static void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        public static void LogError(string message, Exception ex = null)
        {
            Log("ERROR", $"{message} {ex}");
        }

        private static void Log(string level, string message)
        {
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }
    }
}

