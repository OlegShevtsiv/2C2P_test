using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _2C2P_test.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Hello() 
        {
            return Ok("Hello!");
        }
    }
}
