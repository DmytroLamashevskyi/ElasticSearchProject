using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchProject.Models
{ 
    public class Property
    {
        [PropertyName("propertyID")] 
        public int PropertyId { set; get; }
        [PropertyName("name")]
        public string Name { set; get; }
        [PropertyName("formerName")]
        public string FormerName { set; get; }
        [PropertyName("streetAddress")]
        public string StreetAddress { set; get; }
        [PropertyName("city")]
        public string City { set; get; }
        [PropertyName("market")]
        public string Market { set; get; }
        [PropertyName("state")]
        public string State { set; get; }
        [PropertyName("lat")]
        public double Latitude { set; get; }
        [PropertyName("lng")]
        public double Longitude { set; get; }
         
    }
}
