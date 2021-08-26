using Elasticsearch.Net;
using ElasticSearchProject.Models;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElasticSearchUtil
{
    class Program
    {
        public class Root<T>
        {
            public T property { get; set; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {  
            var path = @"F:\repos\ElasticSearchProject\TestData\properties.json";
            var indexName = "property";
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var connectionSettings = new ConnectionSettings(pool);

            var client = new ElasticClient(connectionSettings);

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var List = JsonConvert.DeserializeObject<Root<Property>[]>(json);

                var listProp = new List<Property>();
                foreach (var l in List) listProp.Add(l.property);
                 
                var response = client.IndexMany<Property>(listProp, indexName);

                Console.WriteLine("Response Errors:" + response.Errors);
            }
            else
            {
                Console.WriteLine("path not found");
                return;

            } 
            Console.WriteLine("Finished");
        }
    }
}
