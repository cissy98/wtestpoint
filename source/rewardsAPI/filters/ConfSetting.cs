using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RewardsAPI.filters
{
    public static class ConfSetting
    {
        public static string _proxy = (System.Configuration.ConfigurationManager.AppSettings["RegProxy"] == null ? "N" : System.Configuration.ConfigurationManager.AppSettings["RegProxy"].ToString());
        public static int _cachetime = (System.Configuration.ConfigurationManager.AppSettings["CTime"] == null ? 20 : Convert.ToInt32((System.Configuration.ConfigurationManager.AppSettings["CTime"].ToString())));
        public static string _imgpath = (System.Configuration.ConfigurationManager.AppSettings["imgpath"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["imgpath"].ToString());

        //public static string _Tempkey_RA = (System.Configuration.ConfigurationManager.AppSettings["exchkeyRA"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["exchkeyRA"].ToString());
        //public static int _Tempkey_days = (System.Configuration.ConfigurationManager.AppSettings["expdays"] == null ? 0 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["expdays"].ToString()));

        //public static int _confCount = (System.Configuration.ConfigurationManager.AppSettings["confCount"] == null ? 0 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["confCount"].ToString()));
        //public static int _confCountMax = (System.Configuration.ConfigurationManager.AppSettings["confCount"] == null ? 0 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["confCount"].ToString()));

        //public static string _YTSID = (System.Configuration.ConfigurationManager.AppSettings["YTSID"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["YTSID"].ToString());
        //public static string _YTRID = (System.Configuration.ConfigurationManager.AppSettings["YTRID"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["YTRID"].ToString());
        //public static string _ShowYT = (System.Configuration.ConfigurationManager.AppSettings["ShowYT"] == null ? "N" : System.Configuration.ConfigurationManager.AppSettings["ShowYT"].ToString());
        //public static int _YTCPUPD = (System.Configuration.ConfigurationManager.AppSettings["YTCPUPD"] == null ? 0 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["YTCPUPD"].ToString()));
        //public static string _YTRN = (System.Configuration.ConfigurationManager.AppSettings["YTRN"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["YTRN"].ToString());

        //public static string _ytcpsite = (System.Configuration.ConfigurationManager.AppSettings["YTCPSITE"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["YTCPSITE"].ToString());
        //public static string _ytgetcpid4cd = (System.Configuration.ConfigurationManager.AppSettings["YTGetCPId4CD"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["YTGetCPId4CD"].ToString());

        //public static string _YTKEY = (System.Configuration.ConfigurationManager.AppSettings["YTkey"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["YTKey"].ToString());

        //public static string _ppurchase = (System.Configuration.ConfigurationManager.AppSettings["PastPurchaseUri"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["PastPurchaseUri"].ToString());

        //public static int _chkPPdays = (System.Configuration.ConfigurationManager.AppSettings["chkPPdays"] == null ? 30 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["chkPPdays"].ToString()));
        //public static string _shoplistUrijson = (System.Configuration.ConfigurationManager.AppSettings["ShopListUri"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["ShopListUri"].ToString());
        //public static string _productUri = (System.Configuration.ConfigurationManager.AppSettings["ProductUri"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["ProductUri"].ToString());

        //public static string _baseUri = (System.Configuration.ConfigurationManager.AppSettings["baseUri"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["baseUri"].ToString());

        //public static string _AppToken = (System.Configuration.ConfigurationManager.AppSettings["AppToken"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["AppToken"].ToString());

        //public static string _shoplistUrixml = (System.Configuration.ConfigurationManager.AppSettings["ShopListUrixml"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["ShopListUrixml"].ToString());
     
        
        public static string GetExceptionMessages(this Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs.Replace("Error:", "") == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            return msgs;
        }
    }
}