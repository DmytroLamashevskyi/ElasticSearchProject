﻿using ElasticSearchProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticsearchExtension;

namespace ElasticSearchProject.Controllers
{
    public class PropertyController : Controller
    {
        private readonly ILogger<PropertyController> _logger;
        private readonly ElasticClient _client;
        public readonly string MyTomTomKey;
        public PropertyController(ILogger<PropertyController> logger, ElasticClient client, IConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            MyTomTomKey = configuration.GetSection("TomTomKey").Value;
        }

        public IActionResult View(int id)
        {
            var property = ElasticSearch.Search<Property>(_client, id.ToString(), f => f.propertyID); 

            TempData["MyTomTomKey"] = MyTomTomKey;
            return View(property.Documents.FirstOrDefault());
        }
    }
}
