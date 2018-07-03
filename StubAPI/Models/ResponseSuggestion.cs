using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class ResponseSuggestion
    {
        
             public bool ? showMaps { get; set; }
        public string contactNumber { get; set; }
        public string businessName { get; set; }
        public string businessContact { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string microcategory { get; set; }
        public string sourceName { get; set; }
        public string location { get; set; }
        public bool citiLevelBusiness { get; set; }
        public bool ? isValid { get; set; }
        public bool ? usedTagSuggetion { get; set; }
        
        public string comments { get; set; }
        public string contactName { get; set; }
        public int ?suggestionId { get; set; }
        public int ?sourceId { get; set; }
        public int ?contactId { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public bool ? isAChain { get; set; }
        public int ? microcategoryId { get; set; }
        public int ? tagCount { get; set; }
        public string addedWhen { get; set; }

        public int cityId { get; set; }
        public string cityName { get; set; }

        public string lastAddedWhen { get; set; }

        public string contactComment { get; set; }
        public string lastAddedBy{ get; set; }
        public bool ? vendorIsVerified { get; set; }


    }
    public class PageInfo
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int noOfRecord { get; set; }
    }
}