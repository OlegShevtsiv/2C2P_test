using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Filters.TransactionFilters
{
    public class TransactionFilterByCurrency : IFilter
    {
        public string CurrencyCode { get; set; }

        public TransactionFilterByCurrency(string currencyCode)
        {
            CurrencyCode = currencyCode;
        }
    }
}
