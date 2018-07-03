using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class ContactSuggestions
    {
        
        public string locationId { get; set; }
        public string areaShortCode { get; set; }
        public int sugId { get; set; }
      
        public string source { get; set; }
        public string contact { get; set; }
        public int ? requestID { get; set; }
        public string platForm { get; set; }
        public string contactNumber { get; set; }
        public string sourceImage { get; set; }
        public bool  allowProvideSuggestion { get; set; }
        public int catId { get; set; }
        public bool isValid { get; set; }
        public bool usedTagSuggetion { get; set; }
        public int sourceId { get; set; }
        public int contactId { get; set; }
        public string Category { get; set; }
     
        public string subCategory { get; set; }

      
        public string microcategory { get; set; }
      
        public bool citiLevelBusiness { get; set; }
     
        public string businessName { get; set; }
    
        public string businessContact { get; set; }
      
        public string location1 { get; set; }
        public string location2 { get; set; }
        public string location3 { get; set; }
     
        public string comments { get; set; }

    
        public string location4 { get; set; }
  
        public string location5 { get; set; }
   
        public string location6 { get; set; }
    
        public string contactComments { get; set; }
     
        public int subCategoryId { get; set; }
        public IList<Category> categories { get; set; }

        public string sourceName { get; set; }
        public string contactName { get; set; }
        public string location { get; set; }
        public string category { get; set; }

        public int ? contactLevelUnderstanding { get; set; }
        public int ? notification { get; set; }
       
        public bool ? isContactDetailsAdded { get; set; }
        public bool isAChain { get; set; }

        public int ? sourceType { get; set; }

        public string sourceTypeText { get; set; }
        public int ? city { get; set; }
        public int? platform { get; set; }
    }
}