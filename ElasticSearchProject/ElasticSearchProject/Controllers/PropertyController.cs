using ElasticSearchProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticsearchExtension;
using ElasticsearchExtension.Models;

namespace ElasticSearchProject.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> _logger;
        private readonly ElasticClient _client;
        private readonly string MyTomTomKey;

        public PropertyController(ILogger<PropertyController> logger, ElasticClient client, IConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            MyTomTomKey = configuration.GetSection("TomTomKey").Value;
        }

        public IActionResult View(int id)
        {
            var property = ElasticSearch.SearchByField<Property>(_client,new PageModel<Property>() { Query = id.ToString() } , "PropertyId"); 

            TempData["MyTomTomKey"] = MyTomTomKey;
            return View(property.Documents.FirstOrDefault());
        }
    }
}
