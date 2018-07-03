using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class ActivityLogs
    {
        public string Location { get; set; }
        public DateTime ActivityDateTime { get; set; }
        public string Action { get; set; }

      }
}