using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using TygaSoft.BLL;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.WebUserControls
{
    public partial class UCRArticleDetail : System.Web.UI.UserControl
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
            var Id = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["aId"])) Guid.TryParse(Request.QueryString["aId"], out Id);
            if(!Id.Equals(Guid.Empty))
            {
                Announcement bll = new Announcement();
                var model = bll.GetModel(Id);
                if (model == null)
                {
                    MessageBox.Messager(this.Page, "当前数据不存在或已被删除", "提示", "error");
                    return;
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"<div class=""c_t""><h1>{0}</h1><span>{1}</span></div>",model.Title,model.LastUpdatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendFormat(@"<div class=""c_d"">{0}</div>",model.ContentText);

                ltrHtml.Text = sb.ToString();
            }
        }
    }
}