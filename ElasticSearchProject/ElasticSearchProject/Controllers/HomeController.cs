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

        public IActionResult Index(PageModel<Property> page, string submitButton, int? pagenumber)
        {
            switch (submitButton)
            {
                case "Next":
                    return NextPage(page,pagenumber);
                case "Previous":
                    return PrevPage(page, pagenumber); 
                case "Select Page":
                    return SelectPage(page, pagenumber);
                case "Search":
                    return SearchPage(page);
                default:
                    return SearchPage(page);
            }
        }

        private IActionResult SelectPage(PageModel<Property> page, int? pagenumber)
        { 
            page.CurrentPage = pagenumber.Value;
            return SearchPage(page);
        }

        private IActionResult PrevPage(PageModel<Property> page, int? pagenumber)
        {
            page.CurrentPage = pagenumber.Value-1;
            return SearchPage(page);
        }

        private IActionResult NextPage(PageModel<Property> page, int? pagenumber)
        {
            page.CurrentPage = pagenumber.Value + 1;
            return SearchPage(page);
        }

        public IActionResult SearchPage(PageModel<Property> page)
        {
            ISearchResponse<Property> results;
            if (!string.IsNullOrWhiteSpace(page.Query))
            {
                var FieldsList = new List<string>();

                foreach (var pair in page.Filters)
                {
                    if (pair.Value)
                        FieldsList.Add(pair.Key);
                }
                results = ElasticSearch.PartSearch<Property>(_client, page.Query, FieldsList, page.PageSize * (page.CurrentPage - 1), page.PageSize * (page.CurrentPage));
                page.UseMetadata = true;
            }
            else
            {
                results = ElasticSearch.MatchAll<Property>(_client, page.PageSize * (page.CurrentPage - 1), page.PageSize * (page.CurrentPage));
                page.UseMetadata = false;
            }
            page.Data = results;
            page.UpdateFilters(page.Filters);
            page.TotalPages = (int)Math.Ceiling((decimal)results.Total / page.PageSize);
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
