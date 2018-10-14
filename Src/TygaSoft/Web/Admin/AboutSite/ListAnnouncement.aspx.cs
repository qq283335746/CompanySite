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

namespace TygaSoft.Web.Admin.AboutSite
{
    public partial class ListAnnouncement : System.Web.UI.Page
    {
        int pageIndex = WebCommon.PageIndex;
        int pageSize = WebCommon.PageSize10;
        StringBuilder myDataAppend = new StringBuilder(1000);
        StringBuilder queryStr = new StringBuilder(300);
        StringBuilder sqlWhere = new StringBuilder(300);
        ParamsHelper parms;
        string title;
        Guid parentTypeId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                NameValueCollection nvc = Request.QueryString;
                int index = 0;
                foreach (string item in nvc.AllKeys)
                {
                    GetParms(item, nvc);

                    if (item != "pageIndex" && item != "pageSize")
                    {
                        index++;
                        if (index > 1) queryStr.Append("&");
                        queryStr.AppendFormat("{0}={1}", item, Server.HtmlEncode(nvc[item]));
                    }
                }

                Bind();
            }

            ltrMyData.Text = "<div id=\"myDataAppend\" style=\"display:none;\">" + myDataAppend + "</div>";
        }

        private void Bind()
        {
            //查询条件
            GetSearchItem();

            int totalRecords = 0;
            Announcement bll = new Announcement();

            rpData.DataSource = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere.ToString(), parms == null ? null : parms.ToArray());
            rpData.DataBind();

            myDataAppend.Append("<div id=\"myDataForPage\" style=\"display:none;\">{\"PageIndex\":\"" + pageIndex + "\",\"PageSize\":\"" + pageSize + "\",\"TotalRecord\":\"" + totalRecords + "\",\"QueryStr\":\"" + queryStr + "\"}</div>");
        }

        private void GetSearchItem()
        {
            if (!string.IsNullOrEmpty(title))
            {
                if (parms == null) parms = new ParamsHelper();

                sqlWhere.Append("and Title like @Title ");
                SqlParameter parm = new SqlParameter("@Title", SqlDbType.NVarChar, 50);
                parm.Value = "%" + title + "%";

                parms.Add(parm);
            }
            if (!parentTypeId.Equals(Guid.Empty))
            {
                if (parms == null) parms = new ParamsHelper();

                sqlWhere.Append("and ct.Id = @ParentTypeId ");
                SqlParameter parm = new SqlParameter("@ParentTypeId", SqlDbType.UniqueIdentifier);
                parm.Value = parentTypeId;

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
                    title = nvc[key];
                    txtKeyword.Value = title;
                    break;
                case "parentType":
                    Guid.TryParse(nvc[key],out parentTypeId);
                    cbtParentType.Value = nvc[key];
                    break;
                default:
                    break;
            }
        }
    }
}