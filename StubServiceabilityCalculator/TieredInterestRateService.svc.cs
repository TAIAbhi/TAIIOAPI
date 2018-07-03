using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BeamWcfWebService;
namespace StubServiceabilityCalculator
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class TieredInterestRateService : ITieredInterestRateService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
        public InterestRateResponse GetInterestRateList(InterestRateRequest Request)
        {
            InterestRateResponse InterestRateResponse = new InterestRateResponse();
            ErrorList[] lstErrorLst = new ErrorList[1];
            InterestList[] lstInterestList = new InterestList[Request.TermListGroup.Length];
            double maxValue =Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["Max"]);
            double minValue = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["Min"]);
            try
            {

                InterestRateResponse.CorrelationID = Request.CorrelationID;
                InterestRateResponse.RequestID = Request.RequestID;
                InterestRateResponse.SuccessFlag = true;   
                for (int i = 0; i < Request.TermListGroup.Length; i++)
                {
                   
                    InterestList InterestList = new InterestList();
                    InterestList.Term = Request.TermListGroup[i].Term;
                    InterestList.MaxInterestRate = maxValue;
                    InterestList.MinInterestRate = minValue;
                    lstInterestList[i] = InterestList;
                }
                InterestRateResponse.InterestListGroup = lstInterestList;
                InterestRateResponse.ErrorListGroup = lstErrorLst;
            }
            catch (Exception ex)
            {
                InterestRateResponse.SuccessFlag = false;
                ErrorList ErrorList = new ErrorList();
                ErrorList.ErrorMessage = ex.Message;
                lstErrorLst[1] = ErrorList;
                InterestRateResponse.ErrorListGroup = lstErrorLst;
            }           
            return InterestRateResponse;
        }
       
    }


}
