using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class Category
    {
        public int catId { get; set; }
        public string name { get; set; }
        public int catCount { get; set; }
        public bool isMicroCategoryAvailable { get; set; }
        public IList<SubCategory> subCategories { get; set; }
       


    }
}