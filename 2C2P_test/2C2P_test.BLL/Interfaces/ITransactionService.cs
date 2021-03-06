﻿using _2C2P_test.BLL.DTO;
using _2C2P_test.BLL.Filters;
using _2C2P_test.DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _2C2P_test.BLL.Interfaces
{
    public interface ITransactionService : IService<TransactionDTO, IFilter>
    {
        void UploadFromFile(StreamReader reader, string fileName);
    }
}
