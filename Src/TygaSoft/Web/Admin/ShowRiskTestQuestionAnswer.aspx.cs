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

namespace TygaSoft.Web.Admin
{
    public partial class ShowRiskTestQuestionAnswer : System.Web.UI.Page
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
            if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                Guid Id = Guid.Empty;
                if (!Guid.TryParse(Request.QueryString["Id"], out Id))
                {
                    MessageBox.Messager(this.Page, "请求参数不合法，请正确操作","错误提示","error");
                    return;
                }
                if (Id.Equals(Guid.Empty))
                {
                    MessageBox.Messager(this.Page, "请求参数不合法，请正确操作", "错误提示", "error");
                    return;
                }

                RiskTestQuestionAnswer bll = new RiskTestQuestionAnswer();
                var model = bll.GetModel(Id);
                if (model == null)
                {
                    MessageBox.Messager(this.Page, "数据不存在或已被删除", "错误提示", "error");
                    return;
                }
                hAnswerQuestionId.Value = model.QuestionId.ToString();

                myDataAppend.Append(@"<div id=""xmlAnswer"" style=""display:none;"">" + model.AnswerResult + "</div>");
            }
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
                hQuestionId.Value = model.Id.ToString();

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

                    foreach (var currItem in optionArr)
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
                //sb.Append(@"<div class=""mt30""> <a class=""easyui-linkbutton"" data-options=""width:77"" onclick=""AddRiskProfile.OnCommit()"">提交</a></div>");

                myDataAppend.Append(@"<div id=""questionList"">" + sb.ToString() + "</div>");
            }
        }
    }
}