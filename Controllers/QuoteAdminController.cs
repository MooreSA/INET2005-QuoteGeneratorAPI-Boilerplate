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
            return View("QuoteAdmin", new QuoteManager());
        }

        // Serve the add quote form
        [HttpGet]
        [Route("/quoteAdmin")]
        public IActionResult QuoteAdmin() {
            QuoteManager qm = new QuoteManager();
            return View("QuoteAdmin", qm);
        }

        // Add a new quote to the database
        [HttpPost]
        [Route("/addquote")]
        public IActionResult AddQuote(QuoteManager qm, IFormFile Image) {
            UploadManager uploadManager = new UploadManager(env, UPLOAD_PATH);
            int result = uploadManager.uploadFile(Image);
            if (result == UploadManager.SUCCESS) {
                // Add the quote to the db
                qm.quote.Image = uploadManager.Destination; // filename might be changed
                qm.addQuote();
                return RedirectToAction("Index"); // Redirect to the index page
            } else {
                Console.WriteLine("Error uploading file");
                Console.WriteLine("ERROR CODE: " + result);
                return View("QuoteAdmin", qm);
            }
        }

        [HttpPost]
        [Route("/deletequote")]
        public IActionResult DeleteQuote(QuoteManager qm) {
            Console.WriteLine("Deleting quote: " + qm.id);
            qm.deleteQuote();
            return RedirectToAction("Index");
        }
    }
}
