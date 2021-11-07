using System;
using Microsoft.AspNetCore.Mvc;
using QuoteGeneratorAPI.Models;

namespace QuoteGeneratorAPI.Controllers {

    public class QuoteAdminController : Controller {


        [HttpGet]
        public IActionResult Index() {
            return View();
        }

    }
}
