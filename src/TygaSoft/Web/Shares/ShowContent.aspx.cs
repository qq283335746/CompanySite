using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using TygaSoft.WebHelper;
using TygaSoft.SysHelper;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Shares
{
    public partial class ShowContent : System.Web.UI.Page
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

            this.Page.Title = currNode.Title;

            var aId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["aId"])) Guid.TryParse(Request.QueryString["aId"], out aId);
            if (!aId.Equals(Guid.Empty))
            {
                if (phUc.FindControl("UCRArticleDetail") == null)
                {
                    Control ctl = this.LoadControl("~/WebUserControls/UCRArticleDetail.ascx");
                    ctl.ID = "UCRArticleDetail";
                    phUc.Controls.Clear();
                    phUc.Controls.Add(ctl);
                }
            }
            else
            {
                BindUc(currNode.Title);
            }

            var bll = new ContentDetail();
            var model = bll.GetModelByTitle(currNode.Title);
            if (model == null)
            {
                MessageBox.Messager(this.Page, MC.Submit_Data_NotExists, MC.AlertTitle_Ex_Error, "error");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(model.ContentText);

            string picUrl = PictureUrlHelper.GetUrl(model.FileDirectory, model.RandomFolder, model.FileExtension, EnumData.PictureType.OriginalPicture, EnumData.Platform.PC);
            int width = 0;
            int height = 0;

            sb.AppendFormat(@"<div id=""myDataAppend"" style=""display:none;"">{{""banner"":""{0}"",""width"":""{1}"",""height"":""{2}""}}</div>", picUrl.Replace("/Files/","/sw/Files/"),width,height);

            ltrMyData.Text = sb.ToString();
        }

        private void BindUc(string menu)
        {
            switch (menu)
            {
                case "汇凯博资管动态":
                    if (phUc.FindControl("UCRTrend") == null)
                    {
                        Control ctl = this.LoadControl("~/WebUserControls/UCRTrend.ascx");
                        ctl.ID = "UCRTrend";
                        phUc.Controls.Clear();
                        phUc.Controls.Add(ctl);
                    }
                    break;
                case "汇凯博资管视点":
                    if (phUc.FindControl("UCRArticle") == null)
                    {
                        Control ctl = this.LoadControl("~/WebUserControls/UCRArticle.ascx");
                        ctl.ID = "UCRArticle";
                        phUc.Controls.Clear();
                        phUc.Controls.Add(ctl);
                    }
                    break;
                case "汇凯博资管荐文":
                    if (phUc.FindControl("UCRArticle") == null)
                    {
                        Control ctl = this.LoadControl("~/WebUserControls/UCRArticle.ascx");
                        ctl.ID = "UCRArticle";
                        phUc.Controls.Clear();
                        phUc.Controls.Add(ctl);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}