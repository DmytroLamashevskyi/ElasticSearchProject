using ElasticSearchProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ElasticClient _client;

        public HomeController(ILogger<HomeController> logger, ElasticClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index(string query)
        {
            ISearchResponse<Property> results;
            if (!string.IsNullOrWhiteSpace(query))
            {
                results = _client.Search<Property>(s => s
                    .Query(q => q
                        .Match(t => t
                            .Field(f => f.Name)
                            .Query(query)
                        )
                    )
                );
            }
            else
            {
                results = _client.Search<Property>(s => s
                    .Query(q => q
                        .MatchAll()
                    )
                );
            }
            return View(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
