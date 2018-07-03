using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class Location
    {
        public int locationId { get; set; }
        public string city { get; set; }
        public string area { get; set; }
        public string suburb { get; set; }     
        public string locationName { get; set; }

    }
}