using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class SubCategory
    {
        public int subCatId { get; set; }
        public int catId { get; set; }
        public int subCatCount { get; set; }
        public string name { get; set; }
        public string microCategoryToolTip { get; set; }
        public string commentsToolTip { get; set; }
        public  bool isLocal { get; set; }
    }
}