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

    public class FilterOptions
    {
        public int sequienceId { get; set; }
        public string ddValue { get; set; }
        public string ddText { get; set; }
        public string filterType { get; set; }
        public bool isSelected { get; set; }
        public Int16 cityId { get; set; }

    }
}