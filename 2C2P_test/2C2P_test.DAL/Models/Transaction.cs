using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.DAL.Models
{
    public sealed class Transaction
    {
        public string Id { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime TransactionDate { get; set; }

        public object Status { get; set; }

        public Transaction()
        {

        }
    }
}
