using _2C2P_test.BLL.BusinessModels.Enums;
using System;
using System.Xml.Serialization;

namespace _2C2P_test.BLL.BusinessModels
{
    [Serializable()]
    [XmlRoot("Transactions")]
    public class XmlTransactionModels
    {
        [XmlElement("Transaction")]
        public XmlTransactionModel[] Transactions { get; set; }
    }

    [Serializable()]
    public class XmlTransactionModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("PaymentDetails")]
        public PaymentDetails PaymentDetails { get; set; }

        [XmlElement("TransactionDate", Type = typeof(DateTime))]
        public DateTime TransactionDate { get; set; }

        [XmlElement("Status")]
        public XmlTransactionStatus Status { get; set; }
    }

    [Serializable()]
    public class PaymentDetails 
    {
        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("CurrencyCode")]
        public string CurrencyCode { get; set; }
    }
}
