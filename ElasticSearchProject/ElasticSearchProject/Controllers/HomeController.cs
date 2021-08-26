using ElasticsearchExtension;
using ElasticsearchExtension.Models;
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

        public IActionResult Index(PageModel<Property> page, string submitButton, int? pagenumber)
        {
            switch (submitButton)
            {
                case "Next": { page.CurrentPage = pagenumber.HasValue ? pagenumber.Value + 1 : page.CurrentPage; return SearchPage(page); }
                case "Previous": { page.CurrentPage = pagenumber.HasValue ? pagenumber.Value - 1 : page.CurrentPage; return SearchPage(page); }
                case "Select Page": { page.CurrentPage = pagenumber ?? page.CurrentPage; return SearchPage(page); } 
                case "Search": return SearchPage(page);
                default:
                    return SearchPage(page);
            }
        }

        public IActionResult SearchPage(PageModel<Property> page)
        {
            if (!string.IsNullOrWhiteSpace(page.Query))
            { 
                page.Data = ElasticSearch.FilteringSearch<Property>(_client, page);
            }
            else
            {
                page.Data = ElasticSearch.MatchAll<Property>(_client, page);
            }
            page.UpdateFilters(page.Filters,false);
            return View(page);
        }


        public IActionResult Help()
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
