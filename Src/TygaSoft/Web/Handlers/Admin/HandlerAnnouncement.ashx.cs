using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Handlers.Admin
{
    /// <summary>
    /// HandlerAnnouncement 的摘要说明
    /// </summary>
    public class HandlerAnnouncement : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string msg = "";
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
                    case "SaveAnnouncement":
                        SaveAnnouncement(context);
                        break;
                    case "SaveNotice":
                        SaveNotice(context);
                        break;
                    case "GetJsonForNoticeDatagrid":
                        GetJsonForNoticeDatagrid(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (msg != "")
            {
                context.Response.Write("{\"success\": false,\"message\": \"" + msg + "\"}");
            }

            //Stream stream = context.Request.InputStream;

            //long length = stream.Length;
            //byte[] data = context.Request.BinaryRead((int)length);//对当前输入流进行指定字节数的二进制读取 
            //string reqStr = System.Text.Encoding.UTF8.GetString(data);//解码为UTF8编码形式的字符串 

            //StreamReader steamRd = new StreamReader(stream);
            //string strPostData = steamRd.ReadToEnd();
            //string aa2 = HttpUtility.UrlDecode(strPostData, System.Text.Encoding.GetEncoding("utf-8"));

            //string aa = HttpUtility.UrlDecode(strPostData);

            //Stream stream = context.Request.InputStream;

            //StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            //string reqStr = HttpUtility.HtmlDecode(reader.ReadToEnd());
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
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
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

        private void SaveNotice(HttpContext context)
        {
            try
            {
                string Id = context.Request.Form["ctl00$cphMain$hId"].Trim();
                string sContentTypeId = context.Request.Form["contentTypeId"].Trim();
                string title = context.Request.Form["ctl00$cphMain$txtTitle"].Trim();
                string sDescr = context.Request.Form["ctl00$cphMain$txtaDescr"].Trim();
                string content = context.Request.Form["txtContent"].Trim();
                content = HttpUtility.HtmlDecode(content);
                Guid gId = Guid.Empty;
                if (Id != "") Guid.TryParse(Id, out gId);
                Guid contentTypeId = Guid.Empty;
                Guid.TryParse(sContentTypeId, out contentTypeId);

                NoticeInfo model = new NoticeInfo();
                model.LastUpdatedDate = DateTime.Now;
                model.Title = title;
                model.Descr = sDescr;
                model.ContentText = content;
                model.ContentTypeId = contentTypeId;
                model.Id = gId;

                Notice bll = new Notice();
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
                    context.Response.Write("{\"success\": false,\"message\": \"" + MessageContent.Submit_Exist + "\"}");
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

        private void GetJsonForNoticeDatagrid(HttpContext context)
        {
            int totalRecords = 0;
            int pageIndex = 1;
            int pageSize = 10;
            int.TryParse(context.Request.QueryString["page"], out pageIndex);
            int.TryParse(context.Request.QueryString["rows"], out pageSize);
            string sqlWhere = string.Empty;
            SqlParameter parm = null;
            if (!string.IsNullOrEmpty(context.Request.QueryString["title"]))
            {
                sqlWhere = "and Title like @Title ";
                parm = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
                parm.Value = "%" + context.Request.QueryString["title"].Trim() + "%";
            }

            Notice bll = new Notice();
            var list = bll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere, parm);
            if (list == null || list.Count == 0)
            {
                context.Response.Write("{\"total\":0,\"rows\":[]}");
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var model in list)
            {
                sb.Append("{\"Id\":\"" + model.Id + "\",\"Title\":\"" + model.Title + "\"},");
            }
            context.Response.Write("{\"total\":" + totalRecords + ",\"rows\":[" + sb.ToString().Trim(',') + "]}");
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