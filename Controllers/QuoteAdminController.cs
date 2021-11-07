using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using QuoteGeneratorAPI.Models;

namespace QuoteGeneratorAPI.Controllers {

    public class QuoteAdminController : Controller {

        private IWebHostEnvironment env;
        private const string UPLOAD_PATH = "uploads";

        public QuoteAdminController(IWebHostEnvironment env) {
            this.env = env;
        }


        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        // Serve the add quote form
        [HttpGet]
        [Route("/addquote")]
        public IActionResult AddQuote() {
            Quote quote = new Quote();
            return View("Add", quote);
        }

        // Add a new quote to the database
        [HttpPost]
        [Route("/addquote")]
        public IActionResult AddQuote(Quote quote, IFormFile Image) {
            Console.WriteLine("File: " + Image);
            UploadManager uploadManager = new UploadManager(env, UPLOAD_PATH);
            int result = uploadManager.uploadFile(Image);
            if (result == UploadManager.SUCCESS) {
                quote.Image = Image.FileName;
                QuoteManager quoteManager = new QuoteManager();
                quoteManager.addQuote(quote);
                return RedirectToAction("Index");
            } else {
                Console.WriteLine("Error uploading file");
                Console.WriteLine("ERROR CODE: " + result);
                return View("Add", quote);
            }
        }

    }
}
