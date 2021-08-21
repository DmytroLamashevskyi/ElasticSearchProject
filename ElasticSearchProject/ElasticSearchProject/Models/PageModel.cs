using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ElasticSearchProject.Models
{
    public class PageModel<T> where T: class
    { 
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public Dictionary<string, string> Filters { set; get; }

        public ISearchResponse<T> Data;

        public PageModel()
        {
            Filters = new Dictionary<string, string>();

            foreach(var prop in typeof(T).GetProperties())
            {
                Filters.Add(prop.Name, "on");
            }

        }

        public void UpdateFilters(Dictionary<string, string> filters)
        {
            foreach (var prop in Filters)
            {
                if (filters.ContainsKey(prop.Key))
                    Filters[prop.Key] = "on";
                else
                    Filters[prop.Key] = null;
            }
        }
    }
}
