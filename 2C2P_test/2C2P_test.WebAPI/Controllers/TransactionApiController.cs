using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using _2C2P_test.BLL.BusinessModels;
using _2C2P_test.BLL.DTO;
using _2C2P_test.BLL.DTO.Enums;
using _2C2P_test.BLL.Filters.TransactionFilters;
using _2C2P_test.BLL.Implementations;
using _2C2P_test.BLL.Interfaces;
using _2C2P_test.DAL.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _2C2P_test.BLL.Exceptions;
using _2C2P_test.BLL.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Xml;
using _2C2P_test.HttpModels.ResponseModels;
using _2C2P_test.Loggers.FileLogger;

namespace _2C2P_test.Controllers
{
    [Route("api/transaction/[action]")]
    [ApiController]
    public class TransactionApiController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly ILogger<FileLogger> logger;
        private readonly IHostingEnvironment hostingEnvironment;
        public TransactionApiController(ITransactionService _transactionService,
                                        ILoggerFactory loggerFactory,
                                        IHostingEnvironment _hostingEnvironment)
        {
            this.transactionService = _transactionService;
            this.logger = loggerFactory.CreateLogger<FileLogger>();
            this.hostingEnvironment = _hostingEnvironment;
        }

        /// <summary>
        /// Upload CSV or XML file to database;
        /// Form file hasn't be larger than 1 Mb
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = Constants._1MbLength)]
        public IActionResult UploadFile([Required]IFormFile file)
        {
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    this.transactionService.UploadFromFile(reader, file.FileName);
                }
                return Ok();
            }
            catch (Exception exc)
            {
                if (exc is InvalidFileExtensionException || exc is InvalidCsvRecord || exc is XmlException || exc is InvalidXmlFileException)
                {
                    logger.LogError(exc.Message + $" File name '{file.FileName}'");
                    logger.LogUploadedFile(file, Path.Combine(hostingEnvironment.ContentRootPath, @"Logs\InvalidUploadedFiles"));
                    return BadRequest(exc.Message);
                }

                throw exc;
            }
        }

        /// <summary>
        /// Gets all transactions by currency code
        /// </summary>
        /// <param name="currencyCode">Text in ISO4217 format</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByCurrencyCode([Required]string currencyCode)
        {
            var transactions = this.transactionService.Get(new TransactionFilterByCurrency(currencyCode));
            var result = transactions.Select(t => new TransactionResponseModel(t)).ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// Gets all transactions which date is between two dates
        /// </summary>
        /// <param name="minTime"></param>
        /// <param name="maxTime"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByDateRange([Required] DateTime minTime, [Required] DateTime maxTime)
        {
            var transactions = this.transactionService.Get(new TransactionFilterByDateRange(minTime, maxTime));
            var result = transactions.Select(t => new TransactionResponseModel(t)).ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// Gets all transactions by status
        /// </summary>
        /// <param name="status">0 - Approved, 1 - Failed or Rejected, 2 - Finished or Done </param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetByStatus([Required]TransactionDTOStatus status)
        {
            var transactions = this.transactionService.Get(new TransactionFilterByStatus(status));
            var result = transactions.Select(t => new TransactionResponseModel(t)).ToList();
            return new ObjectResult(result);
        }
    }
}
