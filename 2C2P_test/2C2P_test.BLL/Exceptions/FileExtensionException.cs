using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Exceptions
{
    public class FileExtensionException : Exception
    {
        public FileExtensionException(string message) : base(message)
        {

        }
    }
}
