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
    public partial class RiskTestQuestion : IRiskTestQuestion
    {
        #region IRiskTestQuestion Member

        public int Insert(RiskTestQuestionInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into RiskTestQuestion (Id,QuestionXml,LastUpdatedDate)
			            values
						(@Id, @QuestionXml,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@QuestionXml",SqlDbType.NVarChar,3000),
                                       new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.QuestionXml;
            parms[2].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(RiskTestQuestionInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update RiskTestQuestion set QuestionXml = @QuestionXml,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionXml",SqlDbType.NVarChar,3000),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.QuestionXml;
            parms[2].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from RiskTestQuestion where Id = @Id");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parm);
        }

        public bool DeleteBatch(IList<object> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from RiskTestQuestion where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.GzxySiteDbConnString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int effect = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null);
                        tran.Commit();
                        if (effect > 0) result = true;
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            return result;
        }

        public RiskTestQuestionInfo GetModel(object Id)
        {
            RiskTestQuestionInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,QuestionXml,LastUpdatedDate 
			            from RiskTestQuestion
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new RiskTestQuestionInfo();
                        model.Id = reader.GetGuid(0);
                        model.QuestionXml = reader.GetString(1);
                        model.LastUpdatedDate = reader.GetDateTime(2);
                    }
                }
            }

            return model;
        }

        public IList<RiskTestQuestionInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from RiskTestQuestion ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<RiskTestQuestionInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,QuestionXml,LastUpdatedDate
					  from RiskTestQuestion ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<RiskTestQuestionInfo> list = new List<RiskTestQuestionInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RiskTestQuestionInfo model = new RiskTestQuestionInfo();
                        model.Id = reader.GetGuid(1);
                        model.QuestionXml = reader.GetString(2);
                        model.LastUpdatedDate = reader.GetDateTime(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<RiskTestQuestionInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,QuestionXml,LastUpdatedDate
					   from RiskTestQuestion ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<RiskTestQuestionInfo> list = new List<RiskTestQuestionInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RiskTestQuestionInfo model = new RiskTestQuestionInfo();
                        model.Id = reader.GetGuid(1);
                        model.QuestionXml = reader.GetString(2);
                        model.LastUpdatedDate = reader.GetDateTime(3);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<RiskTestQuestionInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,QuestionXml,LastUpdatedDate
                        from RiskTestQuestion ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<RiskTestQuestionInfo> list = new List<RiskTestQuestionInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RiskTestQuestionInfo model = new RiskTestQuestionInfo();
                        model.Id = reader.GetGuid(0);
                        model.QuestionXml = reader.GetString(1);
                        model.LastUpdatedDate = reader.GetDateTime(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<RiskTestQuestionInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,QuestionXml,LastUpdatedDate 
			            from RiskTestQuestion
					    order by LastUpdatedDate desc ");

            IList<RiskTestQuestionInfo> list = new List<RiskTestQuestionInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RiskTestQuestionInfo model = new RiskTestQuestionInfo();
                        model.Id = reader.GetGuid(0);
                        model.QuestionXml = reader.GetString(1);
                        model.LastUpdatedDate = reader.GetDateTime(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
