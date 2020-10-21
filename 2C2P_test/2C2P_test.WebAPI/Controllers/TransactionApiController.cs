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

namespace _2C2P_test.Controllers
{
    [Route("api/transaction/[action]")]
    [ApiController]
    public class TransactionApiController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly IHostingEnvironment hostingEnvironment;
        public TransactionApiController(ITransactionService _transactionService, IHostingEnvironment _hostingEnvironment)
        {
            this.transactionService = _transactionService;
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
                List<TransactionDTO> dtos;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    dtos = this.transactionService.UploadFromFile(reader, file.FileName).ToList();
                }

                foreach(var dto in dtos)
                {
                    this.transactionService.Add(dto);
                }

                return Ok();
            }
            catch (InvalidFileExtensionException exc)
            {
                return BadRequest(exc.Message);
            }
            catch (FileTooLargeSizeException exc)
            {
                return BadRequest(exc.Message);
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
            var result = this.transactionService.Get(new TransactionFilterByCurrency(currencyCode));
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
            var result = this.transactionService.Get(new TransactionFilterByDateRange(minTime, maxTime));
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
            var result = this.transactionService.Get(new TransactionFilterByStatus(status));
            return new ObjectResult(result);
        }
    }
}
