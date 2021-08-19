﻿using ElasticsearchExtension;
using ElasticSearchProject.Models;
using Microsoft.AspNetCore.Mvc;
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



        public HomeController(ILogger<HomeController> logger, ElasticClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index(string query, string byname, string byFormerName,
                                            string ByStreetAddress, string ByCity, string ByMarket, string ByState,string curentPage)
        {
            var Page = new PageModel<ISearchResponse<Property>>();
            if (curentPage !=  null)
            {
                Page.CurrentPage = Int32.Parse(curentPage);
            }

            ISearchResponse<Property> results;
            if (!string.IsNullOrWhiteSpace(query))
            {
                var FieldsList = new List<Expression<Func<Property, object>>>();
                if (byname == "on")
                    FieldsList.Add(f=>f.Name);

                if (byFormerName == "on")
                    FieldsList.Add(f => f.FormerName);

                if (ByStreetAddress == "on")
                    FieldsList.Add(f => f.StreetAddress);

                if (ByCity == "on")
                    FieldsList.Add(f => f.City);

                if (ByMarket == "on")
                    FieldsList.Add(f => f.Market);

                if (ByState == "on")
                    FieldsList.Add(f => f.State); 

                results = ElasticSearch.PartSearch<Property>(_client, query, FieldsList, Page.PageSize * (Page.CurrentPage-1), Page.PageSize * (Page.CurrentPage));

            }
            else
            {
                results = ElasticSearch.MatchAll<Property>(_client, query, Page.PageSize * (Page.CurrentPage - 1), Page.PageSize * (Page.CurrentPage));
            }
            Page.Data = results;
            Page.TotalPages = (int)Math.Ceiling((decimal)results.Total / Page.PageSize);
             
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
