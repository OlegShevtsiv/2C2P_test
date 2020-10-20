using _2C2P_test.BLL.BusinessModels.Enums;
using CsvHelper.Configuration.Attributes;
using System;

namespace _2C2P_test.BLL.BusinessModels
{
    public class CsvTransactionModel
    {
        [Index(0)]
        public string Id { get; set; }

        [Index(1)]
        public decimal Amount { get; set; }

        [Index(2)]
        public string CurrencyCode { get; set; }

        [Index(3)]
        public DateTime TransactionDate { get; set; }

        [Index(4)]
        public CsvTransactionStatus Status { get; set; }
    }
}
