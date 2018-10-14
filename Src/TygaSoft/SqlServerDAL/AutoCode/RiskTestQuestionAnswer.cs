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
    public partial class RiskTestQuestionAnswer : IRiskTestQuestionAnswer
    {
        #region IRiskTestQuestionAnswer Member

        public int Insert(RiskTestQuestionAnswerInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into RiskTestQuestionAnswer (UserId,QuestionId,AnswerResult,LastUpdatedDate)
			            values
						(@UserId,@QuestionId,@AnswerResult,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionId",SqlDbType.UniqueIdentifier),
new SqlParameter("@AnswerResult",SqlDbType.NVarChar,2000),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.UserId;
            parms[1].Value = model.QuestionId;
            parms[2].Value = model.AnswerResult;
            parms[3].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(RiskTestQuestionAnswerInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update RiskTestQuestionAnswer set UserId = @UserId,QuestionId = @QuestionId,AnswerResult = @AnswerResult,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@QuestionId",SqlDbType.UniqueIdentifier),
new SqlParameter("@AnswerResult",SqlDbType.NVarChar,2000),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.UserId;
            parms[2].Value = model.QuestionId;
            parms[3].Value = model.AnswerResult;
            parms[4].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from RiskTestQuestionAnswer where Id = @Id");
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
                sb.Append(@"delete from RiskTestQuestionAnswer where Id = @Id" + n + " ;");
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

        public RiskTestQuestionAnswerInfo GetModel(object Id)
        {
            RiskTestQuestionAnswerInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,UserId,QuestionId,AnswerResult,LastUpdatedDate 
			            from RiskTestQuestionAnswer
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new RiskTestQuestionAnswerInfo();
                        model.Id = reader.GetGuid(0);
                        model.UserId = reader.GetGuid(1);
                        model.QuestionId = reader.GetGuid(2);
                        model.AnswerResult = reader.GetString(3);
                        model.LastUpdatedDate = reader.GetDateTime(4);
                    }
                }
            }

            return model;
        }

        public IList<RiskTestQuestionAnswerInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from RiskTestQuestionAnswer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<RiskTestQuestionAnswerInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,UserId,QuestionId,AnswerResult,LastUpdatedDate
					  from RiskTestQuestionAnswer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<RiskTestQuestionAnswerInfo> list = new List<RiskTestQuestionAnswerInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RiskTestQuestionAnswerInfo model = new RiskTestQuestionAnswerInfo();
                        model.Id = reader.GetGuid(1);
                        model.UserId = reader.GetGuid(2);
                        model.QuestionId = reader.GetGuid(3);
                        model.AnswerResult = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<RiskTestQuestionAnswerInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,UserId,QuestionId,AnswerResult,LastUpdatedDate
					   from RiskTestQuestionAnswer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<RiskTestQuestionAnswerInfo> list = new List<RiskTestQuestionAnswerInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RiskTestQuestionAnswerInfo model = new RiskTestQuestionAnswerInfo();
                        model.Id = reader.GetGuid(1);
                        model.UserId = reader.GetGuid(2);
                        model.QuestionId = reader.GetGuid(3);
                        model.AnswerResult = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<RiskTestQuestionAnswerInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,UserId,QuestionId,AnswerResult,LastUpdatedDate
                        from RiskTestQuestionAnswer ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<RiskTestQuestionAnswerInfo> list = new List<RiskTestQuestionAnswerInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RiskTestQuestionAnswerInfo model = new RiskTestQuestionAnswerInfo();
                        model.Id = reader.GetGuid(0);
                        model.UserId = reader.GetGuid(1);
                        model.QuestionId = reader.GetGuid(2);
                        model.AnswerResult = reader.GetString(3);
                        model.LastUpdatedDate = reader.GetDateTime(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<RiskTestQuestionAnswerInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,UserId,QuestionId,AnswerResult,LastUpdatedDate 
			            from RiskTestQuestionAnswer
					    order by LastUpdatedDate desc ");

            IList<RiskTestQuestionAnswerInfo> list = new List<RiskTestQuestionAnswerInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RiskTestQuestionAnswerInfo model = new RiskTestQuestionAnswerInfo();
                        model.Id = reader.GetGuid(0);
                        model.UserId = reader.GetGuid(1);
                        model.QuestionId = reader.GetGuid(2);
                        model.AnswerResult = reader.GetString(3);
                        model.LastUpdatedDate = reader.GetDateTime(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
