using Elasticsearch.Net;
using Nest;
using System;

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
    }
}
