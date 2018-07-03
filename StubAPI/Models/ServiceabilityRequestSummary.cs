using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class ServiceabilityRequestSummary
    {
         public int RequestID{get;set;}            
                      
         public int RequestType{get;set;}            
              
         public int ApplicationType{get;set;}
         public decimal CspHemValue { get; set; }
         public string CorrelationID { get; set; }
         public int? ApplicationID { get; set; }
         public List<TermInstallments> TermInstallments { get; set; }
       

    }
}