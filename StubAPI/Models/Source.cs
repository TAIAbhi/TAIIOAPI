using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace StubAPI.Models
{
    public class Source
    {
        public int sourceId { get; set; }
        public int contactId { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string password { get; set; }
        public string myPassword { get; set; }
        public string friendsPassword { get; set; }
        public string contactName { get; set; }
        public int role { get; set; }
        public string locationId1 { get; set; }
        public string locationId2 { get; set; }
        public string locationId3 { get; set; }
        public string errorMessage { get; set; }
        public string loginId { get; set; }
        public bool skipVideo { get; set; }
        public bool everyVideoCheck { get; set; }
        public int sourceType { get; set; }
        public string macID { get; set; }
        

    }
}