using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace _2C2P_test.Loggers.FileLogger
{
    public class FileLogger : ILogger, ILogger<FileLogger>
    {
        private readonly string filePath;
        public static string OutputDirectoryName = "LogsStorage";
        private static readonly object _lock = new object();

        public FileLogger(string fileName)
        {
            this.filePath = Path.Combine(OutputDirectoryName, fileName);
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    if (!Directory.Exists(OutputDirectoryName))
                    {
                        Directory.CreateDirectory(OutputDirectoryName);
                    }

                    if (!File.Exists(this.filePath))
                    {
                        File.Create(this.filePath).Close();
                    }

                    File.AppendAllText(this.filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
