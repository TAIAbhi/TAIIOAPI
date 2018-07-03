using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class CustomResponseMessage
    {
        public string action { get; set; }
        public object message { get; set; }
    }
    public class CustomDataResponseMessage
    {
        public string action { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    public class CustomDataResponseMessageForSuggestion
    {
        public string action { get; set; }
        public string message { get; set; }
        public object pageInfo { get; set; }
        public object data { get; set; }
    }

    public class CustomResponseMessageForDevice
    {
        public string action { get; set; }
        public object message { get; set; }

        public int ? uid { get; set; }
    }
}