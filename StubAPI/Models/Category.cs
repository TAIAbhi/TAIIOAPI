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


    public class SuggestionCounts
    {
        public IList<SectionCount> SectionCountData { get; set; }
        public IList<CategoryCount> CategoryCountData { get; set; }

    }
    public class SectionCount
    {
        public int catId { get; set; }
        public string name { get; set; }
        public int ? suggCount { get; set; }

    }
    public class CategoryCount
    {
        public int catId { get; set; }
        public string name { get; set; }
        public int ? subCatId { get; set; }
        public string categoryName { get; set; }
        public int ? suggCount { get; set; }
        public string microCategoryToolTip { get; set; }
        public string commentsToolTip { get; set; }
        public bool isLocal { get; set; }
        


    }

}