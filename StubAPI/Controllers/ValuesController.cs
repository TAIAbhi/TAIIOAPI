using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StubAPI.Models;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using StubAPI.BAL;
using System.Xml.Serialization;
using System.IO;
using StubAPI.ExceptionLog;
using System.Data;

namespace StubAPI.Controllers
{
    [CustomExceptionFilter]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "Santosh1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        public string singup([FromBody] UserData ApiRequestModel)
        {
            string errorMessage = string.Empty;
            //Check Duplicate mobile and email.
            UserDetails usDetails = new UserDetails();
            errorMessage = usDetails.IsUserExists(ApiRequestModel.MobileNumber, ApiRequestModel.Email);
            if (string.IsNullOrEmpty(errorMessage))
            {
                //Insert user details..                            
                if (usDetails.SaveUsers(ApiRequestModel.MobileNumber, ApiRequestModel.Email, ApiRequestModel.Password, ApiRequestModel.FirstName, ApiRequestModel.LastName, ApiRequestModel.Source))
                {

                    ActivityLogger.ActivityLog("xyz", "PostUser called");
                    errorMessage = "User record added successfuly.";
                }
                else
                {
                    errorMessage = "The system is not responding.";
                }
            }
            return errorMessage;

        }
        [HttpPost]
        public string login([FromBody] UserData ApiRequestModel)
        {
            string errorMessage = string.Empty;
            //Check Duplicate mobile and email.
            UserDetails usDetails = new UserDetails();
            DataTable dtUser = new DataTable();
            dtUser = usDetails.Login(ApiRequestModel.UserId, ApiRequestModel.Password);
            if(dtUser.Rows.Count>0)
            {
                ActivityLogger.ActivityLog("xyz", ApiRequestModel.UserId+" User Login ");
                errorMessage = "Logged in successfuly.";
            }          
            return errorMessage;

        }
        [HttpPost]
        public string updatepassword([FromBody] UserData ApiRequestModel)
        {
            string errorMessage = string.Empty;
            //Check Duplicate mobile and email.
            UserDetails usDetails = new UserDetails();          
            if (usDetails.UpdatePassword(ApiRequestModel.UserId, ApiRequestModel.Password))
            {
                ActivityLogger.ActivityLog("xyz", ApiRequestModel.UserId + " User  details Updated ");
                errorMessage = "Record Updated successfuly.";
            }
            else
            {
                errorMessage = "Invalid Record!";
            }
            return errorMessage;

        }
        [HttpPost]
        public string forgotpassword([FromBody] UserData ApiRequestModel)
        {
            string errorMessage = "This is stub.";
          
            return errorMessage;

        }
        [HttpPost]
        public string verifyotp([FromBody] UserData ApiRequestModel)
        {
            string errorMessage = "This is stub.";

            return errorMessage;

        }

        [HttpPost]
        public string postuserdetails([FromBody] User ApiRequestModel)
        {
            string errorMessage = string.Empty;
            //Check Duplicate mobile and email.
            UserDetails usDetails = new UserDetails();
           // errorMessage= usDetails.IsUserExists(ApiRequestModel.MobileNumber, ApiRequestModel.Email);
            if (string.IsNullOrEmpty(errorMessage))
            {
                //Insert user details..
               // int defaultUserValue = 1;
                string xmlUserDetails = ToXML(ApiRequestModel);
                if (usDetails.SaveUserDetails(xmlUserDetails, ApiRequestModel.Source, ApiRequestModel.UID))
                {
                   
                    ActivityLogger.ActivityLog("xyz", "PostUserDetails called");
                    errorMessage = "User record added successfuly.";
                }
                else
                {
                    errorMessage = "The system is not responding.";
                }
            }
            return errorMessage;

        }

        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }
        // GET api/GetMyCategories/5
        public string GetMyCategories(int id)
        {
            CategoryDetailCounts objCategoryDetailCounts = new CategoryDetailCounts();
            objCategoryDetailCounts.Category = "Food";
            objCategoryDetailCounts.NewReviewCount = 2;
            objCategoryDetailCounts.TotalReviewCount = 22;
            StringContent content = new StringContent(JsonConvert.SerializeObject(objCategoryDetailCounts), Encoding.UTF8, "application/json");
            string stringContent = JsonConvert.SerializeObject(objCategoryDetailCounts);
            return stringContent;            
        }

       // [CustomExceptionFilter]
        [HttpPost]
        public string PostUserBusinessDetails([FromBody] UserBusinessDetails ApiRequestModel)
        {
            string errorMessage = string.Empty;
            //Check Duplicate mobile and email.
            UserDetails usDetails = new UserDetails();
            errorMessage = usDetails.IsUserBusinessExists(ApiRequestModel.UserID, ApiRequestModel.BusinessType);
            if (string.IsNullOrEmpty(errorMessage))
            {
                //Insert user details..
                int defaultUserValue = 1;
                string xmlUserDetails = ToXML(ApiRequestModel);
                if (usDetails.SaveUserBusinessDetails(xmlUserDetails, defaultUserValue))
                {
                    ActivityLogger.ActivityLog("xyz", "PostUserDetails called");
                    errorMessage = "User business record added successfuly.";
                }
                else
                {
                    errorMessage = "The system is not responding.";
                }
            }
            return errorMessage;

        }
        [HttpPost]
        public string PostFOFDetails([FromBody] OtherDetails ApiRequestModel)
        {
            string errorMessage = string.Empty;
            //Check Duplicate mobile and email.
            UserDetails usDetails = new UserDetails();
            errorMessage = "";
           
                //update FOF details..            
               
                if (usDetails.SaveFOFUserDetails(ApiRequestModel.UserId, ApiRequestModel.FOF, ApiRequestModel.MyContacts, ApiRequestModel.NotifyTags, ApiRequestModel.BlockNonRecoTill))
                {

                    ActivityLogger.ActivityLog("xyz", "PostFOFDetails called");
                    errorMessage = "User record updated successfuly.";
                }
                else
                {
                    errorMessage = "The system is not responding.";
                }
           
            return errorMessage;

        }


    }
}
