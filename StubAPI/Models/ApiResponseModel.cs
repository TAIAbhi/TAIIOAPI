using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubAPI.Models
{
    public class ServiceabilityResponse
    {
        public int RequestID { get; set; }
        public int ResponseID { get; set; }
        public List<InstallmentDetails> InstallmentDetails { get; set; }
        public decimal NDIBuffer { get; set; }
        public decimal NDIBufferPercentage { get; set; }
        public decimal NetSurplusPreInstallment { get; set; }
        public decimal NetDisposableIncomePreInstallment { get; set; }
        public decimal ApplicantHEMValue { get; set; }
        public decimal CoBorrowerHEMValue { get; set; }
        public decimal CombinedHEMValue { get; set; }
        //public int CalculatedApplicationType { get; set; }
        public bool IsRequestValid { get; set; }
        public string CorrelationID { get; set; }   
        public List<Error> Errors { get; set; }
        public List<Messages> Messages { get; set; }
        public DiscountAndTotals DiscountAndTotals { get; set; }
    }
}