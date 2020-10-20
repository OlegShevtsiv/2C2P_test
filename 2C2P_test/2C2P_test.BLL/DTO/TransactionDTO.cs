using _2C2P_test.DAL.Models.Enums;
using _2C2P_test.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.DTO
{
    public class TransactionDTO
    {
        public string Id { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionStatus Status { get; set; }

        public TransactionDTO()
        {
           
        }

        public TransactionDTO(string id, decimal amount, string currencyCode, DateTime transactionDate, TransactionStatus status)
        {
            Id = id;
            Amount = amount;
            CurrencyCode = currencyCode;
            TransactionDate = transactionDate;
            Status = status;
        }
    }
}
