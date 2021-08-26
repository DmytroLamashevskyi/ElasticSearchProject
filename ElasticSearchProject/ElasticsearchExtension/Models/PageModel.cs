using Nest;
using System;
using System.Collections.Generic;

namespace ElasticsearchExtension.Models
{
    public class PageModel<T> where T: class
    {  
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get => (int)Math.Ceiling((decimal)Data.Total / PageSize); }
        public int PageSize { get; set; } = 10;
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public Dictionary<string, bool> Filters { set; get; }
        public string Query { set; get; } 

        public ISearchResponse<T> Data;

        public PageModel()
        {
            InitFilters(); 
        }

        public void InitFilters(bool AddnumberTypes=true)
        {
            Filters = new Dictionary<string, bool>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.PropertyType == typeof(string) || AddnumberTypes)
                    Filters.Add(prop.Name, true);
            }
        }

        public void UpdateFilters(Dictionary<string, bool> filters, bool AddnumberTypes = true)
        {
            var temp = new Dictionary<string, bool>(filters);
            InitFilters(AddnumberTypes);
            foreach (var prop in Filters)
            {
                if (temp.ContainsKey(prop.Key))
                    Filters[prop.Key] = true;
                else
                    Filters[prop.Key] = false;
            }
        }
    }
}
