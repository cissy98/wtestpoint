using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Diagnostics;

namespace RewardsAPI.Models
{
    public class Logger
    {
        public static void Log(Exception exception, string func, string uid)
        {
            StringBuilder sbExceptionMessage = new StringBuilder();
            string shrt = exception.GetType().Name;
            do
            {
                sbExceptionMessage.Append("Exception Type" + Environment.NewLine);
                sbExceptionMessage.Append(exception.GetType().Name);
                sbExceptionMessage.Append(Environment.NewLine + Environment.NewLine);
                sbExceptionMessage.Append("Message" + Environment.NewLine);
                sbExceptionMessage.Append(exception.Message + Environment.NewLine + Environment.NewLine);
                sbExceptionMessage.Append("Stack Trace" + Environment.NewLine);
                sbExceptionMessage.Append(exception.StackTrace + Environment.NewLine + Environment.NewLine);

                exception = exception.InnerException;
            } while (exception != null);


            LogToDB("ERR", (shrt != null ? shrt : ""), sbExceptionMessage.ToString(),func,uid);
        }

        public static void RTLog(string shrtmsg, string lngmsg, string func, string uid)
        {
             LogToDB("RTN",shrtmsg,lngmsg,func, uid);
        } 

        private static void LogToDB(string ind,string shrtmsg, string lngmsg, string func, string uinfo)
        {
            try
            {
                using (var db = new WFCEntities())
                {
                    var tt = "exec sp_wfc_PB_spInsertLog '" + ind + "', '" + shrtmsg +"','" +lngmsg + "','"+ func + "','"+uinfo +"'";

                    int iRet = db.Database.ExecuteSqlCommand(tt);
                }
            }
            catch (Exception e)
            {
                //
            }
            
        }
    }
}