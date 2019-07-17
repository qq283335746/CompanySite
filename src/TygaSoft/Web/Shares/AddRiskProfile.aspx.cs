using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
    public partial class AddRiskProfile : System.Web.UI.Page
    {
        StringBuilder myDataAppend; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindExam();

                Bind();

                ltrMyData.Text = myDataAppend.ToString();
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

            if (myDataAppend == null) myDataAppend = new StringBuilder();

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

            myDataAppend.Append(sb.ToString());
        }

        /// <summary>
        /// 绑定风险测试题
        /// </summary>
        private void BindExam()
        {
            if (myDataAppend == null) myDataAppend = new StringBuilder();

            RiskTestQuestion bll = new RiskTestQuestion();
            var list = bll.GetList(1, 1, "", null);
            if (list != null && list.Count > 0)
            {
                var model = list[0];
                hId.Value = model.Id.ToString();

                XElement xel = XElement.Parse(model.QuestionXml);
                var q = from r in xel.Descendants("Xel")
                        select new { title = r.Element("Title").Value, option = r.Element("Option").Value, answer = r.Element("Answer").Value, IsRow = r.Element("Layout").Value == EnumData.QuestionLayout.IsRow.ToString() };
                StringBuilder sb = new StringBuilder();
                int index = 0;
                foreach (var item in q)
                {
                    StringBuilder currSb = new StringBuilder(500);
                    var optionArr = item.option.Split('#');
                    int currIndex = 0;

                    if (item.IsRow) currSb.Append(@"<ul class=""ul_h"">");
                    else currSb.Append(@"<ul class=""ul_v"">");

                    foreach(var currItem in optionArr)
                    {
                        if (item.IsRow)
                        {
                            currSb.AppendFormat(@"<li><input type=""radio"" id=""rbtn{0}"" name=""rbtn{3}"" value=""{1}"" /><label for=""rbtn{0}"">{2}</label></li>", string.Format("{0}{1}", index, currIndex), currItem, currItem, index);
                        }
                        else
                        {
                            currSb.AppendFormat(@"<li><input type=""radio"" id=""rbtn{0}"" name=""rbtn{3}"" value=""{1}"" /><label for=""rbtn{0}"">{2}</label></li>", string.Format("{0}{1}", index, currIndex), currItem, currItem, index);
                        }

                        currIndex++;
                    }

                    if (item.IsRow) currSb.Append(@"</ul><span class=""clr""></span>");
                    else currSb.Append(@"</ul>");

                    sb.AppendFormat(@"<div class=""row""><span class=""title"">{0}</span><div class=""option"">{1}</div></div>", item.title, currSb.ToString());

                    index++;
                }
                sb.Append(@"<div class=""mt30""> <a class=""easyui-linkbutton"" data-options=""width:77"" onclick=""AddRiskProfile.OnCommit()"">提交</a></div>");

                myDataAppend.Append(@"<div id=""questionList"">" + sb.ToString() + "</div>");
            }
        }
    }
}