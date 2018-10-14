using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.BLL;

namespace TygaSoft.Web.WebUserControls
{
    public partial class UCRTrend : System.Web.UI.UserControl
    {
        protected Guid Id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"])) Guid.TryParse(Request.QueryString["Id"], out Id);
            if(!Id.Equals(Guid.Empty))
            {
                Announcement bll = new Announcement();
                rpData.DataSource = bll.GetListByTypeId(Id, 1, 10);
                rpData.DataBind();
            }
            
        }
    }
}