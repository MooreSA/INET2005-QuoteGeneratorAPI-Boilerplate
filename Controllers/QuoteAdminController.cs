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
                Console.WriteLine("Uploaded file to: " + uploadManager.Destination);

                qm.quote.image = uploadManager.getFileName(); // filename might be changed

                if (!qm.quote.isValid()) {
                    Console.WriteLine("Quote Author: " + qm.quote.author);
                    Console.WriteLine("Quote Content: " + qm.quote.content);
                    Console.WriteLine("Quote Permalink: " + qm.quote.permaLink);
                    Console.WriteLine("Quote Image: " + qm.quote.image);
                    Console.WriteLine("Model state is not valid");
                return View("QuoteAdmin", qm);
                }
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
