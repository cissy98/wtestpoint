using RewardsAPI.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using wfcverify;

namespace RewardsAPI.filters
{
    public class TokenValidationAttribute : ActionFilterAttribute
    {
      
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string internatoken="";
            #region check valid token
            try
            {
              
                //if (actionContext.Request.Headers.GetValues("Authorization").First().ToLower() == System.Configuration.ConfigurationManager.AppSettings["exchkeyRA"].ToString())
                //{
                //    #region ask new token
                //    if (actionContext.Request.RequestUri.AbsolutePath.ToLower().IndexOf("/authorization") != -1 &&
                //        (actionContext.Request.RequestUri.AbsolutePath.ToLower().EndsWith("/authorization") ||
                //        actionContext.Request.RequestUri.AbsolutePath.ToLower().EndsWith("/authorization/")) )
                //    {
                //        var method=actionContext.Request.Method.ToString();
                //        base.OnActionExecuting(actionContext);
                //        internatoken = "ok";
                //     }
                //     else
                //     {
                //            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                //            {
                //                Content = new StringContent("Not Found")
                //            };

                //            return;
                //       }
                //      #endregion
                //}
                #region using wrong token
                //else 
                if (actionContext.Request.RequestUri.AbsolutePath.ToLower().IndexOf("/authorization") != -1 &&
                        (actionContext.Request.RequestUri.AbsolutePath.ToLower().EndsWith("/authorization") ||
                        actionContext.Request.RequestUri.AbsolutePath.ToLower().EndsWith("/authorization/")) )
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Invalid Authorization-Token")
                    };

                    return;
                }
                #endregion
            }
            catch (Exception x)
            {
           
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway)
                {
                    Content = new StringContent(x.Message)
                };

                return;
            }
             #endregion


           
            if (internatoken == null || internatoken == "")
            {
                Boolean vld=false;

                var method = actionContext.Request.Method.ToString();

                #region check if valid user access
                try
                {
                    #region using db
                 
                    using (var db = new WFCEntities())
                     {

                        //5/14/2018

                         string auth = "";
                         try
                         {
                             auth = actionContext.Request.Headers.GetValues("Authorization").First();
                         }
                         catch (Exception a)
                         {
                             auth = actionContext.Request.RequestUri.Query.ToString().ToLower().Replace("?authorization=", "");
                         }

                        var sp = "exec sp_wfcapi_chkUsersDEP '" + auth+"'";
                        sp_wfcapi_chkUsersDEP_Result rtn = db.Database.SqlQuery<sp_wfcapi_chkUsersDEP_Result>(sp).FirstOrDefault();

                        if (rtn.ed < DateTime.Now)
                        {
                            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                            {
                                Content = new StringContent("Invalid Authorization-Token (Token expired)")
                            };
                            return;

                        }
                        else if (rtn != null && rtn.hi != null && rtn.hi != "")
                        {
                            internatoken = rtn.hi;
                            try
                            {
                                string basestr = RSACls.DecryptFromText(internatoken);
                                if (basestr == null || basestr == "")
                                {
                                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                                    {
                                        Content = new StringContent("Unauthorized User")
                                    };

                                    return;
                                }

                                string basecomp="";
                                //5/14/2018
                                //basecomp = RSACls.DecryptFromText(WFCAPIKeyClass.keyvalues(rtn.usr , actionContext.Request.Headers.GetValues("Authorization").First()).hdkey);
                                basecomp = RSACls.DecryptFromText(WFCAPIKeyClass.keyvalues(rtn.usr, auth).hdkey);
                                if (basestr == basecomp)
                                {
                                    //3/14/2018
                                    //actionContext.ActionArguments.Add("proj", rtn.usr+" "+rtn.prj);
                                    actionContext.ActionArguments.Add("proj", rtn.prj);
                                    base.OnActionExecuting(actionContext);
                                    vld = true;
                                }
                                if (!vld)
                                {
                                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                                        {
                                            Content = new StringContent("Unauthorized User")
                                        };

                                        return;

                                 }
                            }
                            catch (Exception xx)
                            {
                                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                                {
                                    Content = new StringContent("Unauthorized User")
                                };
                                return;
                            }
                        }
                        else
                            internatoken = "";
                    }
                    #endregion

                }
                catch (Exception x)
                {

                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Missing Authorization-Token")
                    };
                    return;
                }
                #endregion check if valid user access

                #region verify content type
                //try
                //{
                //    string contenttype = (actionContext.Request.Content.Headers.ContentType != null && actionContext.Request.Content.Headers.ContentType.ToString() != "" ? actionContext.Request.Content.Headers.ContentType.ToString() : "");
                //    if (contenttype == null || contenttype == "" || contenttype.ToLower() != "application/json")
                //    {
                //        try
                //        {
                //            contenttype = actionContext.ControllerContext.Request.Headers.Accept.FirstOrDefault().MediaType.ToString();
                //        }
                //        catch (Exception x)
                //        {
                //            contenttype = "aa";
                //        }
                //    }
                //    if (contenttype == null || contenttype == "")
                //    {
                //        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                //        {
                //            Content = new StringContent("Missing Content-Type")
                //        };
                  
                //        return;
                //    }
                //    else if (contenttype.ToLower() != "application/json")
                //    {
                //        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                //        {
                //            Content = new StringContent("Invalid Content-Type")
                //        };

                //        return;
                //    }
                //}
                //catch (Exception xx)
                //{
           
                //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                //    {
                //        Content = new StringContent("Invalid Content-Type")
                //    };

           
                //    return;
                //}

                #endregion

            }

        }
    }

}