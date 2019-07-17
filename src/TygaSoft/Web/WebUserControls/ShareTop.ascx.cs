using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.WebUserControls
{
    public partial class ShareTop : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //BindMenu();

                Bind();
            }
        }

        private void Bind()
        {
            
        }

        private void BindMenu()
        {
            XElement root = XElement.Load(Server.MapPath("~/App_Data/ShareSitemap.xml"));
            
            var rootNode = root.Descendants().FirstOrDefault(x => x.Attribute("Title").Value == "汇凯博资管");
            if(rootNode == null) return;
            var q = from r in root.Descendants()
                    where r.Attribute("ParentId").Value == rootNode.Attribute("Id").Value
                    select new { url = r.Attribute("Url"), title = r.Attribute("title") };

           
        }
    }
}