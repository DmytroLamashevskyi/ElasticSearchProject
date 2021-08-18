using ElasticSearchProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchProject.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> _logger;
        private readonly ElasticClient _client;

        public PropertyController(ILogger<PropertyController> logger, ElasticClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult View(int id)
        { 
            var property = _client.Search<Property>(s => s.Query(q => q.Match(m => m.Field(f => f.propertyID == id))));

            return View(property.Documents.FirstOrDefault());
        }
    }
}
