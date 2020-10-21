using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Exceptions
{
    public class InvalidFileExtensionException : Exception
    {
        public InvalidFileExtensionException() : base()
        {

        }
        public InvalidFileExtensionException(string message) : base(message)
        {

        }
    }
}
