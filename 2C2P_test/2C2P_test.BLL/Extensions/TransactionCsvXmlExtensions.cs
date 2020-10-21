using _2C2P_test.BLL.BusinessModels;
using _2C2P_test.BLL.DTO;
using _2C2P_test.BLL.DTO.Enums;
using _2C2P_test.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace _2C2P_test.BLL.Extensions
{
    internal static class TransactionCsvXmlExtensions
    {
        public static TransactionDTO GetDTO(this CsvTransactionModel csv) 
        {
            TransactionDTO dto = new TransactionDTO(csv.Id, 
                                                    csv.Amount, 
                                                    csv.CurrencyCode, 
                                                    csv.TransactionDate, 
                                                    (TransactionDTOStatus)csv.Status);
            return dto;
        }

        public static TransactionDTO GetDTO(this XmlTransactionModel xml) 
        {
            TransactionDTO dto = new TransactionDTO(xml.Id,
                                                    xml.PaymentDetails.Amount,
                                                    xml.PaymentDetails.CurrencyCode,
                                                    xml.TransactionDate,
                                                    (TransactionDTOStatus)xml.Status);
            return dto;
        }
    }
}
