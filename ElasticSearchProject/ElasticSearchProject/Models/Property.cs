using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchProject.Models
{ 
    public class Property
    {
        [JsonProperty("propertyID")]
        public int propertyID { set; get; }
        [JsonProperty("name")]
        public string Name { set; get; }
        [JsonProperty("formerName")]
        public string FormerName { set; get; }
        [JsonProperty("streetAddress")]
        public string StreetAddress { set; get; }
        [JsonProperty("city")]
        public string City { set; get; }
        [JsonProperty("market")]
        public string Market { set; get; }
        [JsonProperty("state")]
        public string State { set; get; }
        [JsonProperty("lat")]
        public double Lat { set; get; }
        [JsonProperty("lng")]
        public double Lng { set; get; }

    }
}
