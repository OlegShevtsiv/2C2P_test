using _2C2P_test.BLL.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.BLL.Filters.TransactionFilters
{
    public class TransactionFilterByStatus : IFilter
    {
        public TransactionDTOStatus Status { get; set; }

        public TransactionFilterByStatus(TransactionDTOStatus status)
        {
            Status = status;
        }
    }
}
