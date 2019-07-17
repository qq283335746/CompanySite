using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.WebHelper;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Shares
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            ShareSiteMapProvider ssmp = new ShareSiteMapProvider();
            var currNode = ssmp.FindSiteMapNode(HttpContext.Current);
            var bll = new ContentDetail();
            var model = bll.GetModelByTitle(currNode.Title);
            if (model == null)
            {
                MessageBox.Messager(this.Page, MC.Submit_Data_NotExists, MC.AlertTitle_Ex_Error, "error");
                return;
            }

            ltrMyData.Text = model.ContentText;
        }
    }
}