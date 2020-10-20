using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Exceptions
{
    public class FileSizeException : Exception
    {
        public FileSizeException(string message) : base(message)
        {

        }
    }
}
