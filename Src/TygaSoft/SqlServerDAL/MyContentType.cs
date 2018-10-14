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
    public partial class ContentType
    {
        public void UpdateHasChild(Guid Id)
        {
            string cmdText = @"update ContentType set HasChild = 
                            (
                            select (case when COUNT(1) > 0 then 1 else 0 end) isHasChild from dbo.ContentType 
                            where ParentId = @Id
                            )
                            where Id = @Id ";

            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Id;

            SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, cmdText, parm);
        }

        public List<ContentTypeInfo> GetListByJoin(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ct.Id,ct.TypeName,ct.TypeCode,ct.TypeValue,ct.ParentId,ct.Sort,ct.PictureId,ct.IsSys,ct.LastUpdatedDate,
                        pc.FileExtension,pc.FileDirectory,pc.RandomFolder
                        from ContentType ct
                        left join Picture_Content pc on pc.Id = ct.PictureId
                        ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.Append("order by ct.Sort, ct.LastUpdatedDate desc ");

            List<ContentTypeInfo> list = new List<ContentTypeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentTypeInfo model = new ContentTypeInfo();
                        model.Id = reader.GetGuid(0);
                        model.TypeName = reader.GetString(1);
                        model.TypeCode = reader.GetString(2);
                        model.TypeValue = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.PictureId = reader.IsDBNull(6) ? Guid.Empty : reader.GetGuid(6);
                        model.IsSys = reader.GetBoolean(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);
                        model.FileExtension = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.FileDirectory = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        model.RandomFolder = reader.IsDBNull(11) ? "" : reader.GetString(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<ContentTypeInfo> GetListByJoin()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select ct.Id,ct.TypeName,ct.TypeCode,ct.TypeValue,ct.ParentId,ct.Sort,ct.PictureId,ct.IsSys,ct.LastUpdatedDate,
                        pc.FileExtension,pc.FileDirectory,pc.RandomFolder
			            from ContentType ct
                        left join Picture_Content pc on pc.Id = ct.PictureId
					    order by ct.Sort, ct.LastUpdatedDate desc ");

            List<ContentTypeInfo> list = new List<ContentTypeInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentTypeInfo model = new ContentTypeInfo();
                        model.Id = reader.GetGuid(0);
                        model.TypeName = reader.GetString(1);
                        model.TypeCode = reader.GetString(2);
                        model.TypeValue = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.PictureId = reader.IsDBNull(6) ? Guid.Empty : reader.GetGuid(6);
                        model.IsSys = reader.GetBoolean(7);
                        model.LastUpdatedDate = reader.GetDateTime(8);
                        model.FileExtension = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        model.FileDirectory = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        model.RandomFolder = reader.IsDBNull(11) ? "" : reader.GetString(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public Dictionary<object, string> GetKeyValue(string sqlWhere, params SqlParameter[] cmdParms)
        {
            Dictionary<object, string> dic = new Dictionary<object, string>();

            string cmdText = @"select t1.Id,t1.TypeValue 
                                from ContentType t1
                                join ContentType t2 on t2.Id = t1.ParentId ";

            if (!string.IsNullOrEmpty(sqlWhere))
            {
                cmdText += " where 1=1 " + sqlWhere;
            }

            cmdText += " order by t1.Sort,t1.LastUpdatedDate desc ";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, cmdText, cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dic.Add(reader[0], reader.GetString(1));

                    }
                }
            }

            return dic;
        }

        public ContentTypeInfo GetModelByTypeCode(string typeCode)
        {
            ContentTypeInfo model = null;

            string cmdText = @"select top 1 Id,TypeName,TypeCode,TypeValue,ParentId,Sort,IsSys,LastUpdatedDate 
			                   from ContentType
							   where TypeCode = @TypeCode ";
            SqlParameter parm = new SqlParameter("@TypeCode", SqlDbType.VarChar,50);
            parm.Value = typeCode;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ContentTypeInfo();
                        model.Id = reader.GetGuid(0);
                        model.TypeName = reader.GetString(1);
                        model.TypeCode = reader.GetString(2);
                        model.TypeValue = reader.GetString(3);
                        model.ParentId = reader.GetGuid(4);
                        model.Sort = reader.GetInt32(5);
                        model.IsSys = reader.GetBoolean(6);
                        model.LastUpdatedDate = reader.GetDateTime(7);
                    }
                }
            }

            return model;
        }

        private bool IsExist(string name, object parentId, object Id)
        {
            Guid gId = Guid.Empty;
            if (Id != null)
            {
                Guid.TryParse(Id.ToString(), out gId);
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@Name",SqlDbType.NVarChar, 50),
                                       new SqlParameter("@ParentId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = name;
            parms[1].Value = Guid.Parse(parentId.ToString());

            StringBuilder sb = new StringBuilder(100);
            if (!gId.Equals(Guid.Empty))
            {
                sb.Append(@" select 1 from [ContentType] where lower(TypeCode) = @Name and ParentId = @ParentId and Id <> @Id ");

                Array.Resize(ref parms, 3);
                parms[2] = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
                parms[2].Value = gId;
            }
            else
            {
                sb.Append(@" select 1 from [ContentType] where lower(TypeCode) = @Name and ParentId = @ParentId ");
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parms);
            if (obj != null) return true;

            return false;
        }
    }
}
