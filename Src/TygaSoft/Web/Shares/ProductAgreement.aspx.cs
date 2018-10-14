using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.SysHelper;
using TygaSoft.BLL;
using TygaSoft.Model;

namespace TygaSoft.Web.Shares
{
    public partial class ProductAgreement : System.Web.UI.Page
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
            var model = bll.GetModelByTypeCode(EnumData.MenuRoot.ProductAgreement.ToString());
            if (model != null)
            {
                ltrEditorContent.Text = model.ContentText;
            }

            string picUrl = PictureUrlHelper.GetUrl(model.FileDirectory, model.RandomFolder, model.FileExtension, EnumData.PictureType.OriginalPicture, EnumData.Platform.PC);

            agreementContainer.Style.Add("background-image", "url('" + picUrl + "')");
        }
    }
}