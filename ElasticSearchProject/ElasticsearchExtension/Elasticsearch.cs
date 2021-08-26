﻿using Elasticsearch.Net;
using ElasticsearchExtension.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ElasticsearchExtension
{
    public class ElasticSearch
    {
        public ElasticClient Init(string model, string url)
        {
            var pool = new SingleNodeConnectionPool(new Uri(url));
            var settings = new ConnectionSettings(pool)
                .DefaultIndex(model);
            return new ElasticClient(settings);
        }

        public static ISearchResponse<T> PartSearch<T>(ElasticClient client, PageModel<T> page) where T : class
        {
            if (page == null)
                throw new ArgumentNullException(); 
            if (client == null)
                throw new ArgumentNullException();
            if (String.IsNullOrEmpty(page.Query))
                throw new ArgumentNullException();

            List<string> fieldList = page.Filters.Select(kvp => kvp.Key).ToList();
            int from = page.PageSize * (page.CurrentPage - 1);
            int size = page.PageSize * (page.CurrentPage);
            ISearchResponse<T> results;

            if (fieldList == null)
            {
                results = client.Search<T>(q =>
                    q.Query(q =>
                        q.QueryString(qs => qs.Query(page.Query))
                    ).From(from).Size(size)
                );
            }
            else
            {
                List<PropertyInfo> fildArray = new List<PropertyInfo>();
                foreach (var arg in fieldList)
                {
                    var fieldString = typeof(T).GetProperty(arg);
                    fildArray.Add(fieldString);

                }

                results = client.Search<T>(q =>
                {
                    q.Query(q => q
                        .QueryString(qs =>
                        {

                            qs.Fields(fildArray.ToArray());
                            qs.Query(page.Query);
                            qs.DefaultOperator(Operator.Or);
                            qs.Lenient(true);

                            return qs;
                        })
                    ).From(from).Size(size);

                    q.Highlight(h =>
                    { 
                        var highlight = new Highlight { Fields = new Dictionary<Field, IHighlightField>() }; 
                        highlight.Fields = new FluentDictionary<Field, IHighlightField>().Add("*", new HighlightField());
                        highlight.PreTags = new List<string>() { "<b>" };
                        highlight.PostTags = new List<string>() { "</b>" };

                        return highlight;
                    });
                    return q;
                });
            }

            return results;
        }

        public static ISearchResponse<T> Search<T>(ElasticClient client, string query, string fieldName = null, int from = 0, int size = 1) where T : class
        {
            if (client == null)
                throw new ArgumentNullException();
            if (String.IsNullOrEmpty(query))
                throw new ArgumentNullException();
            if (fieldName == null)
                throw new ArgumentNullException();

            ISearchResponse<T> results = client.Search<T>(q =>
                 q.Query(q => q
                     .Match(qs =>
                     {
                         var field = new Field(typeof(T).GetProperty(fieldName));
                         qs.Field(field);
                         qs.Query(query);

                         return qs;
                     })
                 ).From(from).Size(size)
             );
            return results;
        }

        public static ISearchResponse<T> MatchAll<T>(ElasticClient client, int from = 0, int size = 1) where T : class
        {
            var results = client.Search<T>(s => s
               .Query(q => q
                   .MatchAll()
               ).From(from).Size(size)
           );
            return results;
        }
    }
}
