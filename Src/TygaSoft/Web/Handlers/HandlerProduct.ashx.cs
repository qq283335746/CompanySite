using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TygaSoft.Web.Handlers
{
    /// <summary>
    /// HandlerProduct 的摘要说明
    /// </summary>
    public class HandlerProduct : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string action = "";
            switch (context.Request.HttpMethod.ToUpper())
            {
                case "GET":
                    action = context.Request.QueryString["action"];
                    break;
                case "POST":
                    action = context.Request.Form["action"];
                    break;
                default:
                    break;
            }

            switch (action)
            {
                case "ProductAgreement":
                    ProductAgreement(context);
                    break;
                default:
                    break;
            }
        }

        private void ProductAgreement(HttpContext context)
        {
            HttpCookie cookie = new HttpCookie("IsProductAgree");
            cookie.HttpOnly = true;
            cookie.Value = DateTime.Now.AddHours(12).ToString();
            context.Response.Cookies.Add(cookie);
            context.Response.Write("{\"success\": true,\"message\": \"\"}");
            //context.Response.Redirect("/s/tt.html?do=1",true);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}