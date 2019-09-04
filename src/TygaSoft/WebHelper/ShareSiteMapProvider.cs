using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Collections;
using System.IO;
using System.Collections.Specialized;
using TygaSoft.SysHelper;

namespace TygaSoft.WebHelper
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ShareSiteMapProvider : SiteMapProvider
    {
        XElement root = null;

        public ShareSiteMapProvider()
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/Files/Xml/ShareSitemap.xml"));
        }

        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            XElement currNode = null;
            var id = Guid.Empty;
            if(rawUrl.IndexOf('?') > -1)
            {
                Guid.TryParse(WebCommon.GetQueryStringByKey(rawUrl, "Id"), out id);
            }
            if(!id.Equals(Guid.Empty)) currNode = root.Descendants().First(x => x.Attribute("Id").Value == id.ToString());
            else currNode = root.Descendants().First(x => x.Attribute("Url").Value.ToLower().Contains(rawUrl.ToLower()));

            return new SiteMapNode(this,
                    currNode.Attribute("Id").Value,
                    currNode.Attribute("Url").Value,
                    currNode.Attribute("Title").Value,
                    currNode.Attribute("Description").Value);
        }

        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        {
            var q = root.Descendants().Where(x => x.Attribute("ParentId").Value == node.Key);
            if (q == null || q.Count() == 0) return null;
            SiteMapNodeCollection smnc = new SiteMapNodeCollection();
            foreach (var item in q)
            {
                smnc.Add(new SiteMapNode(this,
                    item.Attribute("Id").Value,
                    item.Attribute("Url").Value,
                    item.Attribute("Title").Value,
                    item.Attribute("Description").Value));
            }

            return smnc;
        }

        public override SiteMapNode GetParentNode(SiteMapNode node)
        {
            var currNode = root.Descendants().FirstOrDefault(x => x.Attribute("Id").Value == node.Key);
            if (currNode == null) return null;

            var parentNode = root.Descendants().FirstOrDefault(x => x.Attribute("Id").Value == currNode.Attribute("ParentId").Value);
            if (parentNode == null) return null;

            var temp = new SiteMapNode(this,
                                parentNode.Attribute("Id").Value,
                                parentNode.Attribute("Url").Value,
                                parentNode.Attribute("Title").Value,
                                parentNode.Attribute("Description").Value);

            return temp;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            var siteMapNode = root.Descendants().FirstOrDefault(x => x.Attribute("Title").Value == "前台功能菜单").Elements().First();
            if (siteMapNode == null) throw new Exception("菜单导航配置文件错误，请检查");

            var temp = new SiteMapNode(this,
                                siteMapNode.Attribute("Id").Value,
                                siteMapNode.Attribute("Url").Value,
                                siteMapNode.Attribute("Title").Value,
                                siteMapNode.Attribute("Description").Value);

            return temp;
        }
    }
}
