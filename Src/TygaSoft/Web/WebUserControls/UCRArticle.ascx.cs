using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.BLL;

namespace TygaSoft.Web.WebUserControls
{
    public partial class UCRArticle : System.Web.UI.UserControl
    {
        protected int totalRecords = 0;
        protected int totalPages = 0;
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
            if (!Id.Equals(Guid.Empty))
            {
                Announcement bll = new Announcement();
                int pageIndex = 1;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["pageSize"])) int.TryParse(Request.QueryString["pageSize"], out pageIndex);
                if (pageIndex < 1) pageIndex = 1;

                rpData.DataSource = bll.GetListByTypeId(Id, pageIndex, 10, out totalRecords);
                rpData.DataBind();

                totalPages = totalRecords / 10;
                if (totalPages < 1) totalPages = 1;
            }
        }
    }
}