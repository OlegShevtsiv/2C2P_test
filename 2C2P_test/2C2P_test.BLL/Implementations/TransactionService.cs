using _2C2P_test.BLL.BusinessModels;
using _2C2P_test.BLL.Data;
using _2C2P_test.BLL.DTO;
using _2C2P_test.BLL.Exceptions;
using _2C2P_test.BLL.Extensions;
using _2C2P_test.BLL.Interfaces;
using _2C2P_test.DAL.Models;
using _2C2P_test.DAL.UnitOfWork;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace _2C2P_test.BLL.Implementations
{
    public class TransactionService : Service<Transaction, TransactionDTO>, ITransactionService
    {
        public TransactionService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this.Repository = unitOfWork.Transaction;
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
                Status = entity.Status
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
                Status = dto.Status
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

        public override IEnumerable<TransactionDTO> Get(Func<TransactionDTO, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            Func<Transaction, bool> transactionPredicate = p => predicate(MapToDto(p));

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
            entity.Status = dto.Status;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<TransactionDTO> GetFromCSV(FileStream fileStream) 
        {
            if (fileStream == null) 
            {
                throw new ArgumentNullException(nameof(fileStream));
            }
            
            if (Path.GetExtension(fileStream.Name).ToLower() != Constants.CsvExtension)
            {
                throw new FileExtensionException($"File '{fileStream.Name}' must have {Constants.CsvExtension} extension.");
            }

            if (fileStream.Length > Constants._1MbLength)
            {
                throw new FileSizeException($"File '{fileStream.Name}' must be less than 1 MB.");
            }

            IEnumerable<CsvTransactionModel> csvTransactions;

            using (TextReader textReader = new StreamReader(fileStream)) 
            {
                using (CsvReader csv = new CsvReader(textReader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<CsvTransactionModelMap>();
                    csvTransactions = csv.GetRecords<CsvTransactionModel>();
                }
            }

            IEnumerable<TransactionDTO> transactions = csvTransactions.Select(t => t.GetDTO()).ToList();

            return transactions;
        }

        public IEnumerable<TransactionDTO> GetFromXML(FileStream fileStream) 
        {

            return null;
        }


    }
}
