using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2P_test.Loggers.FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string path;
        public FileLoggerProvider(string _path)
        {
            path = _path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        public void Dispose()
        {
        }
    }
}
