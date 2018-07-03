using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class LoginResponse
    {
        public int sourceId { get; set; }
        public int contactId { get; set; }
        public string sourceName { get; set; }
        public string mobile { get; set; }       
        public string contactName { get; set; }
        public int role { get; set; }
        public bool skipVideo { get; set; }
        public bool showVideo { get; set; }
        public int ? sourceType { get; set; }
        public string videoUrl { get; set; }

        public string sourceImage { get; set; }
        public string sourceTypeText { get; set; }
        public int? platform { get; set; }


    }
}