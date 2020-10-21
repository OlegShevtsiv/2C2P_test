using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Exceptions
{
    public class FileTooLargeSizeException : Exception
    {
        public FileTooLargeSizeException(string message) : base(message)
        {

        }
    }
}
