using _2C2P_test.BLL.BusinessModels.Enums;
using System;

namespace _2C2P_test.BLL.BusinessModels
{
    public class XmlTransactionModel
    {
        public string Id { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime TransactionDate { get; set; }

        public XmlTransactionStatus Status { get; set; }
    }
}
