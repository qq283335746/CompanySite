using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using TygaSoft.SysHelper;
using TygaSoft.WebHelper;
using TygaSoft.BLL;

namespace TygaSoft.Web.Shares
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindCustomerType();
                BindIdType();

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

            StringBuilder sb = new StringBuilder();
            sb.Append(model.ContentText);

            string picUrl = PictureUrlHelper.GetUrl(model.FileDirectory, model.RandomFolder, model.FileExtension, EnumData.PictureType.OriginalPicture, EnumData.Platform.PC);
            var picFullPth = Server.MapPath("~" + picUrl + "");
            int width = 0;
            int height = 0;
            if (File.Exists(picFullPth))
            {
                var img = System.Drawing.Image.FromFile(picFullPth);
                width = img.Width;
                height = img.Height;
            }
            sb.AppendFormat(@"<div id=""myDataAppend"" style=""display:none;"">{{""banner"":""{0}"",""width"":""{1}"",""height"":""{2}""}}</div>", picUrl.Replace("/Files/", "/sw/Files/"), width, height);

            ltrMyData.Text = sb.ToString();
        }

        /// <summary>
        /// 绑定客户类型
        /// </summary>
        private void BindCustomerType()
        {
            Array values = Enum.GetValues(typeof(EnumData.ClientLevel));
            foreach (var item in values)
            {
                rbtnList.Items.Add(new ListItem(item.ToString(), item.ToString()));
            }
        }

        /// <summary>
        /// 绑定证件类型
        /// </summary>
        private void BindIdType()
        {
            ddlIdType.Items.Add(new ListItem("身份证", "IdNum"));
        }
    }
}