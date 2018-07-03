using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class City
    {
        public int cityId { get; set; }       
       public string cityName { get; set; }
       public double minGeoLat { get; set; }
       public double maxGeoLat { get; set; }
       public double minGeoLong { get; set; }
       public double maxGeoLong { get; set; }
        public string cityURL { get; set; }
    }
}