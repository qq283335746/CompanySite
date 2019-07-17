using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Handlers.Admin.AboutSite
{
    /// <summary>
    /// HandlerAboutSite 的摘要说明
    /// </summary>
    public class HandlerAboutSite : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string reqName = "";
                switch (context.Request.HttpMethod.ToUpper())
                {
                    case "GET":
                        reqName = context.Request.QueryString["reqName"];
                        break;
                    case "POST":
                        reqName = context.Request.Form["reqName"];
                        break;
                    default:
                        break;
                }

                switch (reqName)
                {
                    case "SaveContentDetail":
                        SaveContentDetail(context);
                        break;
                    case "SaveAnnouncement":
                        SaveAnnouncement(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + ex.Message + "\"}");
            }
        }

        private void SaveContentDetail(HttpContext context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$txtTitle"]))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MC.Submit_Params_InvalidError + "\"}");
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request.Form["contentTypeId"]))
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MC.Submit_Params_InvalidError + "\"}");
                    return;
                }

                Guid gId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$hId"].Trim()))
                {
                    Guid.TryParse(context.Request.Form["ctl00$cphMain$hId"].Trim(), out gId);
                }

                Guid contentTypeId = Guid.Empty;
                Guid.TryParse(context.Request.Form["contentTypeId"], out contentTypeId);
                Guid pictureId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$hPictureId"]))
                {
                    Guid.TryParse(context.Request.Form["ctl00$cphMain$hPictureId"], out pictureId);
                }
                int virtualViewCount = 0;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$txtVirtualViewCount"]))
                {
                    int.TryParse(context.Request.Form["ctl00$cphMain$txtVirtualViewCount"], out virtualViewCount);
                }
                int sort = 0;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$txtSort"]))
                {
                    int.TryParse(context.Request.Form["ctl00$cphMain$txtSort"], out sort);
                }
                bool isDisable = false;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["isDisable"]))
                {
                    bool.TryParse(context.Request.Form["isDisable"], out isDisable);
                }

                ContentDetailInfo model = new ContentDetailInfo();
                model.Id = gId;
                model.LastUpdatedDate = DateTime.Now;
                model.Title = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                model.PictureId = pictureId;
                model.Descr = context.Request.Form["ctl00$cphMain$txtaDescr"] == null ? "" : context.Request.Form["ctl00$cphMain$txtaDescr"].Trim();
                model.ContentText = context.Request.Form["txtContent"] == null ? "" : HttpUtility.HtmlDecode(context.Request.Form["txtContent"].Trim());
                model.ContentTypeId = contentTypeId;
                model.VirtualViewCount = virtualViewCount;
                model.Sort = sort;
                model.IsDisable = isDisable;

                ContentDetail bll = new ContentDetail();
                int effect = -1;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        if (bll.IsExist(model.ContentTypeId, model.Id))
                        {
                            scope2.Complete();
                            scope.Complete();
                            context.Response.Write("{\"success\": false,\"message\": \"已存在该类别的内容，请勿重复操作\"}");
                            return;
                        }
                    }
                    if (!gId.Equals(Guid.Empty))
                    {
                        effect = bll.Update(model);
                    }
                    else
                    {
                        effect = bll.Insert(model);
                    }

                    scope.Complete();
                }
                if (effect == 110)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MC.Submit_Exist + "\"}");
                    return;
                }
                if (effect != 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"操作成功\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"异常：" + ex.Message + "\"}");
            }
        }

        private void SaveAnnouncement(HttpContext context)
        {
            try
            {
                string Id = context.Request.Form["ctl00$cphMain$hId"].Trim();
                string title = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                string sContentTypeId = context.Request.Form["contentTypeId"].Trim();
                string sDescr = context.Request.Form["ctl00$cphMain$txtaDescr"].Trim();
                string content = context.Request.Form["txtContent"].Trim();
                content = HttpUtility.HtmlDecode(content);
                Guid gId = Guid.Empty;
                if (Id != "") Guid.TryParse(Id, out gId);
                Guid contentTypeId = Guid.Empty;
                Guid.TryParse(sContentTypeId, out contentTypeId);
                int virtualViewCount = 0;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$txtVirtualViewCount"]))
                {
                    int.TryParse(context.Request.Form["ctl00$cphMain$txtVirtualViewCount"], out virtualViewCount);
                }
                int sort = 0;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["ctl00$cphMain$txtSort"]))
                {
                    int.TryParse(context.Request.Form["ctl00$cphMain$txtSort"], out sort);
                }
                bool isDisable = false;
                if (!string.IsNullOrWhiteSpace(context.Request.Form["isDisable"]))
                {
                    bool.TryParse(context.Request.Form["isDisable"], out isDisable);
                }

                AnnouncementInfo model = new AnnouncementInfo();
                model.Id = gId;
                model.LastUpdatedDate = DateTime.Now;
                model.Title = title;
                model.Descr = sDescr;
                model.ContentText = content;
                model.ContentTypeId = contentTypeId;
                model.VirtualViewCount = virtualViewCount;
                model.Sort = sort;
                model.IsDisable = isDisable;

                Announcement bll = new Announcement();
                int effect = -1;

                using (TransactionScope scope = new TransactionScope())
                {
                    if (!gId.Equals(Guid.Empty))
                    {
                        effect = bll.Update(model);
                    }
                    else
                    {
                        effect = bll.Insert(model);
                    }

                    scope.Complete();
                }

                if (effect == 110)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"" + MC.Submit_Exist + "\"}");
                    return;
                }
                if (effect != 1)
                {
                    context.Response.Write("{\"success\": false,\"message\": \"操作失败，请正确输入\"}");
                    return;
                }

                context.Response.Write("{\"success\": true,\"message\": \"操作成功\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false,\"message\": \"异常：" + ex.Message + "\"}");
            }
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