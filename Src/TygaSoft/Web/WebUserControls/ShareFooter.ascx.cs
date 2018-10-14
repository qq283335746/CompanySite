using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.SysHelper;
using TygaSoft.WebHelper;
using TygaSoft.BLL;

namespace TygaSoft.Web.WebUserControls
{
    public partial class ShareFooter : System.Web.UI.UserControl
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
            var bll = new ContentDetail();
            var model = bll.GetModelByTypeCode(EnumData.MenuRoot.PageFooter.ToString());
            if (model != null)
            {
                ltrFooter.Text = model.ContentText;
            }
        }
    }
}