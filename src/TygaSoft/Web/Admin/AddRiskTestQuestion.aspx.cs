using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using TygaSoft.BLL;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Admin
{
    public partial class AddRiskTestQuestion : System.Web.UI.Page
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
            try
            {
                RiskTestQuestion bll = new RiskTestQuestion();
                var list = bll.GetList(1, 1, "", null);
                if (list != null && list.Count > 0)
                {
                    var model = list[0];
                    hId.Value = model.Id.ToString();
                    XElement xel = XElement.Parse(model.QuestionXml);
                    var q = from r in xel.Descendants("Xel")
                            select new { title = r.Element("Title").Value, option = r.Element("Option").Value, answer = r.Element("Answer").Value, IsRow = r.Element("Layout").Value };
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in q)
                    {
                        string s = item.IsRow == EnumData.QuestionLayout.IsRow.ToString() ? "selected=\"selected\"" : "";

                        sb.Append("<tr>");
                        sb.AppendFormat(@"<td><textarea rows=""3"" cols=""100"" class=""txta"">{0}</textarea></td>
                                        <td><textarea rows=""3"" cols=""100"" class=""txta"">{1}</textarea></td>
                                        <td><textarea rows=""2"" cols=""80"" class=""txta"">{2}</textarea></td>
                                        <td><select name=""ddlLayout""><option value=""IsRow"" {3}>同行</option><option value=""NotRow"" {3}>换行</option></select></td>
                                       ", item.title, item.option.Replace("#", "  "), item.answer, s);
                        sb.Append(@"<td><a class=""easyui-linkbutton"" data-options=""iconCls:'icon-undo',plain:true"" onclick=""AddRiskTestQuestion.UpInput(this)"">上移</a>
                                 <a class=""easyui-linkbutton"" data-options=""iconCls:'icon-redo',plain:true"" onclick=""AddRiskTestQuestion.DownInput(this)"">下移</a>
                                 <a class=""easyui-linkbutton"" data-options=""iconCls:'icon-remove',plain:true"" onclick=""AddRiskTestQuestion.DelInput(this)"">移除</a>
                                 </td>");
                        sb.Append("</tr>");
                    }

                    ltrTr.Text = sb.ToString();
                }
                else
                {
                    var emptyTr = @"<tr><td><textarea rows=""2"" cols=""80"" class=""txta""></textarea></td>
                   <td><textarea rows=""3"" cols=""100"" class=""txta""></textarea></td>
                   <td><textarea rows=""2"" cols=""80"" class=""txta""></textarea></td>
                   <td><select name=""ddlLayout""><option value=""IsRow""selected=""selected"">同行</option><option value=""NotRow"">换行</option></select></td>
                   <td>
                       <a class=""easyui-linkbutton"" data-options=""iconCls:'icon-undo',plain:true"" onclick=""AddRiskTestQuestion.UpInput(this)"">上移</a>
                       <a class=""easyui-linkbutton"" data-options=""iconCls:'icon-redo',plain:true"" onclick=""AddRiskTestQuestion.DownInput(this)"">下移</a>
                       <a class=""easyui-linkbutton"" data-options=""iconCls:'icon-remove',plain:true"" onclick=""AddRiskTestQuestion.DelInput(this)"">移除</a>
                   </td>
                   </tr>";
                    ltrTr.Text = emptyTr;
                }
            }
            catch
            {
                var emptyTr = @"<tr><td><textarea rows=""2"" cols=""80"" class=""txta""></textarea></td>
                   <td><textarea rows=""3"" cols=""100"" class=""txta""></textarea></td>
                   <td><textarea rows=""2"" cols=""80"" class=""txta""></textarea></td>
                   <td><select name=""ddlLayout""><option value=""IsRow""selected=""selected"">同行</option><option value=""NotRow"">换行</option></select></td>
                   <td>
                       <a class=""easyui-linkbutton"" data-options=""iconCls:'icon-undo',plain:true"" onclick=""AddRiskTestQuestion.UpInput(this)"">上移</a>
                       <a class=""easyui-linkbutton"" data-options=""iconCls:'icon-redo',plain:true"" onclick=""AddRiskTestQuestion.DownInput(this)"">下移</a>
                       <a class=""easyui-linkbutton"" data-options=""iconCls:'icon-remove',plain:true"" onclick=""AddRiskTestQuestion.DelInput(this)"">移除</a>
                   </td>
                   </tr>";
                ltrTr.Text = emptyTr;
            }
        }
    }
}