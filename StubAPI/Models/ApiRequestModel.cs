using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class ServiceabilityRequest
    {
        public ServiceabilityRequestSummary ServiceabilityRequestSummary { get; set; }
        public Adjustments Adjustments { get; set; }
        public List<ApplicantDataIndividual> ApplicantDataIndividual { get; set; }
    }
}