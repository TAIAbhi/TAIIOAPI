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
    public class UserDetails
    {
        public UserDetails()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        public DataTable Login(string user, string password)
        {
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spLogin";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@UserId", user);
                parameters[1] = new SqlParameter("@Password", password);

                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters).Tables.Count > 0)
                {
                    dtuserDtails = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters).Tables[0];
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtuserDtails;
        }

        public bool SaveUsers(string mobile, string email, string pwd,string firstName, string lastName, string source)
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
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters);
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
                parameters[2] = new SqlParameter("@ErrorMessage",SqlDbType.NVarChar,100, result);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters);
                result = parameters[2].Value.ToString();               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdatePassword(string userId, string password)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spUpdatePassword";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@UserId", userId);
                parameters[1] = new SqlParameter("@Password", password);             
                parameters[2] = new SqlParameter("@IsSaved", outParamSave);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters);
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
        public bool SaveUserDetails(string userXmlData,string source, int userId)
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
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[3].Value==null?0:Convert.ToInt32(parameters[3].Value);
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

        public string IsUserBusinessExists(int userId, int businessType)
        {
            string result = string.Empty;
            int noOfEffectedRows = 0;
            try
            {
                string spName = "spCheck_DuplicateUserBusinessDetails";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@UserId", userId);
                parameters[1] = new SqlParameter("@BusinessType", businessType);
                parameters[2] = new SqlParameter("@ErrorMessage", result);
                parameters[2].Direction = ParameterDirection.Output;
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters);
                result = parameters[2].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


       public bool SaveFOFUserDetails(int userID, bool FoF, bool mycontact, bool tag, DateTime blockdate)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
              

                string spName = "spUser_UpdateFOFDetails";
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@UserID", userID);
                parameters[1] = new SqlParameter("@FOF", FoF);
                parameters[2] = new SqlParameter("@MyContacts", mycontact);
                parameters[3] = new SqlParameter("@BlockNonRecoTill", blockdate);
                parameters[4] = new SqlParameter("@NotifyTags", tag);            

                parameters[5] = new SqlParameter("@IsSaved", outParamSave);
                parameters[5].Direction = ParameterDirection.Output;


                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[5].Value == null ? 0 : Convert.ToInt32(parameters[5].Value);
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
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters);
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
    }
}