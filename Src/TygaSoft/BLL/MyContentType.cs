using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.SysHelper;

namespace TygaSoft.BLL
{
    public partial class ContentType
    {
        public void GetShareSiteMap(IList<ContentTypeInfo> list, object parentId, ref StringBuilder xmlAppend, string url)
        {
            var childList = list.Where(x => x.ParentId.Equals(parentId));
            if (childList != null && childList.Count() > 0)
            {
                var appName = Common.AppName;
                foreach (var model in childList)
                {
                    string currUrl = "";
                    switch (model.TypeValue)
                    {
                        case "首页":
                            currUrl = appName + "/index.html";
                            break;
                        case "客户登录":
                            currUrl = appName + "/s/y.html";
                            break;
                        case "风险测试":
                            currUrl = appName + "/s/g.html";
                            break;
                        case "网上预约":
                            currUrl = appName + "/s/a.html";
                            break;
                        default:
                            currUrl = string.Format("{0}?Id={1}", appName + url, model.Id);
                            break;
                    }
                    xmlAppend.AppendFormat(@"<Add Url=""{0}"" Title=""{1}"" Description=""{2}"" Roles=""{3}"" Id=""{4}"" ParentId=""{5}"">", currUrl, model.TypeValue, "", "", model.Id, model.ParentId);

                    if (list.Any(r => r.ParentId.Equals(model.Id)))
                    {
                        GetShareSiteMap(list, model.Id, ref xmlAppend, url);
                    }

                    xmlAppend.Append("</Add>");
                }
            }
        }

        public void UpdateHasChild(Guid Id)
        {
            dal.UpdateHasChild(Id);
        }

        public IList<ContentTypeInfo> GetChildListJoinByParentCode(string typeCode)
        {
            string sqlWhere = "and ct.ParentId = (select Id from ContentType where TypeCode = @TypeCode) ";
            SqlParameter parm = new SqlParameter("@TypeCode", SqlDbType.VarChar, 50);
            parm.Value = typeCode;

            return dal.GetListByJoin(sqlWhere, parm);
        }

        public IList<ContentTypeInfo> GetChildListJoinByParentId(Guid parentId)
        {
            string sqlWhere = "and ct.ParentId = @ParentId ";
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier);
            parm.Value = parentId;

            return dal.GetListByJoin(sqlWhere, parm);
        }

        public List<ContentTypeInfo> GetListByJoin()
        {
            return dal.GetListByJoin();
        }

        /// <summary>
        /// 获取属于当前代号下的所有子节点
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public Dictionary<object, string> GetKeyValueByParent(string typeCode)
        {
            string sqlWhere = "and t2.TypeCode = @TypeCode ";
            SqlParameter parm = new SqlParameter("@TypeCode", SqlDbType.VarChar, 50);
            parm.Value = typeCode;

            return dal.GetKeyValue(sqlWhere, parm);
        }

        /// <summary>
        /// 获取对应数据
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public ContentTypeInfo GetModelByTypeCode(string typeCode)
        {
            return dal.GetModelByTypeCode(typeCode);
        }

        /// <summary>
        /// 获取公告类别json格式字符串
        /// </summary>
        /// <returns></returns>
        public string GetTreeJsonForContentTypeByTypeCode(string typeCode)
        {
            ContentTypeInfo model = GetModelByTypeCode(typeCode);
            if (model == null) return "[]";
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier);
            parm.Value = model.Id;
            List<ContentTypeInfo> list = GetList("and ParentId = @ParentId", parm).ToList<ContentTypeInfo>();
            StringBuilder jsonAppend = new StringBuilder();

            if (list == null || list.Count == 0) return "[]";

            CreateTreeJson(list, model.Id, ref jsonAppend);

            return jsonAppend.ToString();
        }

        public string GetTreeJsonByParentCode(string typeCode)
        {
            StringBuilder jsonAppend = new StringBuilder(1000);

            var list = dal.GetListByJoin();
            var model = list.FirstOrDefault(m => m.TypeCode == typeCode);
            if (model == null) return "[]";

            jsonAppend.Append("[{\"id\":\"" + model.Id + "\",\"text\":\"" + model.TypeName + "\",\"state\":\"open\",\"attributes\":{\"parentId\":\"" + model.ParentId + "\",\"enumCode\":\"" + model.TypeCode + "\",\"enumValue\":\"" + model.TypeValue + "\",\"sort\":\"" + model.Sort + "\"}");
            jsonAppend.Append(",\"children\":");

            CreateTreeJson(list, model.Id, ref jsonAppend);

            jsonAppend.Append("}]");

            return jsonAppend.ToString();
        }

        /// <summary>
        /// 获取所有类别json格式字符串
        /// </summary>
        /// <returns></returns>
        public string GetTreeJson()
        {
            StringBuilder jsonAppend = new StringBuilder();
            List<ContentTypeInfo> list = GetListByJoin();
            if (list != null && list.Count > 0)
            {
                CreateTreeJson(list, Guid.Empty, ref jsonAppend);
            }
            else
            {
                jsonAppend.Append("[{\"id\":\"" + Guid.Empty + "\",\"text\":\"请选择\",\"state\":\"open\",\"attributes\":{\"parentId\":\"" + Guid.Empty + "\",\"parentName\":\"请选择\"}}]");
            }

            return jsonAppend.ToString();
        }

        private void CreateTreeJson(List<ContentTypeInfo> list, object parentId, ref StringBuilder jsonAppend)
        {
            jsonAppend.Append("[");
            var childList = list.FindAll(x => x.ParentId.Equals(parentId));
            if (childList.Count > 0)
            {
                int temp = 0;
                foreach (var model in childList)
                {
                    jsonAppend.Append("{\"id\":\"" + model.Id + "\",\"text\":\"" + model.TypeName + "\",\"state\":\"open\",\"attributes\":{\"parentId\":\"" + model.ParentId + "\",\"enumCode\":\"" + model.TypeCode + "\",\"enumValue\":\"" + model.TypeValue + "\",\"sort\":\"" + model.Sort + "\"}");
                    if (list.Any(r => r.ParentId.Equals(model.Id)))
                    {
                        jsonAppend.Append(",\"children\":");
                        CreateTreeJson(list, model.Id, ref jsonAppend);
                    }
                    jsonAppend.Append("}");
                    if (temp < childList.Count - 1) jsonAppend.Append(",");
                    temp++;
                }
            }
            jsonAppend.Append("]");
        }
    }
}
