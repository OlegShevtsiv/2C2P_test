using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Exceptions
{
    public class InvalidXmlFileException : Exception
    {
        public InvalidXmlFileException() : base()
        {

        }

        public InvalidXmlFileException(string message) : base(message)
        {

        }
    }
}
