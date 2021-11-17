using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

using QuoteGeneratorAPI.Models;


namespace quoteGeneratorAPI.Controllers {
    // attribute is required for Web APIs
    [ApiController]  
    // disabling CORs for requests / responses of public Web API - eliminates CORs errors if this web API is used with a client side web app
    [DisableCors]

    public class QuoteAPIController : ControllerBase {

        private IMemoryCache _cache;

        public QuoteAPIController(IMemoryCache memoryCache) {
            _cache = memoryCache;
        }

        // set to get instead of HttpPost
        [HttpGet]
        // the URL routing - Web APIs must have one
        [Route("data/")]
        public ActionResult<List<Quote>> Get() {
            // QuoteManager qm = new QuoteManager(_cache);
            QuoteManager qm = new QuoteManager();
            try {
                return qm.GetQuotes();
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("data/{count}")]
        public ActionResult<List<Quote>> Get(int count) {
            // QuoteManager qm = new QuoteManager(_cache);
            QuoteManager qm = new QuoteManager();
            try {
                return qm.GetQuotes(count);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
    
}
