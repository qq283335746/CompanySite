using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public partial class Announcement
    {
        #region IAnnouncement Member

        public void UpdateViewCount(Guid Id)
        {
            string cmdText = @"update Announcement set VirtualViewCount = VirtualViewCount + 1,ViewCount = ViewCount + 1 
                               where Id = @Id
                             ";
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, cmdText, parm);
        }

        public List<AnnouncementInfo> GetListForTitle(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select count(*) from Announcement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat("where 1=1 {0} ",sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Clear();
            sb.Append(@"select * from(select row_number() over(order by Sort,LastUpdatedDate desc) as RowNumber,
			          Id,Title,ContentTypeId,LastUpdatedDate,VirtualViewCount,ViewCount
					  from Announcement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat("where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(")as objTable where RowNumber between {0} and {1} ",startIndex,endIndex);

            List<AnnouncementInfo> list = new List<AnnouncementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnnouncementInfo model = new AnnouncementInfo();
                        model.Id = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.ContentTypeId = reader.GetGuid(3);
                        model.LastUpdatedDate = reader.GetDateTime(4);
                        model.VirtualViewCount = reader.GetInt32(5);
                        model.ViewCount = reader.GetInt32(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<AnnouncementInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from Announcement ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<AnnouncementInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by a.Sort,a.LastUpdatedDate desc) as RowNumber,
			          a.Id,a.ContentTypeId,a.Title,a.Descr,a.ContentText,a.VirtualViewCount,a.ViewCount,a.Sort,a.IsDisable,a.LastUpdatedDate,
                      ct.TypeValue
					  from Announcement a 
                      left join ContentType ct on ct.Id = a.ContentTypeId
                      ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<AnnouncementInfo> list = new List<AnnouncementInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AnnouncementInfo model = new AnnouncementInfo();
                        model.Id = reader.GetGuid(1);
                        model.ContentTypeId = reader.GetGuid(2);
                        model.Title = reader.GetString(3);
                        model.Descr = reader.GetString(4);
                        model.ContentText = reader.GetString(5);
                        model.VirtualViewCount = reader.GetInt32(6);
                        model.ViewCount = reader.GetInt32(7);
                        model.Sort = reader.GetInt32(8);
                        model.IsDisable = reader.GetBoolean(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);
                        model.ContentTypeName = reader.GetString(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        private bool IsExist(string name, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Name",SqlDbType.NVarChar, 100)
                                   };
            parms[0].Value = name;

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [Announcement] where lower(Title) = @Name and Id <> @Id ");

                Array.Resize(ref parms, 2);
                parms[1] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[1].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [Announcement] where lower(Title) = @Name ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }

        #endregion
    }
}
