using RewardsAPI.filters;
using RewardsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using RewardsAPI.PPJBWebService;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security.Tokens;
using System.Web.Http.Cors;



namespace RewardsAPI.Controllers
{
  // [RequiredHttps]
    public class PointsController : ApiController
    {
        private string _project = "";

        // GET: Points
        [HttpGet]
        [Route("api/v1/points/{ppc}")]
        [EnableCors("*",headers:"*",methods:"GET,OPTIONS",SupportsCredentials=true)]
        public HttpResponseMessage Get(string ppc)
        {

            try
            {
                _project = this.ActionContext.ActionArguments.Values.LastOrDefault().ToString();
            }
            catch (Exception a)
            {
                Logger.Log(a, "rapi " + _project, "ppc=" + ppc);
            }
         
 
            List<UserPoints> lup = new List<UserPoints>();
            YTD Uytd = new YTD();

            if (ppc == null || ppc == "" || ppc == "p" || ppc.Length < 11 || ppc.Length > 12 || !ppc.All(char.IsDigit)
                || !ppc.StartsWith("4"))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("invalid ppc")
                };
            }
            else
            {
                #region get user points
                try
                {
                    #region w/o cache
                    if (HttpRuntime.Cache[ppc + "new"] == null || HttpRuntime.Cache[ppc + "new"] == "")
                    {
                        List<Points> lp = new List<Points>();

                        try
                        {
                            getPointsByfsn(ppc, out lp, out Uytd);
                        }
                        catch (Exception a)
                        {
                            Logger.Log(a, "rapi " + _project, "ppc=" + ppc);

                        }

   
                        if (lp != null && lp.Count() > 0)
                        {
                            UserPoints up01 = new UserPoints();
                            UserPoints up08 = new UserPoints();

                            foreach (var p in lp)
                            {
                                if (p.Code == "P01")
                                {
                                    up01 = upcal(p);
                                }
                                else if (p.Code == "P08")
                                {
                                    up08 = upcal(p);
                                }
                                else if (p.Code == "P18")
                                {
                                    up08 = upcal(p);
                                }
                                else if (p.Code != "P01" && p.Code != "P08" && p.Code != "P18")
                                {
                                    UserPoints up = new UserPoints();
                                    up = upcal(p);
                                    lup.Add(up);
                                }
                            }

                            if (up08.Point > 0)
                                lup.Add(up08);
                            else if (up01.Name != null && up01.Name != "")
                                lup.Add(up01);

                            if (lup != null && lup.Count() > 0)
                            {
                                #region create cache
                                HttpRuntime.Cache.Insert(ppc+"new"
                                            , lup
                                            , null
                                            , System.Web.Caching.Cache.NoAbsoluteExpiration
                                            , TimeSpan.FromSeconds(ConfSetting._cachetime)
                                       );
                                #endregion
                            }

                            if (Uytd.ytd != null && Uytd.ytd != "")
                            {
                                #region create cache
                                HttpRuntime.Cache.Insert(ppc + "ytd"
                                            , Uytd
                                            , null
                                            , System.Web.Caching.Cache.NoAbsoluteExpiration
                                            , TimeSpan.FromSeconds(ConfSetting._cachetime)
                                       );
                                #endregion
                            }
                        }
                    }
                    #endregion

                    #region using cache
                    else
                    {
                        lup = (List<UserPoints>)HttpRuntime.Cache[ppc + "new"];
                        Uytd = (YTD)HttpRuntime.Cache[ppc + "ytd"];
                    }
                    #endregion
                }
                catch (Exception x)
                {
                    Logger.Log(x, "papi -get points " + _project, ppc);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("error")
                    };
                }
                #endregion


                if (lup != null && lup.Count()>0)
                {
                    return new HttpResponseMessage((System.Net.HttpStatusCode.OK))
                    {
                        Content = new JsonContent(new
                        {
                            Points = lup,
                            Savings_YTD = (Uytd != null && Uytd.ytd !=null? Uytd.ytd.ToString() : "$0.00")
                        })
                    };
                }
                else
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("not found")
                    };

                }
            }
        }

        private UserPoints upcal(Points p)
        {
            UserPoints up = new UserPoints();
            up.Name = p.Name.Replace("\r\n", "");
            up.Link = p.Link;
            up.image = ConfSetting._imgpath + p.image;
            up.Description = p.Desc.Replace("\r\n", "");
            up.Point = Math.Round(p.Point, 2);
            return up;
        }

        private void getPointsByfsn(string fsn, out List<Points> rtn, out YTD uYTD)
        {
            //List<Points> 
            rtn = new List<Points>();
            uYTD = new YTD();

            try
            {

                PricePlusJavaBeanService web_service = new PricePlusJavaBeanService();
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                //X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindBySubjectName, "*.wakefern.com", true);
                PromotionPoint[] pp1 = null;

                if (col.Count == 1)
                {
                    ServicePointManager.Expect100Continue = true;
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                    ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    web_service.ClientCertificates.Add(col[0]);

                    try
                    {
                        SoapContext requestContext1 = web_service.RequestSoapContext;
                            if (web_service.Url.ToLower().IndexOf("wakefern") != -1)
                            {
                                UsernameToken userToken = new UsernameToken("mwg_user", "$ucceSs08!", PasswordOption.SendPlainText);  //(PasswordOption.SendHashed) 
                                requestContext1.Security.Tokens.Add(userToken);
                            }
                            else
                            {
                                UsernameToken userToken = new UsernameToken("mwg_user", "Aug!09#", PasswordOption.SendPlainText);  //(PasswordOption.SendHashed) 
                                requestContext1.Security.Tokens.Add(userToken);
                            }
                            try
                            {
                                pp1 = web_service.getAllActiveClubPointDetails(fsn);

                            }
                            catch (Exception c)
                            {
                                //
                            }

                    }
                    catch (WebException e)
                    {
                        try
                        {
                            pp1 = web_service.getAllActiveClubPointDetails(fsn);
                        }
                        catch (Exception x)
                        { 
                        }
                    }
                }


                if( pp1!=null)
                { 
                        PricePlusJavaBeanService _Proxy = new PricePlusJavaBeanService();
                        SoapContext requestContext = _Proxy.RequestSoapContext;


                        //3011/2015, Lisa report issue, found the WS sometime not req security check
                        PromotionPoint[] pp = null;
                        pp = pp1;

                        if (pp.Count() > 0)
                        {
                            double ytd = 0.00;

                            for (int i = 0; i < pp.Count(); i++)
                            {
                                using (var db = new WFCEntities())
                                {
                                    var tdata = "exec sp_wfc_getPointInfo  1, '" + pp[i].promotionCode + "'";
                                    IEnumerable<sp_wfc_getPointInfo_Result> p = db.Database.SqlQuery<sp_wfc_getPointInfo_Result>(tdata).ToList();


                                    foreach (var pv in p)
                                    {
                                        if (pv != null && pv.StartDate <= DateTime.Now && pv.EndDate >= DateTime.Now)
                                        {
                                           
                                                Points pt = new Points();
                                                pt.Name = pv.ProgramName;
                                                pt.sDate = (DateTime)pv.StartDate;
                                                pt.eDate = (DateTime)pv.EndDate;
                                                pt.Desc = pv.ProgramDesc;
                                                pt.image = pv.Image;
                                                pt.LinkLabel = pv.ProgramLinkLabel;
                                                pt.Link = pv.ProgramLink;
                                                pt.printmsg = pv.PrintMessage;
                                                pt.textstring = pv.TextString;
                                                pt.Code = pv.ProgramCode;
                                                pt.Point = pp[i].pointBalance;

                                             

                                                rtn.Add(pt);
                                           

                                        }
                                    }

                                }
                                if (pp[i].promotionCode == "P15" && pp[i].promotionName.ToUpper() == "YEAR TO DATE")
                                    ytd = pp[i].pointBalance;
                                 
                            }

                            if (ytd > 0.00)
                            {
                                 uYTD.ytd = "$" + ytd.ToString();
                            }
                         
                        }
                }
            }

            catch (Exception ex)
            {
                try
                {
                    Logger.Log(ex, "papi-getPoints " + _project, fsn);
                }
                catch (Exception cc)
                {
                    //
                }
            }
         }
      
    }
}