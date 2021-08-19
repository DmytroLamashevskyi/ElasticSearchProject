using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ElasticsearchExtension
{
    public class ElasticSearch
    {
        public ElasticClient Init(string model)
        {
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(pool)
                .DefaultIndex(model);
            return new ElasticClient(settings);
        }

        public static ISearchResponse<T> PartSearch<T>(ElasticClient client, string query, List<Expression<Func<T, object>>> fieldList = null, int from = 0, int size = 1) where T : class
        {
            if (client == null)
                throw new ArgumentNullException();
            if (String.IsNullOrEmpty(query))
                throw new ArgumentNullException();
            ISearchResponse<T> results;
            if (fieldList == null)
            {
                results = client.Search<T>(q =>
                    q.Query(q =>
                        q.QueryString(qs => qs.Query(query)) 
                    ).From(from).Size(size)
                );
            }
            else
            {
                results = client.Search<T>(q =>
                     q.Query(q => q
                         .QueryString(qs =>
                         {
                             foreach (var arg in fieldList)
                                 qs.Fields(fs => fs.Field(arg));

                             qs.Query(query);

                             return qs;
                         })
                     ).From(from).Size(size)
                 );

            }

            return results;
        }

        public static ISearchResponse<T> Search<T>(ElasticClient client, string query, Expression<Func<T, object>> field, int from = 0, int size = 1) where T : class
        {
            if (client == null)
                throw new ArgumentNullException();
            if (String.IsNullOrEmpty(query))
                throw new ArgumentNullException();
            if (field == null)
                throw new ArgumentNullException();

            ISearchResponse<T> results = client.Search<T>(q =>
                 q.Query(q => q
                     .Match(qs =>
                     {
                         qs.Field(field);
                         qs.Query(query);

                         return qs;
                     })
                 ).From(from).Size(size)
             );
            return results;
        }

        public static ISearchResponse<T> MatchAll<T>(ElasticClient client, string query, int from = 0, int size = 1) where T : class
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
