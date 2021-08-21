using ElasticsearchExtension;
using ElasticSearchProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ElasticSearchProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ElasticClient _client;

        public HomeController(ILogger<HomeController> logger, ElasticClient client, IMemoryCache memoryCache)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index(string query, Dictionary<string, string> Filters, string curentPage)
        {
            var Page = new PageModel<Property>();
            if (curentPage != null)
            {
                Page.CurrentPage = Int32.Parse(curentPage);
            }

            ISearchResponse<Property> results;
            if (!string.IsNullOrWhiteSpace(query))
            {
                var FieldsList = new List<string>();

                foreach (var pair in Filters)
                {
                    if (pair.Value == "on")
                    FieldsList.Add(pair.Key); 
                }
                 

                results = ElasticSearch.PartSearch<Property>(_client, query, FieldsList, Page.PageSize * (Page.CurrentPage - 1), Page.PageSize * (Page.CurrentPage));

            }
            else
            {
                results = ElasticSearch.MatchAll<Property>(_client, Page.PageSize * (Page.CurrentPage - 1), Page.PageSize * (Page.CurrentPage));
            }
            Page.Data = results;
            Page.TotalPages = (int)Math.Ceiling((decimal)results.Total / Page.PageSize);
            Page.UpdateFilters(Filters);
            return View(Page);
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
