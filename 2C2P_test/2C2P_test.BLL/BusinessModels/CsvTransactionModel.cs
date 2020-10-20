using _2C2P_test.BLL.BusinessModels.Enums;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

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

    public class CsvTransactionModelMap : ClassMap<CsvTransactionModel>
    {
        public CsvTransactionModelMap()
        {
            Map(m => m.Id);
            Map(m => m.Amount).Validate(field => !string.IsNullOrEmpty(field));
            Map(m => m.CurrencyCode).Validate(field => !string.IsNullOrEmpty(field));
            Map(m => m.TransactionDate).Validate(field => !string.IsNullOrEmpty(field));
            Map(m => m.Status).Validate(field => !string.IsNullOrEmpty(field) && Enum.TryParse<CsvTransactionStatus>(field, true, out _));
        }
    }

}
