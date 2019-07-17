using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin
{
    public partial class ListProductOnlineBook : System.Web.UI.Page
    {
        StringBuilder myDataAppend = new StringBuilder(300);
        int pageIndex = WebCommon.PageIndex;
        int pageSize = WebCommon.PageSize10;
        StringBuilder queryStr;
        ParamsHelper parms;
        StringBuilder sqlWhere;
        string keyword;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                queryStr = new StringBuilder(300);
                NameValueCollection nvc = Request.QueryString;
                int index = 0;
                foreach (string item in nvc.AllKeys)
                {
                    GetParms(item, nvc);

                    if (item != "pageIndex" && item != "pageSize")
                    {
                        index++;
                        if (index > 1) queryStr.Append("&");
                        queryStr.Append(string.Format("{0}={1}", item, Server.HtmlEncode(nvc[item])));
                    }
                }

                Bind();
            }

            ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend.ToString() + "</div>";
        }

        private void Bind()
        {
            //查询条件
            GetSearchItem();

            int totalRecords = 0;
            ProductOnlineBook bll = new ProductOnlineBook();

            rpData.DataSource = bll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere == null ? null : sqlWhere.ToString(), parms == null ? null : parms.ToArray());
            rpData.DataBind();

            myDataAppend.Append("<div id=\"myDataForPage\" style=\"display:none;\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr.ToString() + "\"}]</div>");
        }

        private void GetSearchItem()
        {
            myDataAppend.Append("<div id=\"myDataForSearch\" style=\"display:none;\">{\"keyword\":\"" + keyword + "\"}</div>");

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (parms == null) parms = new ParamsHelper();

                sqlWhere.Append("and (CustomerName like @Keyword or ClientType like @Keyword or TelPhone  like @Keyword or MobilePhone like @Keyword or Fax like @Keyword  or Email like @Keyword  or Address like @Keyword  or BookProduct like @Keyword) ");
                SqlParameter parm = new SqlParameter("@Keyword", SqlDbType.NVarChar, 10);
                parm.Value = "%" + keyword + "%";
                parms.Add(parm);

            }
        }

        private void GetParms(string key, NameValueCollection nvc)
        {
            switch (key)
            {
                case "pageIndex":
                    Int32.TryParse(nvc[key], out pageIndex);
                    break;
                case "pageSize":
                    Int32.TryParse(nvc[key], out pageSize);
                    break;
                case "keyword":
                    keyword = nvc[key];
                    break;
                default:
                    break;
            }
        }
    }
}