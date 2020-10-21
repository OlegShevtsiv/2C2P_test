using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Exceptions
{
    public class InvalidCsvRecord : Exception
    {
        public InvalidCsvRecord(): base()
        {

        }


        public InvalidCsvRecord(string message) : base(message)
        {

        }
    }
}
