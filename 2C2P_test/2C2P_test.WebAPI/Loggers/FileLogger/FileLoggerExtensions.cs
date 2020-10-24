using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2P_test.Loggers.FileLogger
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory,
                                        string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }

        public static async void LogUploadedFile(this ILogger<FileLogger> logger, IFormFile file, string logDirName = "InvalidUploadedFiles") 
        {
            string fileName = file.FileName;
            string path;
            string fileExtension;
            int index = 1;

            string directoryPath = Path.Combine(FileLogger.OutputDirectoryName, logDirName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            while (File.Exists(Path.Combine(directoryPath, fileName)))
            {
                fileExtension = Path.GetExtension(fileName);
                fileName = fileName.Replace(fileExtension, string.Empty);
                fileName += index.ToString();
                fileName += fileExtension;
            }

            path = Path.Combine(directoryPath, fileName);

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}
