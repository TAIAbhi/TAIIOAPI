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
        public static bool ActivityLog(string location, string action)
        {
            bool result = false;
            int noOfEffectedRows = 0;
            try
            {
                string spName = "spActivityLog_ActivityLogSave";
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@Location", location);
                parameters[1] = new SqlParameter("@Action", action);
                noOfEffectedRows = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnectionString(string.Empty), CommandType.StoredProcedure, spName, parameters);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}