using System;
using System.IO;

namespace StudentManagement
{
    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR
    }

    public class Logger
    {
        private static readonly string LogFilePath = "student_management.log";
        private static readonly object lockObj = new object();

        public static void Log(string message, LogLevel level = LogLevel.INFO)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = $"[{timestamp}] [{level}] {message}";

            // Console output
            Console.WriteLine(logMessage);

            // File output
            try
            {
                lock (lockObj)
                {
                    File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }

        public static void Info(string message)
        {
            Log(message, LogLevel.INFO);
        }

        public static void Warning(string message)
        {
            Log(message, LogLevel.WARNING);
        }

        public static void Error(string message)
        {
            Log(message, LogLevel.ERROR);
        }
    }
}