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
    public  static class ActivityLogger
    {
        public static bool ActivityLog(byte ? platform, string ipAdd, string module, string act,string actdetails,bool isError, int contactId )
        {
            bool result = false;
            int noOfEffectedRows = 0;
            try
            {
                string spName = "spActivityLog";
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@Platform", platform);
                parameters[1] = new SqlParameter("@IPAddress", ipAdd);
                parameters[2] = new SqlParameter("@Module", module);
                parameters[3] = new SqlParameter("@Action", act);
                parameters[4] = new SqlParameter("@Action_Details", actdetails);
                parameters[5] = new SqlParameter("@IsError", isError);
                parameters[6] = new SqlParameter("@ContactId", contactId);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString("Isweb"), CommandType.StoredProcedure, spName, parameters);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}