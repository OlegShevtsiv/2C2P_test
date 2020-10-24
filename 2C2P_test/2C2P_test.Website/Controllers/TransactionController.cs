using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _2C2P_test.Website.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.IO;
using System.Globalization;
using _2C2P_test.Website.Configs;

namespace _2C2P_test.Website.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
        }

        public IActionResult FileUploader()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile uploadedFile) 
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("Upload ----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    MemoryStream memoryStream = new MemoryStream();

                    uploadedFile.CopyTo(memoryStream);

                    content.Add(new StreamContent(memoryStream), "file", uploadedFile.FileName);

                    using (var message = await client.PostAsync(Urls.ApiUrl + "/api/transaction/UploadFile", content))
                    {
                        var input = await message.Content.ReadAsStringAsync();
                        var inpurt =  message.Content;

                    }
                }
            }

            return View("FileUploader");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
