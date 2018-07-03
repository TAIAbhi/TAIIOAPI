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
using StubAPI.Models;
using System.Globalization;

namespace StubAPI.BAL
{
    public class TokenServices
    {


        #region Private member variables.
        //private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Public constructor.
        /// <summary>
        /// Public constructor.
        /// </summary>
        public TokenServices()//IUnitOfWork unitOfWork)
        {
            // _unitOfWork = unitOfWork;
        }
        #endregion


        #region Public member methods.

        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Token GenerateToken(int userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;
            DateTime expiredOn = DateTime.Now.AddSeconds(
            Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            var tokendomain = new Token
            {
                userId = userId,
                authToken = token,
               // issuedOn = issuedOn,
               // expiresOn = expiredOn
            };

            //bool result = false;
            int noOfEffectedRows = 0;
            int outParamSave = 0;
            try
            {
                string spName = "spSaveToken";
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@UserId", userId);
                parameters[1] = new SqlParameter("@AuthToken", token);
                parameters[2] = new SqlParameter("@IssuedOn", issuedOn);
                parameters[3] = new SqlParameter("@ExpiresOn", expiredOn);
                parameters[4] = new SqlParameter("@IsSaved", outParamSave);
                parameters[4].Direction = ParameterDirection.Output;


                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
                outParamSave = parameters[4].Value == null ? 0 : Convert.ToInt32(parameters[4].Value);
                if (outParamSave > 0)
                {
                    return tokendomain;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tokendomain;
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public bool ValidateToken(string tokenId)
        {
            //[spGetToken]
            DataTable dtuserDtails = new DataTable();

            try
            {
                string spName = "spGetToken";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@AuthToken", tokenId);
                string oDate = Convert.ToDateTime(DateTime.Now, CultureInfo.CurrentCulture).ToString("MM/dd/yyyy hh:MM:ss");
                parameters[1] = new SqlParameter("@todayDate", oDate);
                //parameters[2] = new SqlParameter("@ContactId", contactId);
                if (SqlHelper.ExecuteDataset(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters).Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else {
                    return false;
                }


            }
            catch (Exception ex)
            {
                throw ex;
               
            }
            return false;

        }

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenId">true for successful delete</param>
        public bool Kill(string tokenId)
        {
            //_unitOfWork.TokenRepository.Delete(x => x.AuthToken == tokenId);
            //_unitOfWork.Save();
            //var isNotDeleted = _unitOfWork.TokenRepository.GetMany(x => x.AuthToken == tokenId).Any();
            //if (isNotDeleted) { return false; }
            return true;
        }

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>true for successful delete</returns>
        public bool DeleteByUserId(int userId)
        {
            //_unitOfWork.TokenRepository.Delete(x => x.UserId == userId);
            //_unitOfWork.Save();

            //var isNotDeleted = _unitOfWork.TokenRepository.GetMany(x => x.UserId == userId).Any();
            //return !isNotDeleted;
            return true;
        }

        #endregion
    }
}