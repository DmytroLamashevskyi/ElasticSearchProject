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

        public IActionResult Index(PageModel<Property> page)
        { 
            ISearchResponse<Property> results;
            if (!string.IsNullOrWhiteSpace(page.Query))
            {
                var FieldsList = new List<string>();

                foreach (var pair in page.Filters)
                {
                    if (pair.Value == "on")
                    FieldsList.Add(pair.Key); 
                } 
                results = ElasticSearch.PartSearch<Property>(_client, page.Query, FieldsList, page.PageSize * (page.CurrentPage - 1), page.PageSize * (page.CurrentPage));
                 
            }
            else
            { 
                results = ElasticSearch.MatchAll<Property>(_client, page.PageSize * (page.CurrentPage - 1), page.PageSize * (page.CurrentPage));
            }
            page.Data = results;
            page.TotalPages = (int)Math.Ceiling((decimal)results.Total / page.PageSize); 
            return View(page);
        } 

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PreviousPage(PageModel<Property> page)
        {
            page.CurrentPage--;
            return Index(page);
        }
        public IActionResult NextPage(PageModel<Property> page)
        {
            page.CurrentPage++;
            return Index(page);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
