using _2C2P_test.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _2C2P_test.DAL.Models
{
    public sealed class Transaction
    {
        public string Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }

        public Transaction()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Transaction(decimal amount, string currencyCode, DateTime transactionDate, TransactionStatus status) : this()
        {
            Amount = amount;
            CurrencyCode = currencyCode;
            TransactionDate = transactionDate;
            Status = status;
        }
    }
}
