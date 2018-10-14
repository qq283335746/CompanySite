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
using TygaSoft.WebHelper;
using TygaSoft.DBUtility;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Admin
{
    public partial class ListRiskTestQuestionAnswer : System.Web.UI.Page
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
            RiskTestQuestionAnswer bll = new RiskTestQuestionAnswer();

            rpData.DataSource = bll.GetList(pageIndex, pageSize, out totalRecords, sqlWhere == null ? null : sqlWhere.ToString(), parms == null ? null : parms.ToArray());
            rpData.DataBind();

            myDataAppend.Append("<div id=\"myDataForPage\" style=\"display:none;\">[{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr.ToString() + "\"}]</div>");
        }

        private void GetSearchItem()
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["startDate"]))
            {
                DateTime.TryParse(Request.QueryString["startDate"], out startDate);
            }
            if (!string.IsNullOrWhiteSpace(Request.QueryString["endDate"]))
            {
                DateTime.TryParse(Request.QueryString["endDate"], out endDate);
            }
            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                if (parms == null) parms = new ParamsHelper();
                sqlWhere.Append("and (LastUpdatedDate between @StartDate and @EndDate) ");
                SqlParameter parm = new SqlParameter("@StartDate", SqlDbType.DateTime);
                parm.Value = startDate;
                parms.Add(parm);
                parm = new SqlParameter("@EndDate", SqlDbType.DateTime);
                parm.Value = DateTime.Parse(string.Format("{0} 23:59:59", endDate.ToString("yyyy-MM-dd")));
                parms.Add(parm);
            }
            else
            {
                if (startDate != DateTime.MinValue)
                {
                    sqlWhere.Append("and (LastUpdatedDate >= @StartDate) ");
                    SqlParameter parm = new SqlParameter("@StartDate", SqlDbType.DateTime);
                    parm.Value = startDate;
                    parms.Add(parm);
                }
                else if (endDate != DateTime.MinValue)
                {
                    sqlWhere.Append("and (LastUpdatedDate <= @EndDate) ");
                    SqlParameter parm = new SqlParameter("@EndDate", SqlDbType.DateTime);
                    parm.Value = DateTime.Parse(string.Format("{0} 23:59:59", endDate.ToString("yyyy-MM-dd")));
                    parms.Add(parm);
                }
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