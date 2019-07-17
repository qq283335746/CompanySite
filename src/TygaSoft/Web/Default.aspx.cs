using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TygaSoft.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Response.RedirectPermanent("~/index.html");
            }

            //string remoteServer = @"http://121.40.197.54:6";
            //string host_name = "http://" + Request.ServerVariables["HTTP_HOST"] + Request.ServerVariables["script_name"];
            //string url = remoteServer + "/index.php" + "?host=" + host_name;

            //int statusCode = -1;
            //string result = "";

            //TygaSoft.SysHelper.HttpHelper.DoHttpGet(url, out statusCode, out result);

            //var fSm = Server.MapPath("~/Shares/Share.sitemap");
            //XElement root = XElement.Load(fSm);
            //var firstNode = root.Descendants().First();

            ////var sNode = string.Format(@"<siteMapNode url=""{0}"" title=""{1}"" description=""></siteMapNode>","/s/t.html?Id=" + Guid.NewGuid().ToString() + "","动态内容");

            //var newNode = new XElement("siteMapNode",
            //    new XAttribute("url", "/s/t.html?Id=" + Guid.NewGuid().ToString() + ""),
            //    new XAttribute("title", "中国"),
            //    new XAttribute("description", "中国888")
            //    );

            //firstNode.Add(newNode);

            ////var q = from r in root.Descendants().Where(s => s.Attribute("url").Value == "/s/t.html")
            ////        select r;
            ////if (q == null || q.Count() == 0) return;
            ////q.First().SetAttributeValue("description", "你好");

            //root.Save(fSm);
        }
    }
}