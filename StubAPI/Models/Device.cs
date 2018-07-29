using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class Device
    {
        public int uid { get; set; }
        public string deviceId { get; set; }

        public string type { get; set; }

        public string token { get; set; }

        public int contactId { get; set; }

        public string modelAndOS { get; set; }


    }
}