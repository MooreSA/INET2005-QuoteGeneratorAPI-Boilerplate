using System;
using Microsoft.AspNetCore.Mvc;
using QuoteGeneratorAPI.Models;

namespace QuoteGeneratorAPI.Controllers {

    public class QuoteAdminController : Controller {

        public IActionResult Index() {
            return View();
        }

    }
}
