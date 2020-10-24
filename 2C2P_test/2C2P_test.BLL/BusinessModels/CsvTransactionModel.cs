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

        public static CsvTransactionModel GetFromCsvRow(string csvRow, Dictionary<string, int> fieldIndexes, uint lineNumber) 
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

            decimal amount;
            CsvTransactionStatus status;
            DateTime dateTime;
            foreach (var fieldIndex in fieldIndexes)
            {
                if (fieldIndex.Key == nameof(CsvTransactionModel.Id))
                {
                    csvTransactionModel.Id = fields[fieldIndex.Value];
                    continue;
                }

                if (fieldIndex.Key == nameof(CsvTransactionModel.Amount))
                {
                    if (decimal.TryParse(fields[fieldIndex.Value], NumberStyles.Currency, CultureInfo.InvariantCulture, out amount))
                    {
                        csvTransactionModel.Amount = amount;
                        continue;
                    }
                    else
                    {
                        throw new InvalidCsvRecord($"{fieldIndex.Key} field is invalid at line {lineNumber}!");
                    }
                }

                if (fieldIndex.Key == nameof(CsvTransactionModel.CurrencyCode))
                {
                    if (fields[fieldIndex.Value].Length == 3)
                    {
                        csvTransactionModel.CurrencyCode = fields[fieldIndex.Value];
                        continue;
                    }
                    else
                    {
                        throw new InvalidCsvRecord($"Invalid {fieldIndex.Key} field at line {lineNumber}!");
                    }
                }

                if (fieldIndex.Key == nameof(CsvTransactionModel.TransactionDate))
                {
                    if (Regex.IsMatch(fields[fieldIndex.Value], dateTimeRegexPattern)
                        && DateTime.TryParse(fields[fieldIndex.Value],
                                             CultureInfo.GetCultureInfo("es-ES"),
                                             DateTimeStyles.None,
                                             out dateTime)
                        && dateTime <= SqlDateTime.MaxValue.Value
                        && dateTime >= SqlDateTime.MinValue.Value)
                    {
                        csvTransactionModel.TransactionDate = dateTime;
                        continue;
                    }
                    else
                    {
                        throw new InvalidCsvRecord($"Invalid {fieldIndex.Key} field at line {lineNumber}!");
                    }
                }

                if (fieldIndex.Key == nameof(CsvTransactionModel.Status))
                {
                    if (Enum.TryParse(fields[fieldIndex.Value], true, out status))
                    {
                        csvTransactionModel.Status = status;
                        continue;
                    }
                    else
                    {
                        throw new InvalidCsvRecord($"Invalid {fieldIndex.Key} field at line {lineNumber}!");
                    }
                }
            }

            return csvTransactionModel;
        }
        const string dateTimeRegexPattern = @"^([1-9]|([012][0-9])|(3[01]))/([0]{0,1}[1-9]|1[012])/\d\d\d\d (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
    }
}
