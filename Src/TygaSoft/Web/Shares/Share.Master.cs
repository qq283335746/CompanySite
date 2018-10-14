using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TygaSoft.Web.Shares
{
    public partial class Share : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadShareTop();

                LoadShareFooter();
            }
        }

        private void LoadShareTop()
        {
            if (!phShareTop.HasControls())
            {
                Control ctl = this.LoadControl("~/WebUserControls/ShareTop.ascx");
                ctl.ID = "ShareTopAscx";
                phShareTop.Controls.Add(ctl);
            }
        }

        private void LoadShareFooter()
        {
            if (!phShareFooter.HasControls())
            {
                Control ctl = this.LoadControl("~/WebUserControls/ShareFooter.ascx");
                ctl.ID = "ShareFooterAscx";
                phShareFooter.Controls.Add(ctl);
            }
        }
    }
}