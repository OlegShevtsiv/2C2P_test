﻿using _2C2P_test.BLL.BusinessModels;
using _2C2P_test.BLL.Data;
using _2C2P_test.BLL.DTO;
using _2C2P_test.BLL.DTO.Enums;
using _2C2P_test.BLL.Exceptions;
using _2C2P_test.BLL.Extensions;
using _2C2P_test.BLL.Filters;
using _2C2P_test.BLL.Filters.TransactionFilters;
using _2C2P_test.BLL.Interfaces;
using _2C2P_test.DAL.Models;
using _2C2P_test.DAL.Models.Enums;
using _2C2P_test.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace _2C2P_test.BLL.Implementations
{
    public class TransactionService : Service<Transaction, TransactionDTO, IFilter>, ITransactionService
    {
        public TransactionService() : base()
        {
            this.Repository = _unitOfWork.Transaction;
        }

        public TransactionService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this.Repository = _unitOfWork.Transaction;
        }

        protected override TransactionDTO MapToDto(Transaction entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            TransactionDTO dto = new TransactionDTO
            {
                Id = entity.Id,
                Amount = entity.Amount,
                CurrencyCode = entity.CurrencyCode,
                TransactionDate = entity.TransactionDate,
                Status = (TransactionDTOStatus)entity.Status
            };

            return dto;
        }

        protected override Transaction MapToEntity(TransactionDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Transaction entity = new Transaction
            {
                Id = dto.Id,
                Amount = dto.Amount,
                CurrencyCode = dto.CurrencyCode,
                TransactionDate = dto.TransactionDate,
                Status = (TransactionStatus)dto.Status
            };

            return entity;
        }

        public override void Add(TransactionDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            Transaction checkEntity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Transaction entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override TransactionDTO Get(string id)
        {
            Transaction entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                return new TransactionDTO();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<TransactionDTO> Get(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            Func<Transaction, bool> transactionPredicate = this.GetPredicate(filter);

            List<Transaction> entities = Repository
              .Get(p => transactionPredicate(p))
              .ToList();

            if (!entities.Any())
            {
                return new List<TransactionDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override IEnumerable<TransactionDTO> Get(Func<TransactionDTO, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            Func<Transaction, bool> transactionPredicate = (p => predicate(MapToDto(p)));

            List<Transaction> entities = Repository
              .Get(p => transactionPredicate(p))
              .ToList();

            if (!entities.Any())
            {
                return new List<TransactionDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override IEnumerable<TransactionDTO> GetAll()
        {
            List<Transaction> entities = Repository
              .Get(p => p != null)
              .ToList();

            if (!entities.Any())
            {
                return new List<TransactionDTO>();
            }

            return entities.Select(e => MapToDto(e));
        }

        public override void Remove(string id)
        {
            Transaction entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ArgumentException($"Transaction with id '{id}' doesn't exist.");
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(TransactionDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            Transaction entity = Repository
            .Get(e => e.Id == dto.Id)
            .SingleOrDefault();

            if (entity == null)
            {
                throw new ArgumentException($"Transaction with id '{dto.Id}' doesn't exist.");
            }

            entity.Amount = dto.Amount;
            entity.CurrencyCode = dto.CurrencyCode;
            entity.TransactionDate = dto.TransactionDate;
            entity.Status = (TransactionStatus)dto.Status;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public void UploadFromFile(StreamReader reader, string fileName) 
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            string fileExtension = Path.GetExtension(fileName).ToLower();
            List<TransactionDTO> dtos;

            if (fileExtension == Constants.CsvExtension)
            {
                dtos = this.UploadFromCSV(reader).ToList();
            }
            else if (fileExtension == Constants.XmlExtension)
            {
                dtos = this.UploadFromXML(reader).ToList();
            }
            else 
            {
                throw new InvalidFileExtensionException();
            }

            foreach (var dto in dtos)
            {
                this.Add(dto);
            }
        }

        private IEnumerable<TransactionDTO> UploadFromCSV(StreamReader reader) 
        {
            List<CsvTransactionModel> csvTransactions = new List<CsvTransactionModel>();

            using (TextReader tr = reader)
            {
                //Get header

                Dictionary<string, int> fieldIndexes = new Dictionary<string, int>();
                string header = "";
                if (tr.Peek() > 0)
                {
                    header = reader.ReadLine();
                    if (string.IsNullOrEmpty(header))
                    {
                        throw new InvalidCsvRecord("Csv file must contain header!");
                    }
                    else
                    {
                        List<string> fieldNames = header.Split('"')
                                            .Select(f => f.Trim())
                                            .Where(f => !string.IsNullOrWhiteSpace(f) && f != ",")
                                            .ToList();

                        if (fieldNames.Count != 5)
                        {
                            throw new InvalidCsvRecord($"Csv header must contain 5 fields!");
                        }

                        var CsvTransactionModelProperties = typeof(CsvTransactionModel).GetProperties().Select(p => p.Name).ToList();
                        for (int i = 0; i < fieldNames.Count; i++)
                        {
                            if (CsvTransactionModelProperties.Contains(fieldNames[i]))
                            {
                                fieldIndexes.Add(fieldNames[i], i);
                            }
                            else
                            {
                                throw new InvalidCsvRecord($"Invalid csv header! Unknown field {fieldNames[i]}");
                            }
                        }
                    }
                }
                

                uint lineNumber = 2;
                while (tr.Peek() > 0)
                {
                    csvTransactions.Add(CsvTransactionModel.GetFromCsvRow(reader.ReadLine(), fieldIndexes, lineNumber));
                    lineNumber++;
                }
            }

            IEnumerable<TransactionDTO> transactions;
            if (csvTransactions != null && csvTransactions.Count > 0)
            {
                transactions = csvTransactions.Select(t => t.GetDTO()).ToList();
            }
            else 
            {
                transactions = new List<TransactionDTO>();
            }

            return transactions;
        }

        private IEnumerable<TransactionDTO> UploadFromXML(StreamReader reader)
        {
            List<XmlTransactionModel> xmlTransactions = new List<XmlTransactionModel>();

            string xmlDocumentString = string.Empty;
            if (reader.Peek() >= 0) 
            {
                xmlDocumentString = reader.ReadToEnd();
            }

            using (TextReader sr = new StringReader(xmlDocumentString))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(XmlTransactionModels));
                try
                {
                    xmlTransactions = ((XmlTransactionModels)serializer.Deserialize(sr)).Transactions.ToList();
                }
                catch (InvalidOperationException exc) 
                {
                    throw new InvalidXmlFileException();
                }
            }

            IEnumerable<TransactionDTO> transactions;
            if (xmlTransactions != null && xmlTransactions.Count > 0)
            {
                transactions = xmlTransactions.Select(t => t.GetDTO()).ToList();
            }
            else
            {
                transactions = new List<TransactionDTO>();
            }

            return transactions;
        }

        private Func<Transaction, bool> GetPredicate(IFilter filter)
        {
            Func<Transaction, bool> result = e => true;
            if (filter is TransactionFilterByCurrency)
            {
                if (!string.IsNullOrEmpty((filter as TransactionFilterByCurrency)?.CurrencyCode))
                {
                    result += e => e.CurrencyCode == (filter as TransactionFilterByCurrency).CurrencyCode;
                }
            }
            else if (filter is TransactionFilterByStatus)
            {
                result += e => e.Status == (TransactionStatus)((filter as TransactionFilterByStatus).Status);
            }
            else if (filter is TransactionFilterByDateRange) 
            {
                TransactionFilterByDateRange filterByDateRange = (filter as TransactionFilterByDateRange);
                if (filterByDateRange.MinDate >= SqlDateTime.MinValue.Value && filterByDateRange.MinDate <= SqlDateTime.MaxValue.Value &&
                    filterByDateRange.MaxDate >= SqlDateTime.MinValue.Value && filterByDateRange.MaxDate <= SqlDateTime.MaxValue.Value) 
                {
                    result += e => (e.TransactionDate >= filterByDateRange.MinDate &&
                                    e.TransactionDate <= filterByDateRange.MaxDate);
                }
            }

            return result;
        }
    }
}
