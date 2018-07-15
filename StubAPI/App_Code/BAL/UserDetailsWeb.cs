using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using DAL;
namespace StubAPI.BAL
{
    public class UserDetailsWeb
    {
        public UserDetailsWeb()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataTable GetMySuggestionCategoryWise(string contactNumber, string token)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetMySuggestionCategoryWise";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ContactNumber", contactNumber);
                if (!string.IsNullOrEmpty(token))
                {
                    parameters[1] = new SqlParameter("@Token", token);
                }
                else
                {
                    parameters[1] = new SqlParameter("@Token", DBNull.Value);
                }
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetMySuggestionSubCategoryWise(int catId, string contactNumber, string token)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetMySuggestionSubCategoryWise";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@ContactNumber", contactNumber);
                if (!string.IsNullOrEmpty(token))
                {
                    parameters[2] = new SqlParameter("@Token", token);
                }
                else
                {
                    parameters[2] = new SqlParameter("@Token", DBNull.Value);
                }
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetHelp(string modulename)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetHelp";
                SqlParameter[] parameters = new SqlParameter[1];
                if (!string.IsNullOrEmpty(modulename))
                {
                    parameters[0] = new SqlParameter("@ModuleName", modulename);
                }
                else
                {
                    parameters[0] = new SqlParameter("@ModuleName", DBNull.Value);
                }
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetBusinessName(int? catId, int? subCat, int? cityId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetBusinessName";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@Category", catId);
                parameters[1] = new SqlParameter("@SubCategory", subCat);
                parameters[2] = new SqlParameter("@CityId", cityId);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public string MobileFormat(string str)
        {
            str = str.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "");
            str = 10 < str.Length ? str.Substring(str.Length - 10) : str;
            return str;
        }

        public bool SaveUsers(string mobile, string email, string pwd, string firstName, string lastName, string source)
        {

            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spSaveUserDetails";
                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@Mobile", mobile);
                parameters[1] = new SqlParameter("@Email", email);
                parameters[2] = new SqlParameter("@Password", pwd);
                parameters[3] = new SqlParameter("@FirstName", firstName);
                parameters[4] = new SqlParameter("@LastName", lastName);
                parameters[5] = new SqlParameter("@Source", source);
                parameters[6] = new SqlParameter("@IsSaved", outParamSave);
                parameters[6].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[6].Value == null ? 0 : Convert.ToInt32(parameters[6].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public string IsUserExists(string mobile, string emailIdAccess)
        {
            string result = string.Empty;
            int noOfEffectedRows = 0;
            try
            {
                string spName = "spCheck_DuplicateUserDetails";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@Mobile", mobile);
                parameters[1] = new SqlParameter("@Email", emailIdAccess);
                parameters[2] = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 100, result);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                result = parameters[2].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveUserDetails(string userXmlData, string source, int userId)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spUser_UserDetailsSave";
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@ImportData", userXmlData);
                parameters[1] = new SqlParameter("@ExistingUserID", userId);
                parameters[2] = new SqlParameter("@Source", source);
                parameters[3] = new SqlParameter("@IsSaved", outParamSave);
                parameters[3].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[3].Value == null ? 0 : Convert.ToInt32(parameters[3].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DataTable GetSourceDetails(string mobile, string password, out int role)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetSourceDetails";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@Mobile", mobile);
                parameters[1] = new SqlParameter("@Password", password);
                parameters[2] = new SqlParameter("@IsAdmin", 0);
                parameters[2].Direction = ParameterDirection.Output;
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }
                role = Convert.ToInt32(parameters[2].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataSet GetCategory(int? catid, int? contactId, bool? isRequest)
        {
            DataSet dtuserDtails = new DataSet();

            try
            {
                string spName = "spGetCategory";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CatId", catid);
                parameters[1] = new SqlParameter("@ContactId", contactId);
                parameters[2] = new SqlParameter("@IsRequest", isRequest == null ? false : isRequest);

                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataTable GetMicroCategory(int? Subcatid, int? MicroId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetMicroCategory";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@SubCateId", Subcatid);
                parameters[1] = new SqlParameter("@MicroId", MicroId);

                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }


        public DataTable GetCity()
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetCity";

                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName).Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetCategoryCount(int? sourceId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetCategoryCount";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@SourceId", sourceId);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataSet GetRequestSuggestion(int? catId, int? subCatId, int? contactId, string location, int? microcate, int? pageSize, int? pageNumber, out int returnTotalRows, int? cityId)
        {


            DataSet dtuserDtails = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[15];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@SubCatId", subCatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@SugId", DBNull.Value);




                parameters[4] = new SqlParameter("@Token", DBNull.Value);

                parameters[5] = new SqlParameter("@SourceId", DBNull.Value);

                parameters[6] = new SqlParameter("@BusinessName", DBNull.Value);

                if (location == "location")
                {
                    parameters[7] = new SqlParameter("@Location", DBNull.Value);
                }
                else
                {

                    parameters[7] = new SqlParameter("@Location", location);
                }
                parameters[8] = new SqlParameter("@IsLocal", DBNull.Value);
                parameters[9] = new SqlParameter("@Microcategory", microcate);

                parameters[10] = new SqlParameter("@ReturnTotalRows", 0);
                parameters[10].Direction = ParameterDirection.Output;

                parameters[11] = new SqlParameter("@PageSize", pageSize);
                parameters[12] = new SqlParameter("@PageNumber", pageNumber);

                // parameters[13] = new SqlParameter("@MCName", microName);

                parameters[13] = new SqlParameter("@MCName", DBNull.Value);

                parameters[14] = new SqlParameter("@CityId", cityId);
                string spName = "spGetRequestSuggestion";

                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                returnTotalRows = parameters[10].Value == null ? 0 : Convert.ToInt32(parameters[10].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataTable GetContacts(int? contactId, int? sourceId, string token, string name)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetContacts";
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@ContactId", contactId);
                parameters[1] = new SqlParameter("@SourceId", sourceId);
                if (!string.IsNullOrEmpty(token))
                {
                    parameters[2] = new SqlParameter("@Token", token);
                }
                else
                {
                    parameters[2] = new SqlParameter("@Token", DBNull.Value);
                }
                if (name == "name")
                {
                    parameters[3] = new SqlParameter("@Name", DBNull.Value);

                }
                else
                {
                    parameters[3] = new SqlParameter("@Name", name);
                }
                parameters[4] = new SqlParameter("@ContactNumber", DBNull.Value);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        //public DataSet GetSuggestion(int ? catId, int? subCatId,  int ? sugId, int? contactId, string token, int ? sourceId)
        //{
        //    DataSet dtuserDtails = new DataSet();

        //    try
        //    {

        //        SqlParameter[] parameters = new SqlParameter[6];
        //        parameters[0] = new SqlParameter("@CatId", catId);
        //        parameters[1] = new SqlParameter("@SubCatId", subCatId);
        //        parameters[2] = new SqlParameter("@ContactId", contactId);
        //        parameters[3] = new SqlParameter("@SugId", sugId);

        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            parameters[4] = new SqlParameter("@Token", token);
        //        }
        //        else
        //        {
        //            parameters[4] = new SqlParameter("@Token", DBNull.Value);
        //        }
        //        parameters[5] = new SqlParameter("@SourceId", sourceId);
        //        string spName = "spGetSuggestion";

        //        dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return dtuserDtails;
        //}
        public DataSet GetSuggestionWithCount(int? catId, int? subCatId, int? sugId, int? contactId, string token, int? sourceId, string businessName, bool? isLocal, string location, int? microcate, int pageSize, int pageNumber, out int returnTotalRows, string microName, int? cityId, string areaShortCode)
        {


            DataSet dtuserDtails = new DataSet();

            try
            {

                SqlParameter[] parameters = new SqlParameter[16];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@SubCatId", subCatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@SugId", sugId);



                if (!string.IsNullOrEmpty(token))
                {
                    parameters[4] = new SqlParameter("@Token", token);
                }
                else
                {
                    parameters[4] = new SqlParameter("@Token", DBNull.Value);
                }
                parameters[5] = new SqlParameter("@SourceId", sourceId);

                if (businessName == "businessName")
                {
                    parameters[6] = new SqlParameter("@BusinessName", DBNull.Value);
                }
                else
                {
                    parameters[6] = new SqlParameter("@BusinessName", businessName);
                }
                if (location == "location")
                {
                    parameters[7] = new SqlParameter("@Location", DBNull.Value);
                }
                else
                {

                    parameters[7] = new SqlParameter("@Location", location);
                }
                parameters[8] = new SqlParameter("@IsLocal", isLocal);
                parameters[9] = new SqlParameter("@Microcategory", microcate);

                parameters[10] = new SqlParameter("@ReturnTotalRows", 0);
                parameters[10].Direction = ParameterDirection.Output;

                parameters[11] = new SqlParameter("@PageSize", pageSize);
                parameters[12] = new SqlParameter("@PageNumber", pageNumber);

                // parameters[13] = new SqlParameter("@MCName", microName);

                if (microName == "microName")
                {
                    parameters[13] = new SqlParameter("@MCName", DBNull.Value);
                }
                else
                {

                    parameters[13] = new SqlParameter("@MCName", microName);
                }
                parameters[14] = new SqlParameter("@CityId", cityId);


                if (areaShortCode == "areaShortCode")
                {
                    parameters[15] = new SqlParameter("@AreaShortCode", DBNull.Value);
                }
                else
                {

                    parameters[15] = new SqlParameter("@AreaShortCode", areaShortCode);
                }
                string spName = "spGetSuggestion_backup";

                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                returnTotalRows = parameters[10].Value == null ? 0 : Convert.ToInt32(parameters[10].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataSet GetSuggestion(int? catId, int? subCatId, int? sugId, int? contactId, string token, int? sourceId, string businessName, bool? isLocal, string location, int? microcate, int pageSize, int pageNumber, out int returnTotalRows, string microName, int? cityId)
        {


            DataSet dtuserDtails = new DataSet();

            try
            {

                SqlParameter[] parameters = new SqlParameter[15];
                parameters[0] = new SqlParameter("@CatId", catId);
                parameters[1] = new SqlParameter("@SubCatId", subCatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@SugId", sugId);



                if (!string.IsNullOrEmpty(token))
                {
                    parameters[4] = new SqlParameter("@Token", token);
                }
                else
                {
                    parameters[4] = new SqlParameter("@Token", DBNull.Value);
                }
                parameters[5] = new SqlParameter("@SourceId", sourceId);

                if (businessName == "businessName")
                {
                    parameters[6] = new SqlParameter("@BusinessName", DBNull.Value);
                }
                else
                {
                    parameters[6] = new SqlParameter("@BusinessName", businessName);
                }
                if (location == "location")
                {
                    parameters[7] = new SqlParameter("@Location", DBNull.Value);
                }
                else
                {

                    parameters[7] = new SqlParameter("@Location", location);
                }
                parameters[8] = new SqlParameter("@IsLocal", isLocal);
                parameters[9] = new SqlParameter("@Microcategory", microcate);

                parameters[10] = new SqlParameter("@ReturnTotalRows", 0);
                parameters[10].Direction = ParameterDirection.Output;

                parameters[11] = new SqlParameter("@PageSize", pageSize);
                parameters[12] = new SqlParameter("@PageNumber", pageNumber);

                // parameters[13] = new SqlParameter("@MCName", microName);

                if (microName == "microName")
                {
                    parameters[13] = new SqlParameter("@MCName", DBNull.Value);
                }
                else
                {

                    parameters[13] = new SqlParameter("@MCName", microName);
                }
                parameters[14] = new SqlParameter("@CityId", cityId);
                string spName = "spGetSuggestion";

                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                returnTotalRows = parameters[10].Value == null ? 0 : Convert.ToInt32(parameters[10].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetLocation(int? locationid, string suburb, string locationName, int? cityId, string areaShortCode)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetLocation";
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@LocationId", locationid);
                if (suburb == string.Empty)
                {
                    parameters[1] = new SqlParameter("@Suburb", DBNull.Value);
                }
                else
                {
                    parameters[1] = new SqlParameter("@Suburb", suburb);
                }
                if (locationName == string.Empty)
                {
                    parameters[2] = new SqlParameter("@Location", DBNull.Value);
                }
                else
                {
                    parameters[2] = new SqlParameter("@Location", locationName);
                }
                parameters[3] = new SqlParameter("@CityId", cityId);


                if (areaShortCode == "areaShortCode")
                {
                    parameters[4] = new SqlParameter("@AreaShortCode", DBNull.Value);
                }
                else
                {
                    parameters[4] = new SqlParameter("@AreaShortCode", areaShortCode);
                }
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetSoruce(int? sourceID, string name)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetSoruce";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@SourceId", sourceID);
                parameters[1] = new SqlParameter("@IsInters", DBNull.Value);
                if (name == "name")
                {
                    parameters[2] = new SqlParameter("@Name", DBNull.Value);

                }
                else
                {
                    parameters[2] = new SqlParameter("@Name", name);
                }

                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataSet GetSubCategory(int? catid, int? subcatId, int? contactId, bool? isRequest)
        {
            DataSet dtuserDtails = new DataSet();


            try
            {
                string spName = "spGetSubCategory";
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@CatId", catid);
                parameters[1] = new SqlParameter("@SubCatId", subcatId);
                parameters[2] = new SqlParameter("@ContactId", contactId);
                parameters[3] = new SqlParameter("@IsRequest", isRequest == null ? false : isRequest);

                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public bool UpdateMacID(int contactid, string macId)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spUpdateMacID";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                parameters[1] = new SqlParameter("@MacID", macId);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateContact(int contactid, string loc1, string loc2, string loc3, string comments, int? understanding, int? notification, bool? isContactDetailsAdded, int? platform, bool allowProvideSuggestion)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spUpdateContact";
                SqlParameter[] parameters = new SqlParameter[11];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                parameters[1] = new SqlParameter("@LocationId1", loc1);
                parameters[2] = new SqlParameter("@LocationId2", loc2);
                parameters[3] = new SqlParameter("@LocationId3", loc3);
                parameters[4] = new SqlParameter("@Comments", comments);
                parameters[5] = new SqlParameter("@ContactLevelUnderstanding", understanding);
                parameters[6] = new SqlParameter("@Notification", notification);
                parameters[7] = new SqlParameter("@IsSaved", outParamSave);
                parameters[7].Direction = ParameterDirection.Output;
                parameters[8] = new SqlParameter("@IsContactDetailsAdded", isContactDetailsAdded);
                parameters[9] = new SqlParameter("@Platform", platform);
                parameters[10] = new SqlParameter("@AllowProvideSuggestion", allowProvideSuggestion);

                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[7].Value == null ? 0 : Convert.ToInt32(parameters[7].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateSkipVedio(int contactid, bool isSkip)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spUpdateSkipVideo";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                parameters[1] = new SqlParameter("@IsSkipVideo", isSkip);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateEveryVideoCheck(int contactid, bool isSkip)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spUpdateEveryVideoCheck";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                parameters[1] = new SqlParameter("@EveryVideoCheck", isSkip);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateLoginCount(int contactid, string macID)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spUpdateLoginCount";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ContactId", contactid);
                if (macID == string.Empty)
                {
                    parameters[1] = new SqlParameter("@MacID", DBNull.Value);
                }
                else
                {
                    parameters[1] = new SqlParameter("@MacID", macID);
                }
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveContact(int sourceid, string name, string number, string loc1, string loc2, string loc3, string comments, int? understanding, int? notification, bool? isContactDetailsAdded, int? platform, bool allowProvideSuggestion)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spSaveContact";
                SqlParameter[] parameters = new SqlParameter[13];
                parameters[0] = new SqlParameter("@SourceId", sourceid);
                parameters[1] = new SqlParameter("@ContactName", name);
                parameters[2] = new SqlParameter("@ContactNumber", number);
                parameters[3] = new SqlParameter("@LocationId1", loc1);
                parameters[4] = new SqlParameter("@LocationId2", loc2);

                parameters[5] = new SqlParameter("@LocationId3", loc3);
                parameters[6] = new SqlParameter("@Comments", comments);

                parameters[7] = new SqlParameter("@ContactLevelUnderstanding", understanding);
                parameters[8] = new SqlParameter("@Notification", notification);


                parameters[9] = new SqlParameter("@IsSaved", outParamSave);
                parameters[9].Direction = ParameterDirection.Output;

                parameters[10] = new SqlParameter("@IsContactDetailsAdded", isContactDetailsAdded);

                parameters[11] = new SqlParameter("@Platform", platform);
                parameters[12] = new SqlParameter("@AllowProvideSuggestion", allowProvideSuggestion);

                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[9].Value == null ? 0 : Convert.ToInt32(parameters[9].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveContactSuggestions(int sourceid, int contactid, string category, string subcate, string microcat, string bussiName, bool iscitilevel, string busCont, string loc1, string loc2, string loc3, string comments, string importdata, string locationId4, string locationId5, string @locationId6, string contactComments, bool isAChain, string platForm, int? city, int? requestID, bool usedTagSuggetion)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {


                string spName = "spSaveContactSuggestions";
                SqlParameter[] parameters = new SqlParameter[23];
                parameters[0] = new SqlParameter("@SourceId", sourceid);
                parameters[1] = new SqlParameter("@ContactId", contactid);
                parameters[2] = new SqlParameter("@Category", category);
                parameters[3] = new SqlParameter("@SubCategory", subcate);
                parameters[4] = new SqlParameter("@Microcategory", microcat);
                parameters[5] = new SqlParameter("@CitiLevelBusiness", iscitilevel);
                parameters[6] = new SqlParameter("@BusinessName", bussiName);
                parameters[7] = new SqlParameter("@BusinessContact", busCont);
                parameters[8] = new SqlParameter("@LocationId1", loc1);
                parameters[9] = new SqlParameter("@LocationId2", loc2);
                parameters[10] = new SqlParameter("@LocationId3", loc3);
                parameters[11] = new SqlParameter("@Comments", comments);
                parameters[12] = new SqlParameter("@ImportData", importdata);
                parameters[13] = new SqlParameter("@LocationId4", locationId4);
                parameters[14] = new SqlParameter("@LocationId5", locationId5);
                parameters[15] = new SqlParameter("@LocationId6", locationId6);
                parameters[16] = new SqlParameter("@ContactComments", contactComments);
                parameters[17] = new SqlParameter("@IsAChain", isAChain);
                parameters[18] = new SqlParameter("@IsSaved", outParamSave);
                parameters[18].Direction = ParameterDirection.Output;
                parameters[19] = new SqlParameter("@Platform", platForm);
                parameters[20] = new SqlParameter("@City", city);
                parameters[21] = new SqlParameter("@RequestID", requestID);
                parameters[22] = new SqlParameter("@UsedTagSuggetion", usedTagSuggetion);


                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[18].Value == null ? 0 : Convert.ToInt32(parameters[18].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public bool SaveUserBusinessDetails(string userXmlData, int userBusinessId)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spUserBusiness_UserBusinessSave";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ImportData", userXmlData);
                parameters[1] = new SqlParameter("@ExistingBUID", userBusinessId);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateContactSuggestions(int suggId, int sourceid, int contactid, string category, string subcate, string microcat, string bussiName, bool iscitilevel, string busCont, string loc1, string loc2, string loc3, string comments, string importdata, string locationId4, string locationId5, string @locationId6, string contactComments, bool isAChain, string platForm, int? city)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {



                string spName = "spUpdateContactSuggestions";
                SqlParameter[] parameters = new SqlParameter[22];
                parameters[0] = new SqlParameter("@SourceId", sourceid);
                parameters[1] = new SqlParameter("@ContactId", contactid);
                parameters[2] = new SqlParameter("@Category", category);
                parameters[3] = new SqlParameter("@SubCategory", subcate);
                parameters[4] = new SqlParameter("@Microcategory", microcat);
                parameters[5] = new SqlParameter("@CitiLevelBusiness", iscitilevel);
                parameters[6] = new SqlParameter("@BusinessName", bussiName);
                parameters[7] = new SqlParameter("@BusinessContact", busCont);
                parameters[8] = new SqlParameter("@LocationId1", loc1);
                parameters[9] = new SqlParameter("@LocationId2", loc2);
                parameters[10] = new SqlParameter("@LocationId3", loc3);
                parameters[11] = new SqlParameter("@Comments", comments);
                parameters[12] = new SqlParameter("@ImportData", importdata);
                parameters[13] = new SqlParameter("@LocationId4", locationId4);
                parameters[14] = new SqlParameter("@LocationId5", locationId5);
                parameters[15] = new SqlParameter("@LocationId6", locationId6);
                parameters[16] = new SqlParameter("@ContactComments", contactComments);
                parameters[17] = new SqlParameter("@IsAChain", isAChain);
                parameters[18] = new SqlParameter("@IsSaved", outParamSave);
                parameters[18].Direction = ParameterDirection.Output;
                parameters[19] = new SqlParameter("@SuggId", suggId);
                parameters[20] = new SqlParameter("@Platform", platForm);
                parameters[21] = new SqlParameter("@City", city);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[18].Value == null ? 0 : Convert.ToInt32(parameters[18].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public bool SaveLocation(string suburb, string location, string city)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spSaveLoction";
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@Suburb", suburb);
                parameters[1] = new SqlParameter("@LocationName", location);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                parameters[3] = new SqlParameter("@City", city);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public DataTable GetSuburb(int? cityId, string areaShortCode)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spLoctionSuburb";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CityId", cityId);
                if (areaShortCode == "areaShortCode")
                {
                    parameters[1] = new SqlParameter("@AreaShortCode", DBNull.Value);
                }
                else
                {
                    parameters[1] = new SqlParameter("@AreaShortCode", areaShortCode);
                }

                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        #region Token
        public bool SaveToken(int UserId, string AuthToken, DateTime IssuedOn, DateTime ExpiresOn)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {



                string spName = "spSaveToken";
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@UserId", UserId);
                parameters[1] = new SqlParameter("@AuthToken", AuthToken);
                parameters[2] = new SqlParameter("@IssuedOn", IssuedOn);
                parameters[3] = new SqlParameter("@ExpiresOn", ExpiresOn);
                parameters[4] = new SqlParameter("@IsSaved", outParamSave);
                parameters[4].Direction = ParameterDirection.Output;


                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[4].Value == null ? 0 : Convert.ToInt32(parameters[4].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        #endregion

        public bool SaveDevice(int uid, int contactID, string token, string deviceID, string type, out int? returnuid)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spSaveDevice";
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@ContactID", contactID);
                parameters[1] = new SqlParameter("@Token", token);
                parameters[2] = new SqlParameter("@DeviceID", deviceID);
                parameters[3] = new SqlParameter("@Type", type);
                parameters[4] = new SqlParameter("@IsSaved", outParamSave);
                parameters[4].Direction = ParameterDirection.Output;
                parameters[5] = new SqlParameter("@UID", uid);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[4].Value == null ? 0 : Convert.ToInt32(parameters[4].Value);
                returnuid = outParamSave;
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool SaveRequestSuggestion(int uid, int catid, int subcatid, int microcatid, string location, int cityid, string comments, int contactid, int platform)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spSaveRequestSuggestion";
                SqlParameter[] parameters = new SqlParameter[10];
                parameters[0] = new SqlParameter("@UID", uid);
                parameters[1] = new SqlParameter("@CategoryId", catid);
                parameters[3] = new SqlParameter("@SubCategoryId", subcatid);
                parameters[4] = new SqlParameter("@MicrocategoryId", microcatid);
                parameters[5] = new SqlParameter("@Location", location);
                parameters[6] = new SqlParameter("@CityId", cityid);
                parameters[7] = new SqlParameter("@Comments", comments);
                parameters[8] = new SqlParameter("@ContactId", contactid);
                parameters[9] = new SqlParameter("@Platform", platform);
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[2].Value == null ? 0 : Convert.ToInt32(parameters[2].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateNotification(int ContactId, int? NotificationID, bool? Done, bool? Dismiss)
        {
            bool result = true;

            try
            {

                string spName = "spUpdateNotification";
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@ContactId", ContactId);
                parameters[1] = new SqlParameter("@NotificationID", NotificationID);
                parameters[2] = new SqlParameter("@Done", Done);
                parameters[3] = new SqlParameter("@Dismiss", Dismiss);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);

            }
            catch (Exception ex)
            {
                result = false;
                // throw ex;
            }
            return result;
        }
        public bool DeleteSuggesion(int uid, string reasonForChange)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {

                string spName = "spDeleteContactSuggestions";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@SuggId", uid);
                parameters[1] = new SqlParameter("@IsSaved", outParamSave);
                parameters[1].Direction = ParameterDirection.Output;
                parameters[2] = new SqlParameter("@ReasonForChange", reasonForChange);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[1].Value == null ? 0 : Convert.ToInt32(parameters[1].Value);
                if (outParamSave > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public DataTable GetNotifications(int ContactId, string DeviceId)
        {
            DataTable dtuserDtails = new DataTable();
            try
            {
                string spName = "spGetNotifications";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ContactId", ContactId);
                parameters[1] = new SqlParameter("@DeviceId", DeviceId);
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataTable GetDeviceDetails(int contactId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetDeviceDetails";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ContactId", contactId);
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }
        public DataSet GetSourcesToken()
        {
            DataSet dtuserDtails = new DataSet();

            try
            {
                string spName = "spGetSourcesToken";
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public bool UpdateNotificationTimeSent(string deviceUIDList)
        {
            bool result = true;

            try
            {

                string spName = "spUpdateNotificationTimeSent";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@DeviceUID", deviceUIDList);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
            }
            catch (Exception ex)
            {
                result = false;
                // throw ex;
            }
            return result;
        }
        public DataTable BindVSFilterDD(int contactId, string suburb, string geoCoordinates, string address, int cityId)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {

                string spName = "spGetViewScreenFilterDropDownDetails";
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@contactId", contactId);


                if (suburb == "suburb")
                {
                    parameters[1] = new SqlParameter("@suburb", DBNull.Value);
                }
                else
                {
                    parameters[1] = new SqlParameter("@suburb", suburb);
                }
                if (geoCoordinates == "geoCoordinates")
                {
                    parameters[2] = new SqlParameter("@geoCoordinates", DBNull.Value);
                }
                else
                {
                    parameters[2] = new SqlParameter("@geoCoordinates", geoCoordinates);
                }
                if (address == "address")
                {
                    parameters[3] = new SqlParameter("@address", DBNull.Value);
                }
                else
                {
                    parameters[3] = new SqlParameter("@address", address);
                }
                parameters[4] = new SqlParameter("@cityId", cityId);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public DataSet GetSCWithCount(string contactNumber, string suburb, string areaCode, int? cityId, bool uniqueCount)
        {
            DataSet dtuserDtails = new DataSet();

            try
            {
                string spName = "spGetSCWithCount";
                SqlParameter[] parameters = new SqlParameter[5];
                if (contactNumber == "contactNumber")
                    parameters[0] = new SqlParameter("@ContactNumber", DBNull.Value);
                else
                    parameters[0] = new SqlParameter("@ContactNumber", contactNumber);
                if (suburb == "suburb")
                    parameters[1] = new SqlParameter("@Suburb", DBNull.Value);
                else
                    parameters[1] = new SqlParameter("@Suburb", suburb);
                if (areaCode == "areaCode")
                    parameters[2] = new SqlParameter("@AreaCode", DBNull.Value);
                else
                    parameters[2] = new SqlParameter("@AreaCode", areaCode);

                parameters[3] = new SqlParameter("@CityId", cityId);
                parameters[4] = new SqlParameter("@UniqueCount", uniqueCount);
                dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

    }
}