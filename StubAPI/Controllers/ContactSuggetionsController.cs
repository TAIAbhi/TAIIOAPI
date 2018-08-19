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

using StubAPI.ActionFilters;
using System.Net.Mail;
using System.Web.Script.Serialization;

namespace StubAPI.Controllers
{
    [CustomExceptionFilter]

    public class ContactSuggetionsController : ApiController
    {
        //[HttpPost]
        [Route("api/login")]
        [HttpPost]
        public HttpResponseMessage login(Source objSourcePram)
        {
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            LoginResponse objSource = new LoginResponse();
            Token objToken = new Token();
            TokenResponse objTokenResponse = new TokenResponse();
            CustomResponseMessage custResponse = new CustomResponseMessage();
            custResponse.action = "failure";
            custResponse.message = "Invalid user login Id.Please provide valid login details.";
            string msg = string.Empty;
            byte? platform = null;
            try
            {
                if (!string.IsNullOrEmpty(objSourcePram.loginId.Trim()) && !string.IsNullOrEmpty(objSourcePram.password.Trim()))
                {
                    custResponse = new CustomResponseMessage();
                    objSourcePram.loginId = objUserDetails.MobileFormat(objSourcePram.loginId);
                    Int64 isNumber = 0;
                    Int64.TryParse(objSourcePram.loginId, out isNumber);
                    if (objSourcePram.loginId.Length == 10)
                    {
                        if (isNumber > 0)
                        {
                            int admin = 0;


                            DataTable dtSource = new DataTable();
                            dtSource = objUserDetails.GetSourceDetails(objSourcePram.loginId, objSourcePram.password, out admin);

                            if (dtSource.Rows.Count > 0)
                            {
                                msg = "";
                                objSource.sourceName = Convert.ToString(dtSource.Rows[0]["Name"]);
                                objSource.mobile = Convert.ToString(dtSource.Rows[0]["ContactNumber"]);
                                objSource.contactName = Convert.ToString(dtSource.Rows[0]["ContactName"]);
                                objSource.role = admin;
                                objSource.sourceId = Convert.ToInt32(dtSource.Rows[0]["SourceId"]);
                                objSource.contactId = Convert.ToInt32(dtSource.Rows[0]["ContactId"]);
                                objSource.sourceImage = Convert.ToString(dtSource.Rows[0]["SourceImage"]).Length > 0 ? Convert.ToString(dtSource.Rows[0]["SourceImage"]) : string.Empty;
                                objSource.skipVideo = Convert.ToString(dtSource.Rows[0]["SkipVideo"]).Length > 0 ? Convert.ToBoolean(dtSource.Rows[0]["SkipVideo"]) : false;
                                objSource.sourceTypeText = Convert.ToString(dtSource.Rows[0]["Text"]);
                                objSource.platform = Convert.ToInt32(dtSource.Rows[0]["Platform"]);
                                string macId = Convert.ToString(dtSource.Rows[0]["MacID"]);
                                if (Convert.ToString(dtSource.Rows[0]["SourceType"]).Length > 0)
                                {
                                    objSource.sourceType = Convert.ToInt32(dtSource.Rows[0]["SourceType"]);
                                }

                                string requestMac = objSourcePram.macID == null ? "" : objSourcePram.macID;
                                if (string.IsNullOrEmpty(macId))
                                {
                                    objUserDetails.UpdateLoginCount(objSource.contactId, requestMac);
                                    objSource.showVideo = true;
                                    objSource.videoUrl = "https://www.youtube.com/watch?v=deOTP7VhnWA";
                                }
                                else if (macId == objSourcePram.macID)
                                {
                                    objSource.showVideo = true;
                                    objSource.videoUrl = "https://www.youtube.com/watch?v=deOTP7VhnWA";
                                }
                                else
                                {
                                    objSource.showVideo = false;
                                    objSource.videoUrl = "https://www.youtube.com/watch?v=deOTP7VhnWA";
                                }

                                TokenServices objTokenService = new TokenServices();
                                objToken = objTokenService.GenerateToken(objSource.contactId);
                                objTokenResponse.action = "success";
                                objTokenResponse.message = "Log in successfuly.";
                                objTokenResponse.authToken = objToken.authToken;
                                objTokenResponse.loginDetails = objSource;

                                if (objSource.platform != null)
                                {
                                    platform = Convert.ToByte(objSource.platform);
                                }

                                ActivityLogger.ActivityLog(platform, "", "Login", "Login","login", false, objSource.contactId);
                                return Request.CreateResponse(HttpStatusCode.OK, objTokenResponse);

                            }
                            else
                            {
                                //objSource.ErrorMessage = "Invalid user login.Please provide valid login details.";
                                ActivityLogger.ActivityLog(platform, "", "Login", "Login", "1 Invalid user login.Please provide valid login details.", true, objSource.contactId);
                                custResponse.action = "failure";
                                custResponse.message = "Invalid user login.Please provide valid login details.";
                                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
                            }
                        }
                        else
                        {
                            ActivityLogger.ActivityLog(platform, "", "Login", "Login", "2 Invalid user login.Please provide valid login details.", true, objSource.contactId);
                            //objSource.ErrorMessage = "Invalid user mobile number. Not a number :-" + loginId;
                            custResponse.action = "failure";
                            custResponse.message = "Invalid user login Id.Please provide valid login details.";
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
                        }
                    }
                    else
                    {
                        ActivityLogger.ActivityLog(platform, "", "Login", "Login", "Invalid user mobile number", true, objSource.contactId);
                        //objSource.ErrorMessage = "Invalid user mobile number. :-" + loginId;
                        custResponse.action = "failure";
                        custResponse.message = "Invalid user mobile number";

                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.ActivityLog(platform, "", "Login", "Login", ex.Message, true, objSource.contactId);
                custResponse.action = "failure";
                custResponse.message = ex.Message;
                SendEmail(ex.Message, "Login");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }


            return Request.CreateResponse(HttpStatusCode.OK, custResponse);

            // return objSource;
        }
        [Route("api/categories/{isRequest=isRequest}/{cityId=cityId}/{areaShortCode=areaShortCode}/{location=location}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetCategory(bool? isRequest, int? cityId, string areaShortCode, string location)
        {

            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                dtContact = objUserDetails.GetCategory(null, null, isRequest, cityId, areaShortCode, location).Tables[2];
                IList<Category> items = dtContact.AsEnumerable().Select(row =>
                 new Category
                 {
                     catId = row.Field<int>("CatId"),
                     name = row.Field<string>("Name"),
                     // isMicroCategoryAvailable = row.Field<bool>("IsMicroCategoryAvailable"),
                     catCount = row.Field<int>("CatCount"),
                     subCategories = GeSubCategorybyCatId(row.Field<int>("CatId"), isRequest, cityId, areaShortCode, location)
                 }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {
                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "categories");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }
            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }
        public IList<SubCategory> GeSubCategorybyCatId(int categoryId, bool? isRequest, int? cityId, string areaShortCode, string location)
        {
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            dtContact = objUserDetails.GetSubCategory(categoryId, null, null, isRequest, cityId, areaShortCode, location).Tables[2];
            IList<SubCategory> items = dtContact.AsEnumerable().Select(row =>
             new SubCategory
             {
                 subCatId = row.Field<int>("SubCatId"),
                 name = row.Field<string>("Name"),
                 catId = row.Field<int>("CatId"),
                 //isLocal = row.Field<bool>("IsLocal"),
                 subCatCount = row.Field<int>("SubCatCount")
             }).ToList();
            return items;
        }
        [Route("api/subcat/{categoryId=categoryId}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetSubCate(int? categoryId)
        {

            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            Category objCategory = new Category();

            try
            {


                dtContact = objUserDetails.GetSubCategory(categoryId, null, null, null, null, "areashortCode", "location").Tables[0];
                IList<SubCategory> items = dtContact.AsEnumerable().Select(row =>
                    new SubCategory
                    {
                        subCatId = row.Field<int>("SubCatId"),
                        name = row.Field<string>("Name"),
                        catId = row.Field<int>("CatId"),
                        isLocal = row.Field<bool>("IsLocal"),
                        microCategoryToolTip = row.Field<string>("MicroCategoryToolTip"),
                        commentsToolTip = row.Field<string>("CommentsToolTip")
                    }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "subcat");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }
            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }

        [Route("api/microcat/{subcategoryId=subcategoryId}/{microId=microId}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetMicroCategory(int? subcategoryId, int? microId)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtLocation = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {

                dtLocation = objUserDetails.GetMicroCategory(subcategoryId, microId);
                IList<MicroCategory> items = dtLocation.AsEnumerable().Select(row =>
                 new MicroCategory
                 {
                     microId = row.Field<int>("MicroId"),
                     name = row.Field<string>("Name"),
                     isExample = row.Field<bool?>("IsExample")
                 }).ToList();
                var contact = (from con in items
                               select new
                               {
                                   microId = con.microId,
                                   name = con.name,
                                   isExample = con.isExample
                               }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "microcat");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);

        }


        [Route("api/location/{query=query}/{cityId=cityId}/{locationId=locationId}/{areaShortCode=areaShortCode}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetLocation(string query, int? cityId, int? locationId, string AreaShortCode)
        {
            //string token = string.Empty;
            //token = Request.Headers.GetValues("Token").First();

            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtLocation = new DataTable();
            if (query == "query")
            {
                query = string.Empty;
            }
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                dtLocation = objUserDetails.GetLocation(locationId, query, query, cityId, AreaShortCode);
                IList<Location> items = dtLocation.AsEnumerable().Select(row =>
                 new Location
                 {
                     locationId = row.Field<int>("LocationId"),
                     locationName = row.Field<string>("LocationName"),
                     area = row.Field<string>("Area"),
                     city = row.Field<string>("City"),
                     suburb = row.Field<string>("Suburb"),
                     areaShortCode = row.Field<string>("AreaShortCode")

                 }).ToList();

                var contact = (from con in items
                               select new
                               {
                                   locationId = con.locationId,
                                   locationName = con.locationName != null ? con.locationName.Trim() : "",
                                   area = con.area != null ? con.area.Trim() : "",
                                   city = con.city != null ? con.city : "",
                                   suburb = con.suburb != null ? con.suburb.Trim() : "",
                                   locSuburb = con.locationName != null ? con.locationName.Trim() : "" + " - " + con.suburb != null ? con.suburb.Trim() : ""
                               }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {
                ActivityLogger.ActivityLog(null, "", "Location", "GetLocation", "Location" + query + "#" + cityId.ToString() + "#" + locationId.ToString() + "#" + AreaShortCode, true, 0); //contactId
                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "location");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }


            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }


        [Route("api/suburbs/{cityId=cityId}/{areaShortCode=areaShortCode}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetSuburb(int? cityId, string areaShortCode)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtLocation = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                dtLocation = objUserDetails.GetSuburb(cityId, areaShortCode);
                IList<Location> items = dtLocation.AsEnumerable().Select(row =>
                 new Location
                 {
                     // LocationId = row.Field<int>("LocationId"),
                     suburb = row.Field<string>("Suburb")

                 }).ToList();

                var contact = (from con in items
                               select new
                               {
                                   suburb = con.suburb
                               }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "suburbs");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);

        }

        [Route("api/location")]
        [HttpPost]
        [AuthorizationRequired]
        // public string AddLocation(string Suburb, string LocationName)
        public HttpResponseMessage AddLocation([FromBody] Location ApiRequestModel)
        {
            CustomResponseMessage custMessage = new CustomResponseMessage();
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(ApiRequestModel.suburb.Trim()) && !string.IsNullOrEmpty(ApiRequestModel.locationName.Trim()))
                {
                    UserDetailsWeb objUserDetail = new UserDetailsWeb();
                    if (objUserDetail.SaveLocation(ApiRequestModel.suburb.Trim(), ApiRequestModel.locationName.Trim(), ApiRequestModel.city))
                    {
                        custMessage.action = "Success";
                        custMessage.message = "Location saved sucessfully!.";
                    }
                    else
                    {
                        custMessage.action = "Failure";

                        custMessage.message = "Duplicate location found!.";
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);

                    }

                }
                else
                {
                    custMessage.action = "Failure";
                    custMessage.message = "location details is not provided!.";
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
            }
            catch (Exception ex)
            {

                custMessage.action = "failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "location POST");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }



            return Request.CreateResponse(HttpStatusCode.OK, custMessage);

        }


        [Route("api/suggestion")]
        [HttpPost]
        [AuthorizationRequired]
        public HttpResponseMessage AddSuggestion(ContactSuggestions contactSugg)
        {

            string message = string.Empty;
            CustomResponseMessage custMessage = new CustomResponseMessage();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                contactSugg.businessContact = objUserDetails.MobileFormat(contactSugg.businessContact);
                ContactSuggestions objContactSugg = GetContact(contactSugg.contactId);


                if (!string.IsNullOrEmpty(contactSugg.businessContact))
                {
                    Int64 isNumber = 0;
                    Int64.TryParse(contactSugg.businessContact, out isNumber);
                    if (contactSugg.businessContact.Length >= 10 && isNumber > 0)
                    {
                        if (objUserDetails.SaveContactSuggestions(objContactSugg.sourceId, contactSugg.contactId, contactSugg.catId.ToString(), contactSugg.subCategoryId.ToString(), contactSugg.microcategory, contactSugg.businessName, contactSugg.citiLevelBusiness, contactSugg.businessContact, contactSugg.location, contactSugg.locationId, contactSugg.areaShortCode, contactSugg.comments, "", contactSugg.location4, contactSugg.location5, contactSugg.location6, contactSugg.contactComments, contactSugg.isAChain, contactSugg.platForm, contactSugg.city, contactSugg.requestID, contactSugg.usedTagSuggetion))
                        {
                            ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "AddSugg", "AddSugg", "AddSugg : " + contactSugg.businessName + "#" + contactSugg.businessContact + "#" + contactSugg.microcategory != null? contactSugg.microcategory : contactSugg.subCategory + "#" + contactSugg.location + "#" + contactSugg.usedTagSuggetion.ToString() + "#" + contactSugg.comments + "#" + contactSugg.isAChain.ToString() + "#" + contactSugg.citiLevelBusiness.ToString() + "#" + contactSugg.city.ToString(), false, contactSugg.contactId);
                            custMessage.action = "Success";
                            custMessage.message = "Suggestion Added Successfully!.";
                        }
                        else
                        {
                            ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "AddSugg", "AddSugg", "Failed : Please choose different Business Name and Location. AddSugg: " + contactSugg.businessName + "#" + contactSugg.businessContact + "#" + contactSugg.microcategory != null? contactSugg.microcategory : contactSugg.subCategory + "#" + contactSugg.location + "#" + contactSugg.usedTagSuggetion.ToString() + "#" + contactSugg.comments + "#" + contactSugg.isAChain.ToString() + "#" + contactSugg.citiLevelBusiness.ToString() + "#" + contactSugg.city.ToString(), true, contactSugg.contactId);
                            custMessage.action = "Failure";
                            custMessage.message = "Please choose different Business Name and Location!.";
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                        }
                    }
                    else
                    {
                        ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "AddSugg", "AddSugg", "Invalid Business contact number! AddSugg: " + contactSugg.businessName + "#" + contactSugg.businessContact + "#" + contactSugg.microcategory != null ? contactSugg.microcategory : contactSugg.subCategory + "#" + contactSugg.location + "#" + contactSugg.usedTagSuggetion.ToString() + "#" + contactSugg.comments + "#" + contactSugg.isAChain.ToString() + "#" + contactSugg.citiLevelBusiness.ToString() + "#" + contactSugg.city.ToString(), true, contactSugg.contactId);
                        custMessage.action = "Failure";
                        custMessage.message = "Invalid Business contact number!";
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                    }
                }
                else
                {
                    if (objUserDetails.SaveContactSuggestions(objContactSugg.sourceId, contactSugg.contactId, contactSugg.catId.ToString(), contactSugg.subCategoryId.ToString(), contactSugg.microcategory, contactSugg.businessName, contactSugg.citiLevelBusiness, contactSugg.businessContact, contactSugg.location, contactSugg.locationId, contactSugg.areaShortCode, contactSugg.comments, "", contactSugg.location4, contactSugg.location5, contactSugg.location6, contactSugg.contactComments, contactSugg.isAChain, contactSugg.platForm, contactSugg.city, contactSugg.requestID, contactSugg.usedTagSuggetion))
                    {
                        ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "AddSugg", "AddSugg", "AddSugg : " + contactSugg.businessName + "#" + contactSugg.businessContact + "#" + contactSugg.microcategory != null ? contactSugg.microcategory : contactSugg.subCategory + "#" + contactSugg.location + "#" + contactSugg.usedTagSuggetion.ToString() + "#" + contactSugg.comments + "#" + contactSugg.isAChain.ToString() + "#" + contactSugg.citiLevelBusiness.ToString() + "#" + contactSugg.city.ToString(), false, contactSugg.contactId);
                        custMessage.action = "Success";
                        custMessage.message = "Suggestion Added Successfully!.";
                    }
                    else
                    {
                        ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "AddSugg", "AddSugg", "Failed : Please choose different Business Name and Location. AddSugg: " + contactSugg.businessName + "#" + contactSugg.businessContact + "#" + contactSugg.microcategory != null ? contactSugg.microcategory : contactSugg.subCategory + "#" + contactSugg.location + "#" + contactSugg.usedTagSuggetion.ToString() + "#" + contactSugg.comments + "#" + contactSugg.isAChain.ToString() + "#" + contactSugg.citiLevelBusiness.ToString() + "#" + contactSugg.city.ToString(), true, contactSugg.contactId);
                        custMessage.action = "Failure";
                        custMessage.message = "Please choose different Business Name and Location!.";
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "AddSugg", "AddSugg", "AddSugg: " + contactSugg.businessName + "#" + contactSugg.businessContact + "#" + contactSugg.microcategory != null ? contactSugg.microcategory : contactSugg.subCategory + "#" + contactSugg.location + "#" + contactSugg.usedTagSuggetion.ToString() + "#" + contactSugg.comments + "#" + contactSugg.isAChain.ToString() + "#" + contactSugg.citiLevelBusiness.ToString() + "#" + contactSugg.city.ToString() + ex.Message, true, contactSugg.contactId);
                custMessage.action = "failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "suggestion POST");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }



            return Request.CreateResponse(HttpStatusCode.OK, custMessage);


        }
        public IList<SubCategory> GetAllSubCate(int categoryId, string contactNumber, string token)
        {
            CustomResponseMessage custResponse = new CustomResponseMessage();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            Category objCategory = new Category();

            dtContact = objUserDetails.GetMySuggestionSubCategoryWise(categoryId, contactNumber, token);
            IList<SubCategory> items = dtContact.AsEnumerable().Select(row =>
                new SubCategory
                {
                    subCatId = row.Field<int>("SubCatId"),
                    name = row.Field<string>("Name"),
                    catId = row.Field<int>("CatId")

                }).ToList();
            return items;
        }
        [Route("api/suggestion/{contactNumber=contactNumber}")]
        [HttpGet]
        [AuthorizationRequired]
        public HttpResponseMessage MySuggestion(string contactNumber)
        {
            string token = string.Empty;
            if (contactNumber == "contactNumber")
            {
                token = Request.Headers.GetValues("Token").First();
            }
            string messgae = string.Empty;
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            Category objCategory = new Category();
            try
            {
                dtContact = objUserDetails.GetMySuggestionCategoryWise(contactNumber, token);
                IList<Category> items = dtContact.AsEnumerable().Select(row =>
                 new Category
                 {
                     catId = row.Field<int>("CatId"),
                     name = row.Field<string>("Name"),
                     subCategories = GetAllSubCate(row.Field<int>("CatId"), contactNumber, token)


                 }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "suggestion GET");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
            // var list = new SelectList(items, "CatId", "Name");           
            // return items;

        }

        [Route("api/getrequestsuggestion/{catId=catId}/{subCatId=subCatId}/{sugId=sugId}/{contactId=contactId}/{sourceId=sourceId}/{businessName=businessName}/{isLocal=isLocal}/{location=location}/{microcate=microcate}/{pageSize=pageSize}/{pageNumber=pageNumber}/{microName=microName}/{cityId=cityId}")]
        [HttpGet]
        [AuthorizationRequired]
        public HttpResponseMessage GetRequestSuggestion(int? catId, int? subCatId, int? sugId, int? contactId, int? sourceId, string businessName, bool? isLocal, string location, int? microcate, int? pageSize, int? pageNumber, string microName, int? cityId)
        {
            int totalNumberofRow = 0;
            string token = string.Empty;
            if (contactId == null)
            {
                token = Request.Headers.GetValues("Token").First();
            }

            CustomDataResponseMessageForSuggestion custResponse = new CustomDataResponseMessageForSuggestion();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                dtContact = objUserDetails.GetRequestSuggestion(catId, subCatId, contactId, location, microcate, pageSize, pageNumber, out totalNumberofRow, cityId).Tables[0];
                IList<ResponseSuggestion> items = dtContact.AsEnumerable().Select(row =>
                    new ResponseSuggestion
                    {

                        suggestionId = row.Field<int>("UID"),
                        cityId = row.Field<int>("CityId"),
                        cityName = row.Field<string>("CityName"),
                        contactNumber = row.Field<string>("ContactNumber"),
                        category = row.Field<string>("CategoryName"),
                        categoryId = row.Field<int>("Category"),
                        subCategory = row.Field<string>("SubCategoryName"),
                        subCategoryId = row.Field<int>("SubCategory"),
                        microcategory = row.Field<string>("Microcategory"),
                        microcategoryId = row.Field<int?>("MicrocategoryId"),
                        location = row.Field<string>("Location"),
                        // comments = GetComments(row.Field<string>("Comments"), row.Field<string>("SourceName"), row.Field<string>("ContactName"), row.Field<string>("AddedWhen")),
                        comments = row.Field<string>("Comments"),
                        contactName = row.Field<string>("ContactName"),
                        contactId = row.Field<int?>("ContactId"),

                        sourceId = row.Field<int?>("SourceId"),
                        sourceName = row.Field<string>("SourceName"),
                        addedWhen = row.Field<string>("AddedWhen")
                        //  tagCount = row.Field<int?>("Tag")                  
                    }).ToList();

                var contact = (from con in items
                               select new
                               {
                                   contactNumber = con.contactNumber,
                                   category = con.category,
                                   categoryId = con.categoryId,
                                   subCategory = con.subCategory,
                                   subCategoryId = con.subCategoryId,
                                   microcategory = con.microcategory,
                                   microcategoryId = con.microcategoryId,
                                   location = con.location,
                                   comments = con.comments,
                                   contactName = con.contactName,
                                   contactId = con.contactId,
                                   sourceId = con.sourceId,
                                   sourceName = con.sourceName,
                                   uid = con.suggestionId,
                                   cityId = con.cityId,
                                   cityName = con.cityName,
                                   addedWhen = con.addedWhen
                               }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "getrequestsuggestion");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }

        [Route("api/getsuggestion/{catId=catId}/{subCatId=subCatId}/{sugId=sugId}/{contactId=contactId}/{getall=getall}/{sourceId=sourceId}/{businessName=businessName}/{isLocal=isLocal}/{location=location}/{microcate=microcate}/{pageSize=pageSize}/{pageNumber=pageNumber}/{microName=microName}/{cityId=cityId}")]
        [HttpGet]
        [AuthorizationRequired]
        public HttpResponseMessage Suggestion(int? catId, int? subCatId, int? sugId, int? contactId, int? getall, int? sourceId, string businessName, bool? isLocal, string location, int? microcate, int pageSize, int pageNumber, string microName, int? cityId)
        {
            int totalNumberofRow = 0;
            string token = string.Empty;
            if (contactId == null && getall == null)
            {
                token = Request.Headers.GetValues("Token").First();
            }

            CustomDataResponseMessageForSuggestion custResponse = new CustomDataResponseMessageForSuggestion();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                dtContact = objUserDetails.GetSuggestion(catId, subCatId, sugId, contactId, token, sourceId, businessName, isLocal, location, microcate, pageSize, pageNumber, out totalNumberofRow, microName, cityId).Tables[0];
                IList<ResponseSuggestion> items = dtContact.AsEnumerable().Select(row =>
                    new ResponseSuggestion
                    {
                        contactNumber = row.Field<string>("ContactNumber"),
                        businessName = row.Field<string>("BusinessName"),
                        businessContact = row.Field<string>("BusinessContact"),
                        category = row.Field<string>("CategoryName"),
                        categoryId = row.Field<int>("Category"),
                        subCategory = row.Field<string>("SubCategoryName"),
                        subCategoryId = row.Field<int>("SubCategory"),
                        microcategory = row.Field<string>("Microcategory"),
                        microcategoryId = row.Field<int?>("MicrocategoryId"),
                        location = row.Field<string>("LocationId1"),
                        citiLevelBusiness = row.Field<bool>("CitiLevelBusiness"),
                        // comments = GetComments(row.Field<string>("Comments"), row.Field<string>("SourceName"), row.Field<string>("ContactName"), row.Field<string>("AddedWhen")),
                        comments = row.Field<string>("Comments"),
                        contactName = row.Field<string>("ContactName"),
                        contactId = row.Field<int?>("ContactId"),
                        suggestionId = row.Field<int?>("UID"),
                        isAChain = row.Field<bool?>("IsAChain"),
                        sourceId = row.Field<int?>("SourceId"),
                        sourceName = row.Field<string>("SourceName"),
                        addedWhen = row.Field<string>("AddedWhen")
                        //  tagCount = row.Field<int?>("Tag")                  
                    }).ToList();

                PageInfo objPageinfo = new PageInfo();
                objPageinfo.noOfRecord = totalNumberofRow;
                objPageinfo.pageNumber = pageNumber;
                objPageinfo.pageSize = pageSize;
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.pageInfo = objPageinfo;
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.pageInfo = null;
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.pageInfo = null;
                custResponse.data = null;
                SendEmail(ex.Message, "getsuggestion");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }

        [Route("api/getsuggestionwithcount/{catId=catId}/{subCatId=subCatId}/{sugId=sugId}/{contactId=contactId}/{sourceId=sourceId}/{businessName=businessName}/{isLocal=isLocal}/{location=location}/{microcate=microcate}/{pageSize=pageSize}/{pageNumber=pageNumber}/{microName=microName}/{cityId=cityId}/{areaShortCode=areaShortCode}")]
        [HttpGet]
        [AuthorizationRequired]
        [CustomExceptionFilter]
        public HttpResponseMessage SuggestionWithCount(int? catId, int? subCatId, int? sugId, int? contactId, int? sourceId, string businessName, bool? isLocal, string location, int? microcate, int pageSize, int pageNumber, string microName, int? cityId, string areaShortCode)
        {
            int totalNumberofRow = 0;
            string token = string.Empty;
            if (contactId == null)
            {
                token = Request.Headers.GetValues("Token").First();
            }

            CustomDataResponseMessageForSuggestion custResponse = new CustomDataResponseMessageForSuggestion();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                dtContact = objUserDetails.GetSuggestionWithCount(catId, subCatId, sugId, contactId, token, sourceId, businessName, isLocal, location, microcate, pageSize, pageNumber, out totalNumberofRow, microName, cityId, areaShortCode).Tables[0];
                IList<ResponseSuggestion> items = dtContact.AsEnumerable().Select(row =>
                    new ResponseSuggestion
                    {
                        contactNumber = row.Field<string>("ContactNumber"),
                        businessName = row.Field<string>("BusinessName"),
                        businessContact = row.Field<string>("BusinessContact"),
                        category = row.Field<string>("CategoryName"),
                        categoryId = row.Field<int>("Category"),
                        subCategory = row.Field<string>("SubCategoryName"),
                        subCategoryId = row.Field<int>("SubCategory"),
                        microcategory = row.Field<string>("Microcategory"),
                        microcategoryId = row.Field<int?>("MicrocategoryId"),
                        location = row.Field<string>("LocationId1"),
                        citiLevelBusiness = row.Field<bool>("CitiLevelBusiness"),
                        comments = GetComments(row.Field<string>("Comments"), row.Field<string>("SourceName"), row.Field<string>("ContactName"), row.Field<string>("AddedWhen")),
                        //  comments = row.Field<string>("Comments"),
                        contactName = row.Field<string>("ContactName"),
                        contactId = row.Field<int?>("ContactId"),
                        suggestionId = row.Field<int?>("UID"),
                        isAChain = row.Field<bool?>("IsAChain"),
                        sourceId = row.Field<int?>("SourceId"),
                        sourceName = row.Field<string>("SourceName"),
                        tagCount = row.Field<int?>("Tag"),
                        addedWhen = row.Field<string>("AddedWhen") != null ? row.Field<string>("AddedWhen") : null,
                        cityId = row.Field<int>("CityID"),
                        cityName = row.Field<string>("CityName"),
                        lastAddedWhen = row.Field<string>("lastAddedWhen") != null ? row.Field<string>("lastAddedWhen") : null,
                        lastAddedBy = row.Field<string>("LastAddedBy") != null ? row.Field<string>("LastAddedBy") : null,
                        contactComment = row.Field<string>("ContactComment") != null ? row.Field<string>("ContactComment") : null,
                        vendorIsVerified = row.Field<bool?>("VendorIsVerified"),
                        isValid = row.Field<bool?>("IsValid"),
                        usedTagSuggetion = row.Field<bool?>("UsedTagSuggetion"),
                        showMaps = row.Field<bool?>("ShowMaps"),
                        locationId = row.Field<string>("locationId"),
                        areaShortCode = row.Field<string>("areaShortCode")

                    }).ToList().OrderByDescending(a => a.tagCount).ToList();

                PageInfo objPageinfo = new PageInfo();
                objPageinfo.noOfRecord = totalNumberofRow;
                objPageinfo.pageNumber = pageNumber;
                objPageinfo.pageSize = pageSize;
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.pageInfo = objPageinfo;
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.pageInfo = null;
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.pageInfo = null;
                custResponse.data = null;
                SendEmail(ex.Message, "getsuggestionwithcount");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }
        [Route("api/suggestion")]
        [HttpPut]
        public HttpResponseMessage UpdateSuggestion(ContactSuggestions contactSugg)
        {

            string message = string.Empty;
            CustomResponseMessage custMessage = new CustomResponseMessage();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                ContactSuggestions objContactSugg = GetContact(contactSugg.contactId);
                contactSugg.businessContact = objUserDetails.MobileFormat(contactSugg.businessContact);

                if (!string.IsNullOrEmpty(contactSugg.businessContact))
                {
                    Int64 isNumber = 0;
                    Int64.TryParse(contactSugg.businessContact, out isNumber);
                    if (contactSugg.businessContact.Length == 10 && isNumber > 0)
                    {
                        if (objUserDetails.UpdateContactSuggestions(contactSugg.sugId, objContactSugg.sourceId, contactSugg.contactId, contactSugg.catId.ToString(), contactSugg.subCategoryId.ToString(), contactSugg.microcategory, contactSugg.businessName, contactSugg.citiLevelBusiness, contactSugg.businessContact, contactSugg.location, contactSugg.locationId, contactSugg.areaShortCode, contactSugg.comments, "", contactSugg.location4, contactSugg.location5, contactSugg.location6, contactSugg.contactComments, contactSugg.isAChain, contactSugg.platForm, contactSugg.city))
                        {

                            custMessage.action = "Success";
                            custMessage.message = "Update Successfully!";
                        }
                        else
                        {
                            custMessage.action = "Failure";
                            custMessage.message = "Please choose different Business Name and Location!.";
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                        }
                    }
                    else
                    {
                        custMessage.action = "Failure";
                        custMessage.message = "Invalid Business contact number!";
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);

                    }
                }
                else
                {
                    if (objUserDetails.UpdateContactSuggestions(contactSugg.sugId, objContactSugg.sourceId, contactSugg.contactId, contactSugg.catId.ToString(), contactSugg.subCategoryId.ToString(), contactSugg.microcategory, contactSugg.businessName, contactSugg.citiLevelBusiness, contactSugg.businessContact, contactSugg.location, contactSugg.locationId, contactSugg.areaShortCode, contactSugg.comments, "", contactSugg.location4, contactSugg.location5, contactSugg.location6, contactSugg.contactComments, contactSugg.isAChain, contactSugg.platForm, contactSugg.city))
                    {

                        custMessage.action = "Success";
                        custMessage.message = "Update Successfully!";
                    }
                    else
                    {
                        custMessage.action = "Failure";
                        custMessage.message = "Please choose different Business Name and Location!.";
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "suggestion PUT");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);


        }

        [Route("api/me")]
        [AuthorizationRequired]
        [HttpPut]
        public HttpResponseMessage UpdateMyDetails([FromBody] ContactSuggestions contactSugg)
        {
            CustomResponseMessage custMessage = new CustomResponseMessage();
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(contactSugg.location1) && !string.IsNullOrEmpty(contactSugg.location2) && !string.IsNullOrEmpty(contactSugg.location3))
                    {
                        contactSugg.isContactDetailsAdded = true;
                    }
                    UserDetailsWeb objUserDetails = new UserDetailsWeb();
                    if (objUserDetails.UpdateContact(contactSugg.contactId, contactSugg.location1, contactSugg.location2, contactSugg.location3, contactSugg.comments, contactSugg.contactLevelUnderstanding, contactSugg.notification, contactSugg.isContactDetailsAdded, contactSugg.platform, contactSugg.allowProvideSuggestion))
                    {
                        ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "UpdateMyDetails", 
                            "UpdateMyDetails: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString(), false, contactSugg.contactId);
                        custMessage.action = "Success";
                        custMessage.message = "Contact details updated Successfully!";
                    }
                    else
                    {
                        ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "UpdateMyDetails",
                            "UpdateMyDetails: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString() + "# Could not Updated, please check data and submit again!", true, contactSugg.contactId);
                        custMessage.action = "Failure";

                        custMessage.message = "Could not Updated, please check data and submit again!";
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                    }
                }
                else
                {
                    ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "UpdateMyDetails",
                        "UpdateMyDetails: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString() + "# Validation failed", true, contactSugg.contactId);
                    custMessage.message = "Validation failed";
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
                // TODO: Add insert logic here

                // return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "UpdateMyDetails",
                    "UpdateMyDetails: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString() + "# " + ex.Message, true, contactSugg.contactId);

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "me PUT Update my Details");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }
        [Route("api/me")]
        [AuthorizationRequired]
        [HttpPost]
        public HttpResponseMessage AddContact([FromBody] ContactSuggestions contactSugg)
        {
            CustomResponseMessage custMessage = new CustomResponseMessage();
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    UserDetailsWeb objUserDetails = new UserDetailsWeb();
                    contactSugg.contactNumber = objUserDetails.MobileFormat(contactSugg.contactNumber);
                    Int64 isNumber = 0;
                    Int64.TryParse(contactSugg.contactNumber, out isNumber);
                    if (contactSugg.contactNumber.Length == 10 && isNumber > 0)
                    {
                        if (!string.IsNullOrEmpty(contactSugg.location1) && !string.IsNullOrEmpty(contactSugg.location2) && !string.IsNullOrEmpty(contactSugg.location3))
                        {
                            contactSugg.isContactDetailsAdded = true;
                        }
                        if (objUserDetails.SaveContact(contactSugg.sourceId, contactSugg.contactName, contactSugg.contactNumber, contactSugg.location1, contactSugg.location2, contactSugg.location3, contactSugg.comments, contactSugg.contactLevelUnderstanding, contactSugg.notification, contactSugg.isContactDetailsAdded, contactSugg.platform, contactSugg.allowProvideSuggestion))
                        {
                            ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "AddContact",
                                "AddContact: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString(), false, contactSugg.contactId);
                            custMessage.action = "Success";
                            custMessage.message = "Contact details added Successfully!";
                        }
                        else
                        {
                            ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "AddContact",
                                "AddContact: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString() + "#1 Could not Add, please check data and submit again!", true, contactSugg.contactId);
                            custMessage.action = "Failure";
                            custMessage.message = "Could not Add, please check data and submit again!";
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                        }
                    }
                    else
                    {
                        ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "AddContact",
                            "AddContact: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString() + "#2 Could not Add, please check data and submit again!", true, contactSugg.contactId);
                        custMessage.action = "Failure";
                        custMessage.message = "Could not Add, please check data and submit again!";
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                    }
                }
                else
                {
                    ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "AddContact",
                        "AddContact: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString() + "# Validation failed", true, contactSugg.contactId);
                    custMessage.action = "Failure";
                    custMessage.message = "Validation failed";
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
                // TODO: Add insert logic here

                // return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ActivityLogger.ActivityLog(Convert.ToByte(contactSugg.platform != null ? contactSugg.platform : 0), "", "MyDetails", "AddContact",
                    "AddContact: " + contactSugg.location1 + "#" + contactSugg.location2 + "#" + contactSugg.location3 + "#" + contactSugg.comments + "#" + contactSugg.contactLevelUnderstanding.ToString() + "#" + contactSugg.notification.ToString() + "#" + contactSugg.isContactDetailsAdded.ToString() + "#" + contactSugg.platform.ToString() + "#" + contactSugg.allowProvideSuggestion.ToString() + "#" + ex.Message, true, contactSugg.contactId);
                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "me POST Add Contact");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }


        [Route("api/Locations/{locationId=locationId}/{cityId=cityId}/{areaShortCode=areaShortCode}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetLocationWithSuburb(int? locationId, int? cityId, string areaShortCode)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtLocation = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {
                dtLocation = objUserDetails.GetLocation(locationId, string.Empty, string.Empty, cityId, areaShortCode);
                IList<Location> items = dtLocation.AsEnumerable().Select(row =>
                 new Location
                 {
                     locationId = row.Field<int>("LocationId"),
                     locationName = row.Field<string>("LocationName"),
                     area = row.Field<string>("Area"),
                     city = row.Field<string>("City"),
                     suburb = row.Field<string>("Suburb"),
                     areaShortCode = row.Field<string>("areaShortCode")

                 }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "Failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "Locations Get");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);

        }
        public ContactSuggestions GetContact(int? contactId)
        {
            ContactSuggestions objContact = null;
            if (contactId != null)
            {
                objContact = new ContactSuggestions();
                // ContactSuggestion.Models.Source objSource = (ContactSuggestion.Models.Source)Session["UserDetails"];
                UserDetailsWeb objUserDetails = new UserDetailsWeb();
                DataTable dtContact = new DataTable();
                dtContact = objUserDetails.GetContacts(Convert.ToInt32(contactId), null, string.Empty, "name");
                if (dtContact.Rows.Count > 0)
                {
                    objContact.sourceId = Convert.ToInt32(dtContact.Rows[0]["SourceId"]);
                    objContact.source = Convert.ToString(dtContact.Rows[0]["Name"]);
                    objContact.contactId = Convert.ToInt32(dtContact.Rows[0]["ContactId"]);
                    objContact.contact = Convert.ToString(dtContact.Rows[0]["ContactName"]);
                    objContact.location4 = Convert.ToString(dtContact.Rows[0]["LocationId1"]);
                    objContact.location5 = Convert.ToString(dtContact.Rows[0]["LocationId2"]);
                    objContact.location6 = Convert.ToString(dtContact.Rows[0]["LocationId3"]);
                    objContact.contactNumber = Convert.ToString(dtContact.Rows[0]["ContactNumber"]);
                    objContact.contactComments = Convert.ToString(dtContact.Rows[0]["Comments"]);
                }
            }
            return objContact;
        }
        [Route("api/getcontacts/{contactId=contactId}/{sourceId=sourceId}/{name=name}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetContacts(int? contactId, int? sourceId, string name)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            ContactSuggestions objContact = new ContactSuggestions();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            DataTable dtContact = new DataTable();
            try
            {
                dtContact = objUserDetails.GetContacts(contactId, sourceId, string.Empty, name);

                IList<ContactSuggestions> items = dtContact.AsEnumerable().Select(row =>
                    new ContactSuggestions
                    {
                        sourceId = row.Field<int>("SourceId"),
                        source = row.Field<string>("Name"),
                        contactId = row.Field<int>("ContactId"),
                        contact = row.Field<string>("ContactName"),
                        location4 = row.Field<string>("LocationId1"),
                        location5 = row.Field<string>("LocationId2"),
                        location6 = row.Field<string>("LocationId3"),
                        contactNumber = row.Field<string>("ContactNumber"),
                        contactComments = row.Field<string>("Comments"),
                        contactLevelUnderstanding = row.Field<int?>("ContactLevelUnderstanding"),
                        notification = row.Field<int?>("Notification"),
                        isContactDetailsAdded = row.Field<bool?>("IsContactDetailsAdded")
                    }).ToList();

                var contact = (from con in items
                               select new
                               {
                                   sourceId = con.sourceId,
                                   source = con.source,
                                   contactId = con.contactId,
                                   contact = con.contact,
                                   location1 = con.location4,
                                   location2 = con.location5,
                                   location3 = con.location6,
                                   contactNumber = con.contactNumber,
                                   contactComments = con.contactComments,
                                   contactLevelUnderstanding = con.contactLevelUnderstanding,
                                   notification = con.notification,
                                   isContactDetailsAdded = con.isContactDetailsAdded
                               }).ToList();

                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }

            }
            catch (Exception ex)
            {

                custResponse.action = "Failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "getcontacts Get");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }
        [Route("api/getsources/{sourceId=sourceId}/{name=name}/{isInterns=isInterns}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetSources(int? sourceId, string name, bool? isInterns)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            ContactSuggestions objContact = new ContactSuggestions();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            DataTable dtContact = new DataTable();
            try
            {
                dtContact = objUserDetails.GetSoruce(sourceId, name, isInterns);

                IList<ContactSuggestions> items = dtContact.AsEnumerable().Select(row =>
                    new ContactSuggestions
                    {
                        sourceId = row.Field<int>("SourceId"),
                        source = row.Field<string>("Name"),
                        sourceType = row.Field<int?>("SourceType"),
                        contactNumber = row.Field<string>("Mobile"),
                        sourceImage = row.Field<string>("SourceImage"),
                        sourceTypeText = row.Field<string>("SourceTypeText")

                    }).ToList();

                var contact = (from con in items
                               select new
                               {
                                   sourceId = con.sourceId,
                                   source = con.source,
                                   sourceType = con.sourceType,
                                   contactNumber = con.contactNumber,
                                   sourceImage = con.sourceImage,
                                   sourceTypeText = con.sourceTypeText
                               }).ToList();

                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "Failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "getsources");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }
        [Route("api/getbusiness/{catId=catId}/{SubCatId=SubCatId}/{cityId=cityId}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetBusinessName(int? catId, int? SubCatId, int? cityId)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            ContactSuggestions objContact = new ContactSuggestions();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            DataTable dtContact = new DataTable();
            try
            {
                dtContact = objUserDetails.GetBusinessName(catId, SubCatId, cityId);

                IList<ContactSuggestions> items = dtContact.AsEnumerable().Select(row =>
                    new ContactSuggestions
                    {
                        businessName = row.Field<string>("BusinessName"),
                        citiLevelBusiness = row.Field<bool>("CitiLevelBusiness"),
                        isAChain = row.Field<bool>("IsAChain"),
                        location = row.Field<string>("Location"),
                        businessContact = row.Field<string>("BusinessContact")

                    }).ToList();

                var contact = (from con in items
                               select new
                               {
                                   businessName = con.businessName,
                                   isLocal = con.citiLevelBusiness,
                                   isachain = con.isAChain,
                                   location = con.location,
                                   businessContact = con.businessContact
                               }).ToList();

                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "Failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "getbusiness");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }
        [Route("api/me/{contactId=contactId}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetMyDetails(int? contactId)
        {
            string token = string.Empty;
            if (contactId == null)
            {
                token = Request.Headers.GetValues("Token").First();
            }
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            ContactSuggestions objContact = new ContactSuggestions();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            DataTable dtContact = new DataTable();
            int contcactId = 0;
            int? platform = 0;
            try
            {
                dtContact = objUserDetails.GetContacts(Convert.ToInt32(contactId), null, token, "name");

                IList<ContactSuggestions> items = dtContact.AsEnumerable().Select(row =>
                    new ContactSuggestions
                    {
                        sourceId = row.Field<int>("SourceId"),
                        source = row.Field<string>("Name"),
                        contactId = row.Field<int>("ContactId"),
                        contact = row.Field<string>("ContactName"),
                        location4 = row.Field<string>("LocationId1"),
                        location5 = row.Field<string>("LocationId2"),
                        location6 = row.Field<string>("LocationId3"),
                        contactNumber = row.Field<string>("ContactNumber"),
                        contactComments = row.Field<string>("Comments"),
                        contactLevelUnderstanding = row.Field<int?>("ContactLevelUnderstanding"),
                        notification = row.Field<int?>("Notification"),
                        isContactDetailsAdded = row.Field<bool?>("IsContactDetailsAdded"),
                        platform = row.Field<int?>("Platform"),
                        allowProvideSuggestion = row.Field<bool>("AllowProvideSuggestion")
                    }).ToList();

                var contact = (from con in items
                               select new
                               {
                                   sourceId = con.sourceId,
                                   source = con.source,
                                   contactId = con.contactId,
                                   contact = con.contact,
                                   location1 = con.location4,
                                   location2 = con.location5,
                                   location3 = con.location6,
                                   contactNumber = con.contactNumber,
                                   contactComments = con.contactComments,
                                   contactLevelUnderstanding = con.contactLevelUnderstanding,
                                   notification = con.notification,
                                   isContactDetailsAdded = con.isContactDetailsAdded,
                                   platform = con.platform,
                                   allowProvideSuggestion = con.allowProvideSuggestion
                               }).FirstOrDefault();

                if (items.Count > 0)
                {
                    ActivityLogger.ActivityLog(Convert.ToByte(platform != null ? platform : 0), "", "MyDetails", "ViewContact", "ViewContact", false, contcactId);
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                    contcactId = contact.contactId;
                    platform = contact.platform;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {
                ActivityLogger.ActivityLog(Convert.ToByte(platform != null ? platform : 0), "", "MyDetails", "AddContact", "ViewContact: " + ex.Message, true, contcactId);
                custResponse.action = "Failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "me GET my details");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }

        [Route("api/GetSubCate")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetSubCate(int catId, int? contactId)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            Category objCategory = new Category();
            try
            {
                dtContact = objUserDetails.GetSubCategory(catId, null, contactId, null, null, "areashortCode", "location").Tables[1];
                IList<SubCategory> items = dtContact.AsEnumerable().Select(row =>
                    new SubCategory
                    {
                        subCatId = row.Field<int>("SubCatId"),
                        name = row.Field<string>("Name"),
                        catId = row.Field<int>("CatId")


                    }).ToList();

                var contact = (from con in items
                               select new
                               {
                                   subCatId = con.subCatId,
                                   name = con.name,
                                   catId = con.catId,

                               }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "Failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "GetSubCate");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }

        [Route("api/skipvideo")]
        [HttpPost]
        [AuthorizationRequired]
        public HttpResponseMessage SkipVidio(Source objSoruce)
        {
            string message = string.Empty;
            CustomResponseMessage custMessage = new CustomResponseMessage();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            custMessage.action = "Failure";
            custMessage.message = "some error has occured";
            try
            {
                if (objUserDetails.UpdateSkipVedio(objSoruce.contactId, objSoruce.skipVideo))
                {
                    custMessage.action = "Success";
                    custMessage.message = "Record updated successfully!.";
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
            }
            catch (Exception ex)
            {

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "skipvideo POST");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }


        [Route("api/help/{module=module}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetHelp(string module)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtContact = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            Category objCategory = new Category();
            if (module == "module")
            {
                module = string.Empty;
            }
            try
            {
                dtContact = objUserDetails.GetHelp(module);
                IList<Help> items = dtContact.AsEnumerable().Select(row =>
                    new Help
                    {
                        moduleName = row.Field<string>("ModuleName"),
                        uRLorText = row.Field<string>("URLorText").Split('~')

                    }).ToList();

                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "Failure";
                custResponse.message = ex.Message;
                SendEmail(ex.Message, "help");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }
        public string GetComments(string comm, string source, string contact, string time)
        {
            string strReturnComments = string.Empty;
            try
            {




                if (comm != null)
                {
                    string[] splicontact = contact.Split('|');
                    string[] spliComm = comm.Split('|');
                    string[] splisource = source.Split('|');
                    if (splicontact.Length > 1)
                    {
                        string[] splitime = time != null ? time.Split('|') : null;
                        for (int i = 0; i < spliComm.Length; i++)
                        {
                            if (spliComm.Length - 1 == i)
                            {
                                strReturnComments += (spliComm[i] != null ? spliComm[i] : "") + "~" + (splicontact[i] != null ? splicontact[i] : "") + " (Ref-" + (splisource[i] != null ? splisource[i] : "") + ")";
                            }
                            else
                            {
                                strReturnComments += (spliComm[i] != null ? spliComm[i] : "") + "~" + (splicontact[i] != null ? splicontact[i] : "") + " (Ref-" + (splisource[i] != null ? splisource[i] : "") + ")" + (splitime[i] != null ? splitime[i] : "") + "~";
                            }

                        }
                    }
                    else
                    {
                        strReturnComments += (spliComm[0] != null ? spliComm[0] : "") + "~" + (splicontact[0] != null ? splicontact[0] : "") + " (Ref-" + (splisource[0] != null ? splisource[0] : "") + ")";
                    }

                }
                else
                {
                    string[] splisource = source.Split('|');
                    string[] splicontact = contact.Split('|');
                    if (splicontact.Length > 1)
                    {
                        string[] splitime = time != null ? time.Split('|') : null;
                        for (int i = 0; i < splisource.Length; i++)
                        {
                            if (splisource.Length - 1 == i)
                            {
                                strReturnComments += "~" + (splicontact[i] != null ? splicontact[i] : "") + " (Ref-" + (splisource[i] != null ? splisource[i] : "") + ")";
                            }
                            else
                            {
                                strReturnComments += (splicontact[i] != null ? splicontact[i] : "") + " (Ref-" + (splisource[i] != null ? splisource[i] : "") + ")" + (splitime[i] != null ? splitime[i] : "") + "~"; ;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return strReturnComments;
        }
        #region Device
        [Route("api/registerdevice")]
        [HttpPost]
        [AuthorizationRequired]
        public HttpResponseMessage SaveDevice(Device objDevice)
        {
            int? uid = null;
            string message = string.Empty;
            CustomResponseMessageForDevice custMessage = new CustomResponseMessageForDevice();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            custMessage.action = "Failure";
            custMessage.message = "some error has occured";
            try
            {
                if (objUserDetails.SaveDevice(objDevice.uid, objDevice.contactId, objDevice.token, objDevice.deviceId, objDevice.type, out uid, objDevice.modelAndOS))
                {
                    custMessage.action = "Success";
                    custMessage.message = "Record saved successfully!.";
                    custMessage.uid = uid;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
            }
            catch (Exception ex)
            {

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "registerdevice POST");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }

        [Route("api/registerdevice")]
        [HttpPut]
        [AuthorizationRequired]
        public HttpResponseMessage UpdateDevice(Device objDevice)
        {
            int? uid = null;
            string message = string.Empty;
            CustomResponseMessageForDevice custMessage = new CustomResponseMessageForDevice();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            custMessage.action = "Failure";
            custMessage.message = "some error has occured";
            try
            {
                if (objUserDetails.SaveDevice(objDevice.uid, objDevice.contactId, objDevice.token, objDevice.deviceId, objDevice.type, out uid, objDevice.modelAndOS))
                {
                    custMessage.action = "Success";
                    custMessage.message = "Record updated successfully!.";
                    custMessage.uid = uid;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
            }
            catch (Exception ex)
            {

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "registerdevice PUT");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }


        #endregion

        #region Request Suggestion
        [Route("api/requestsuggestion")]
        [HttpPost]
        [AuthorizationRequired]
        public HttpResponseMessage RequestSuggestionSave(RequestSuggestions objRequestSugg)
        {
            string message = string.Empty;
            CustomResponseMessage custMessage = new CustomResponseMessage();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            custMessage.action = "Failure";
            custMessage.message = "some error has occured";
            try
            {
                objRequestSugg.uID = 0;
                if (objUserDetails.SaveRequestSuggestion(objRequestSugg.uID, objRequestSugg.categoryId, objRequestSugg.subCategoryId, objRequestSugg.microcategoryId, objRequestSugg.location, objRequestSugg.cityId, objRequestSugg.comments, objRequestSugg.contactId, objRequestSugg.platform))
                {
                    DataTable dtDeviceDetails = new DataTable();
                    DataTable dtlocation = new DataTable();
                    DataTable dtDeviceUIDList = new DataTable();
                    dtDeviceDetails = objUserDetails.GetSourcesToken().Tables[0];
                    dtDeviceUIDList = objUserDetails.GetSourcesToken().Tables[1];
                    dtlocation = objUserDetails.GetLocation(null, "", objRequestSugg.location, objRequestSugg.cityId, "areaShortCode");
                    int? locID = Convert.ToInt32(dtlocation.Rows[0]["LocationId"]);
                    PushAndroidNotification(Convert.ToString(dtDeviceDetails.Rows[0]["TokenList"]), Convert.ToInt32(objRequestSugg.categoryId), Convert.ToInt32(objRequestSugg.subCategoryId), Convert.ToInt32(objRequestSugg.microcategoryId), locID, "Suggestion Request", objRequestSugg.comments, Convert.ToString(dtDeviceUIDList.Rows[0]["UIDList"]));
                    custMessage.action = "Success";
                    custMessage.message = "Record added successfully!.";
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
            }
            catch (Exception ex)
            {

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "requestsuggestion POST");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }
        public void PushAndroidNotification(string token, int? CatId, int? SubCategoryId, int? MicrocategoryId, int? LocationId, string title, string text, string uIDList)
        {
            // Authorization: key = AAAAS7HcV0s:APA91bF436VQayZCb - O3blmqqovG - 8ttC78jbyPVUXmgOrvCNRU8A94CWqg20lsamKjxcU2k5iPTnn2oiGJ6_hVWprBhXLD_3NtZZwDz7 - 0utoLGprkzIm06OYR2zn43m4qGkS5V - Jep
            string[] regIDs = token.Split('|');
            string androidApiUrl = "https://fcm.googleapis.com/fcm/send";
            PushNotification objPushnotification = new PushNotification();
            objPushnotification.registration_ids = regIDs;

            Notifications objNotifications = new Notifications();
            objNotifications.title = title;
            objNotifications.body = text; //"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            objNotifications.sound = "default";
            objNotifications.vibrate = "default";
            objNotifications.priority = "high";
            objPushnotification.notification = objNotifications;
            PushData objPushData = new PushData();
            objPushData.title = title;
            objPushData.body = text;// "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            objPushData.date = DateTime.Now.ToString();
            objPushData.catId = CatId;
            objPushData.subCatId = SubCategoryId;
            objPushData.microId = MicrocategoryId;
            objPushData.locationId = LocationId;
            objPushnotification.data = objPushData;
            // CallApi(objPushnotification, androidApiUrl);
            SendPushNotification(objPushnotification, androidApiUrl, uIDList);

        }
        public static void SendPushNotification(Object ApiRequestModel, string api, string uIDList)
        {
            try
            {
                //System.Configuration.ConfigurationManager.AppSettings["RegKey"];
                string applicationID = System.Configuration.ConfigurationManager.AppSettings["RegKey"]; //"AAAAS7HcV0s:APA91bF436VQayZCb-O3blmqqovG-8ttC78jbyPVUXmgOrvCNRU8A94CWqg20lsamKjxcU2k5iPTnn2oiGJ6_hVWprBhXLD_3NtZZwDz7-0utoLGprkzIm06OYR2zn43m4qGkS5V-Jep";
                HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(api);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(ApiRequestModel);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                // tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
                    {

                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {

                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                                if (tResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    UserDetailsWeb objUserDetailsWeb = new UserDetailsWeb();
                                    objUserDetailsWeb.UpdateNotificationTimeSent(uIDList);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
        [Route("api/requestsuggestion")]
        [HttpPut]
        [AuthorizationRequired]
        public HttpResponseMessage RequestSuggestionUpdate(RequestSuggestions objRequestSugg)
        {
            string message = string.Empty;
            CustomResponseMessage custMessage = new CustomResponseMessage();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            custMessage.action = "Failure";
            custMessage.message = "some error has occured";
            try
            {
                if (objRequestSugg.uID > 0)
                {
                    if (objUserDetails.SaveRequestSuggestion(objRequestSugg.uID, objRequestSugg.categoryId, objRequestSugg.subCategoryId, objRequestSugg.microcategoryId, objRequestSugg.location, objRequestSugg.cityId, objRequestSugg.comments, objRequestSugg.contactId, objRequestSugg.platform))
                    {
                        custMessage.action = "Success";
                        custMessage.message = "Record updated successfully!.";
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                    }
                }
                else
                {
                    custMessage.message = "Uid is missing for update the record.";
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }

            }
            catch (Exception ex)
            {

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "requestsuggestion PUT");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }
        #endregion

        #region City
        [Route("api/city")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetCity()
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtLocation = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {

                dtLocation = objUserDetails.GetCity();
                IList<City> items = dtLocation.AsEnumerable().Select(row =>
                 new City
                 {
                     cityId = row.Field<int>("CityId"),
                     cityName = row.Field<string>("CityName"),
                     minGeoLat = row.Field<double>("MinGeoLat"),
                     maxGeoLat = row.Field<double>("MaxGeoLat"),
                     minGeoLong = row.Field<double>("MinGeoLong"),
                     maxGeoLong = row.Field<double>("MaxGeoLong"),
                     cityURL = row.Field<string>("CityURL")
                 }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "city");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);

        }
        #endregion
        #region Notification


        [Route("api/updatenotification")]
        [HttpPut]
        [AuthorizationRequired]
        public HttpResponseMessage UpdateNotification(NotificationDetails objDevice)
        {
            int? uid = null;
            string message = string.Empty;
            CustomResponseMessageForDevice custMessage = new CustomResponseMessageForDevice();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            custMessage.action = "Failure";
            custMessage.message = "some error has occured";
            try
            {
                if (objUserDetails.UpdateNotification(objDevice.contactId, objDevice.notificationID, objDevice.IsDone, objDevice.dismiss))
                {
                    custMessage.action = "Success";
                    custMessage.message = "Record updated successfully!.";
                    custMessage.uid = uid;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
            }
            catch (Exception ex)
            {

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "updatenotification PUT");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }
        public ViewData GetDataForNotification(int? locid, int? catid, int? subcateid, int? mid, string scName, string Cname, string locName, string mcName, string addedBy, string addedwhen, string target, string redirectTo, int? suggestionId, int? redirectToType)
        {
            ViewData objViewData = new ViewData();
            objViewData.LocationId = locid;
            objViewData.CatId = catid;
            objViewData.SubCatId = subcateid;
            objViewData.MCId = mid;
            objViewData.SubCategoryName = !string.IsNullOrEmpty(scName) ? scName : null;
            objViewData.CategoryName = !string.IsNullOrEmpty(Cname) ? Cname : null;
            objViewData.LocationName = !string.IsNullOrEmpty(locName) ? locName : null;
            objViewData.MCName = !string.IsNullOrEmpty(mcName) ? mcName : null;
            objViewData.addedBy = !string.IsNullOrEmpty(addedBy) ? addedBy : null;
            objViewData.addedWhen = !string.IsNullOrEmpty(addedwhen) ? addedwhen : null;
            objViewData.target = !string.IsNullOrEmpty(target) ? target : null;
            objViewData.redirectTo = !string.IsNullOrEmpty(redirectTo) ? redirectTo : null;
            objViewData.redirectToType = redirectToType;
            objViewData.suggestionId = suggestionId;
            return objViewData;
        }
        [Route("api/getnotifications/{contactId=contactId}/{deviceId=deviceId}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetNotifications(int contactId, string deviceId)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtLocation = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {

                dtLocation = objUserDetails.GetNotifications(contactId, deviceId);
                IList<NotificationDetails> items = dtLocation.AsEnumerable().Select(row =>
                 new NotificationDetails
                 {

                     notificationID = row.Field<int?>("UID"),
                     notificationType = !string.IsNullOrEmpty(row.Field<string>("NotificationType")) ? row.Field<string>("NotificationType") : null,
                     notificationTitle = !string.IsNullOrEmpty(row.Field<string>("NotificationTitle")) ? row.Field<string>("NotificationTitle") : null,
                     text = row.Field<string>("Text"),
                     notificationPhoto = !string.IsNullOrEmpty(row.Field<string>("NotificationPhoto")) ? (row.Field<string>("NotificationPhoto").Contains("http://tagaboutit.com") ? row.Field<string>("NotificationPhoto") : null) : null,
                     timeSent = !string.IsNullOrEmpty(row.Field<string>("TimeSent")) ? row.Field<string>("TimeSent") : null,

                     data = GetDataForNotification(row.Field<int?>("LocationId"), row.Field<int?>("CatId"), row.Field<int?>("SubCategoryId"), row.Field<int?>("MicrocategoryId"), row.Field<string>("SubCategoryName"), row.Field<string>("CategoryName"), row.Field<string>("LocationName"), row.Field<string>("MCName"), row.Field<string>("AddedBy"), row.Field<string>("AddedWhen"), row.Field<string>("Target"), row.Field<string>("RedirectTo"), row.Field<int?>("SuggestionId"), row.Field<int?>("RedirectToType"))


                 }).ToList();
                var contact = (from con in items
                               select new
                               {
                                   notificationID = con.notificationID,
                                   notificationType = con.notificationType,
                                   notificationTitle = con.notificationTitle,
                                   description = con.text,
                                   notificationPhoto = con.notificationPhoto,
                                   timeSent = con.timeSent,
                                   data = con.data
                               }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = contact;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = contact;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "getnotifications");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }



            return Request.CreateResponse(HttpStatusCode.OK, custResponse);
        }
        #endregion
        #region Send Email
        public void SendEmail(string error, string apiName)
        {
            try
            {


                using (MailMessage mm = new MailMessage("admin@tagaboutit.com", "gwalvanshi@gmail.com,abhi.saste@gmail.com"))
                // using (MailMessage mm = new MailMessage("emailus @d2digitalservices.com", ToEmail))


                {
                    mm.Subject = "TAG API Error";
                    string body = "Hello Developer,";
                    body += "<br /><br />Please check the following error";
                    body += "<br /> API:" + apiName;
                    body += "<br />" + error;
                    body += "<br /><br />Thanks";
                    mm.Body = body;

                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "us3.smtp.mailhostbox.com";
                    // smtp.Host = "smtpout.secureserver.net";


                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("admin@tagaboutit.com", "TAI_8min");
                    // NetworkCredential NetworkCred = new NetworkCredential("emailus@d2digitalservices.com", "Ddig@87!");

                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.TargetName = "STARTTLS/us3.smtp.mailhostbox.com";
                    // smtp.Port = 565;



                    smtp.Send(mm);
                }
            }
            catch (Exception ex)
            {
                // Erroemsg.Text = ex.Message;
                /// throw  ;
            }
        }
        #endregion
        [Route("api/deletesuggestion")]
        [HttpDelete]
        [AuthorizationRequired]
        public HttpResponseMessage DeleteSuggestion(DeleteSuggestion objContact)
        {
            int? uid = null;
            string message = string.Empty;
            CustomResponseMessageForDevice custMessage = new CustomResponseMessageForDevice();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            custMessage.action = "Failure";
            custMessage.message = "some error has occured";
            try
            {
                if (objUserDetails.DeleteSuggesion(objContact.sugId, objContact.reasonForChange))
                {
                    custMessage.action = "Success";
                    custMessage.message = "Record deleted successfully!.";
                    custMessage.uid = uid;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
                }
            }
            catch (Exception ex)
            {

                custMessage.action = "Failure";
                custMessage.message = ex.Message;
                SendEmail(ex.Message, "DeleteSuggestion Delete");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custMessage);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custMessage);
        }


        [Route("api/bindvsfilterdd/{contactId=contactId}/{suburb=suburb}/{geoCoordinates=geoCoordinates}/{address=address}/{cityId=cityId}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage BindVSFilterDD(int contactId, string suburb, string geoCoordinates, string address, int cityId)
        {
            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataTable dtLocation = new DataTable();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {

                dtLocation = objUserDetails.BindVSFilterDD(contactId, suburb, geoCoordinates, address, cityId);
                IList<FilterOptions> items = dtLocation.AsEnumerable().Select(row =>
                 new FilterOptions
                 {
                     sequienceId = row.Field<int>("sequienceId"),
                     ddValue = row.Field<string>("ddValue"),
                     ddText = row.Field<string>("ddText"),
                     filterType = row.Field<string>("filterType"),
                     isSelected = row.Field<bool>("isSelected"),
                     cityId = row.Field<Int16>("cityID")
                 }).ToList();
                if (items.Count > 0)
                {
                    custResponse.action = "success";
                    custResponse.message = "";
                    custResponse.data = items;
                }
                else
                {
                    custResponse.action = "success";
                    custResponse.message = "No records found!";
                    custResponse.data = items;

                }
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "BindVSFilterDD");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);

        }
        [Route("api/getsectioncategorywithcount/{contactNumber=contactNumber}/{suburb=suburb}/{areaCode=areaCode}/{cityId=cityId}/{uniqueCount=uniqueCount}")]
        [AuthorizationRequired]
        [HttpGet]
        public HttpResponseMessage GetSCWithCount(string contactNumber, string suburb, string areaCode, int? cityId, bool uniqueCount)
        {
            SuggestionCounts objSuggCount = new SuggestionCounts();

            CustomDataResponseMessage custResponse = new CustomDataResponseMessage();
            DataSet dtLocation = new DataSet();
            UserDetailsWeb objUserDetails = new UserDetailsWeb();
            try
            {

                dtLocation = objUserDetails.GetSCWithCount(contactNumber, suburb, areaCode, cityId, uniqueCount);
                IList<CategoryCount> items = dtLocation.Tables[0].AsEnumerable().Select(row =>
                 new CategoryCount
                 {
                     catId = row.Field<int>("CatId"),
                     name = row.Field<string>("Name"),
                     subCatId = row.Field<int?>("SubCatId"),
                     categoryName = row.Field<string>("CName"),
                     suggCount = row.Field<int?>("SugCnt"),
                     microCategoryToolTip = row.Field<string>("microCategoryToolTip"),
                     commentsToolTip = row.Field<string>("commentsToolTip"),
                     isLocal = row.Field<bool>("isLocal")

                 }).ToList();
                objSuggCount.CategoryCountData = items;

                if (contactNumber != "contactNumber")
                {
                    IList<SectionCount> sectionItem = dtLocation.Tables[1].AsEnumerable().Select(row =>
                   new SectionCount
                   {
                       catId = row.Field<int>("CatId"),
                       name = row.Field<string>("Name"),
                       suggCount = row.Field<int?>("SugCnt")
                   }).ToList();
                    objSuggCount.SectionCountData = sectionItem;
                }

                custResponse.action = "success";
                custResponse.message = "";
                custResponse.data = objSuggCount;
            }
            catch (Exception ex)
            {

                custResponse.action = "failure";
                custResponse.message = ex.Message;
                custResponse.data = null;
                SendEmail(ex.Message, "BindVSFilterDD");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, custResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, custResponse);

        }

    }
}