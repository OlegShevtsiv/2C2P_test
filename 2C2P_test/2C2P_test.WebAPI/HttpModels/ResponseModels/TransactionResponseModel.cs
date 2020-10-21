using _2C2P_test.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2P_test.HttpModels.ResponseModels
{
    public class TransactionResponseModel
    {
        public string Id { get; set; }

        public string Payment { get; set; }

        public string Status { get; set; }

        public TransactionResponseModel()
        {

        }

        public TransactionResponseModel(string id, string payment, string status)
        {
            Id = id;
            Payment = payment;
            Status = status;
        }

        public TransactionResponseModel(TransactionDTO transaction)
        {
            this.Id = transaction.Id;
            this.Payment = $"{transaction.Amount} {transaction.CurrencyCode}";
            this.Status = transaction.Status.ToString();
        }
    }
}
