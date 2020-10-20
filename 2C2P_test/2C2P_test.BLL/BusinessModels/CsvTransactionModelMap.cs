using _2C2P_test.BLL.BusinessModels.Enums;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace _2C2P_test.BLL.BusinessModels
{
    public class CsvTransactionModelMap : ClassMap<CsvTransactionModel>
    {
        public CsvTransactionModelMap()
        {
            string dateTimeRegexPattern = @"^([1-9]|([012][0-9])|(3[01]))/([0]{0,1}[1-9]|1[012])/\d\d\d\d (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";

            Map(m => m.Id).Validate(field => !string.IsNullOrEmpty(field));

            Map(m => m.Amount).Validate(field => !string.IsNullOrEmpty(field)
                                                 && decimal.TryParse(field, out _));

            Map(m => m.CurrencyCode).Validate(field => !string.IsNullOrEmpty(field)
                                                       && field.Length == 3);

            Map(m => m.TransactionDate).Validate(field => !string.IsNullOrEmpty(field)
                                                          && Regex.IsMatch(field, dateTimeRegexPattern)
                                                          && DateTime.TryParse(field, out var t)
                                                          && t <= SqlDateTime.MaxValue.Value
                                                          && t >= SqlDateTime.MinValue.Value);

            Map(m => m.Status).Validate(field => !string.IsNullOrEmpty(field) && Enum.TryParse<CsvTransactionStatus>(field, true, out _));
        }
    }
}
