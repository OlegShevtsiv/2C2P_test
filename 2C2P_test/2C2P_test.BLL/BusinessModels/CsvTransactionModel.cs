using _2C2P_test.BLL.BusinessModels.Enums;
using _2C2P_test.BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2C2P_test.BLL.BusinessModels
{
    public class CsvTransactionModel
    {
        public string Id { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public DateTime TransactionDate { get; set; }

        public CsvTransactionStatus Status { get; set; }

        public static CsvTransactionModel GetFromCsvRow(string csvRow, uint lineNumber) 
        {
            if (string.IsNullOrEmpty(csvRow)) 
            {
                throw new InvalidCsvRecord($"Csv record at {lineNumber} is empty!");
            }

            List<string> fields = csvRow.Split('"')
                                        .Select(f => f.Trim())
                                        .Where(f => !string.IsNullOrWhiteSpace(f) && f != ",")
                                        .ToList();

            if (fields.Count != 5) 
            {
                throw new InvalidCsvRecord($"Csv record at line {lineNumber} must contain 5 fields!");
            }

            CsvTransactionModel csvTransactionModel = new CsvTransactionModel();

            csvTransactionModel.Id = fields[0];

            decimal amount;
            if (decimal.TryParse(fields[1], NumberStyles.Currency, CultureInfo.InvariantCulture, out amount))
            {
                csvTransactionModel.Amount = amount;
            }
            else 
            {
                throw new InvalidCsvRecord($"Amount field is invalid at line {lineNumber}!");
            }

            if (fields[2].Length == 3)
            {
                csvTransactionModel.CurrencyCode = fields[2];
            }
            else 
            {
                throw new InvalidCsvRecord($"Invalid CurrencyCode field at line {lineNumber}!");
            }


            if (Regex.IsMatch(fields[3], dateTimeRegexPattern)
                && DateTime.TryParse(fields[3], out var dateTime)
                && dateTime <= SqlDateTime.MaxValue.Value
                && dateTime >= SqlDateTime.MinValue.Value)
            {
                csvTransactionModel.TransactionDate = dateTime;
            }
            else 
            {
                throw new InvalidCsvRecord($"Invalid CurrencyCode field at line {lineNumber}!");
            }


            if (Enum.TryParse<CsvTransactionStatus>(fields[4], true, out var status))
            {
                csvTransactionModel.Status = status;
            }
            else 
            {
                throw new InvalidCsvRecord($"Invalid Status field at line {lineNumber}!");
            }


            return csvTransactionModel;
        }
        const string dateTimeRegexPattern = @"^([1-9]|([012][0-9])|(3[01]))/([0]{0,1}[1-9]|1[012])/\d\d\d\d (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
    }
}
