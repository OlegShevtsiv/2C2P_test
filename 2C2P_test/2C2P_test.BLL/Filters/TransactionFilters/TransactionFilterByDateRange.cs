using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace _2C2P_test.BLL.Filters.TransactionFilters
{
    public class TransactionFilterByDateRange : IFilter
    {
        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public TransactionFilterByDateRange(DateTime minDate, DateTime maxDate)
        {
            MinDate = minDate;
            MaxDate = maxDate;
        }
    }
}
