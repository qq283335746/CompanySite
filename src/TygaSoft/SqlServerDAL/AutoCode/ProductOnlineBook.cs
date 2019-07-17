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
    public partial class ProductOnlineBook : IProductOnlineBook
    {
        #region IProductOnlineBook Member

        public int Insert(ProductOnlineBookInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"insert into ProductOnlineBook (CustomerName,ClientType,TelPhone,MobilePhone,Fax,Email,Address,BookProduct,Price,LastUpdatedDate)
			            values
						(@CustomerName,@ClientType,@TelPhone,@MobilePhone,@Fax,@Email,@Address,@BookProduct,@Price,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@CustomerName",SqlDbType.NVarChar,30),
                                        new SqlParameter("@ClientType",SqlDbType.NVarChar,10),
                                        new SqlParameter("@TelPhone",SqlDbType.VarChar,15),
                                        new SqlParameter("@MobilePhone",SqlDbType.VarChar,15),
                                        new SqlParameter("@Fax",SqlDbType.VarChar,10),
                                        new SqlParameter("@Email",SqlDbType.VarChar,50),
                                        new SqlParameter("@Address",SqlDbType.NVarChar,100),
                                        new SqlParameter("@BookProduct",SqlDbType.NVarChar,100),
                                        new SqlParameter("@Price",SqlDbType.Decimal),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.CustomerName;
            parms[1].Value = model.ClientType;
            parms[2].Value = model.TelPhone;
            parms[3].Value = model.MobilePhone;
            parms[4].Value = model.Fax;
            parms[5].Value = model.Email;
            parms[6].Value = model.Address;
            parms[7].Value = model.BookProduct;
            parms[8].Value = model.Price;
            parms[9].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ProductOnlineBookInfo model)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"update ProductOnlineBook set CustomerName = @CustomerName,ClientType = @ClientType,TelPhone = @TelPhone,MobilePhone = @MobilePhone,Fax = @Fax,Email = @Email,Address = @Address,BookProduct = @BookProduct,Price = @Price,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
                                        new SqlParameter("@CustomerName",SqlDbType.NVarChar,30),
                                        new SqlParameter("@ClientType",SqlDbType.NVarChar,10),
                                        new SqlParameter("@TelPhone",SqlDbType.VarChar,15),
                                        new SqlParameter("@MobilePhone",SqlDbType.VarChar,15),
                                        new SqlParameter("@Fax",SqlDbType.VarChar,10),
                                        new SqlParameter("@Email",SqlDbType.VarChar,50),
                                        new SqlParameter("@Address",SqlDbType.NVarChar,100),
                                        new SqlParameter("@BookProduct",SqlDbType.NVarChar,100),
                                        new SqlParameter("@Price",SqlDbType.Decimal),
                                        new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.CustomerName;
            parms[2].Value = model.ClientType;
            parms[3].Value = model.TelPhone;
            parms[4].Value = model.MobilePhone;
            parms[5].Value = model.Fax;
            parms[6].Value = model.Email;
            parms[7].Value = model.Address;
            parms[8].Value = model.BookProduct;
            parms[9].Value = model.Price;
            parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(object Id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ProductOnlineBook where Id = @Id");
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
                sb.Append(@"delete from ProductOnlineBook where Id = @Id" + n + " ;");
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

        public ProductOnlineBookInfo GetModel(object Id)
        {
            ProductOnlineBookInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,CustomerName,ClientType,TelPhone,MobilePhone,Fax,Email,Address,BookProduct,Price,LastUpdatedDate 
			            from ProductOnlineBook
						where Id = @Id ");
            SqlParameter parm = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(Id.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new ProductOnlineBookInfo();
                        model.Id = reader.GetGuid(0);
                        model.CustomerName = reader.GetString(1);
                        model.ClientType = reader.GetString(2);
                        model.TelPhone = reader.GetString(3);
                        model.MobilePhone = reader.GetString(4);
                        model.Fax = reader.GetString(5);
                        model.Email = reader.GetString(6);
                        model.Address = reader.GetString(7);
                        model.BookProduct = reader.GetString(8);
                        model.Price = reader.GetDecimal(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);
                    }
                }
            }

            return model;
        }

        public IList<ProductOnlineBookInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select count(*) from ProductOnlineBook ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ProductOnlineBookInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,CustomerName,ClientType,TelPhone,MobilePhone,Fax,Email,Address,BookProduct,Price,LastUpdatedDate
					  from ProductOnlineBook ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductOnlineBookInfo> list = new List<ProductOnlineBookInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductOnlineBookInfo model = new ProductOnlineBookInfo();
                        model.Id = reader.GetGuid(1);
                        model.CustomerName = reader.GetString(2);
                        model.ClientType = reader.GetString(3);
                        model.TelPhone = reader.GetString(4);
                        model.MobilePhone = reader.GetString(5);
                        model.Fax = reader.GetString(6);
                        model.Email = reader.GetString(7);
                        model.Address = reader.GetString(8);
                        model.BookProduct = reader.GetString(9);
                        model.Price = reader.GetDecimal(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductOnlineBookInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,CustomerName,ClientType,TelPhone,MobilePhone,Fax,Email,Address,BookProduct,Price,LastUpdatedDate
					   from ProductOnlineBook ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ProductOnlineBookInfo> list = new List<ProductOnlineBookInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductOnlineBookInfo model = new ProductOnlineBookInfo();
                        model.Id = reader.GetGuid(1);
                        model.CustomerName = reader.GetString(2);
                        model.ClientType = reader.GetString(3);
                        model.TelPhone = reader.GetString(4);
                        model.MobilePhone = reader.GetString(5);
                        model.Fax = reader.GetString(6);
                        model.Email = reader.GetString(7);
                        model.Address = reader.GetString(8);
                        model.BookProduct = reader.GetString(9);
                        model.Price = reader.GetDecimal(10);
                        model.LastUpdatedDate = reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductOnlineBookInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,CustomerName,ClientType,TelPhone,MobilePhone,Fax,Email,Address,BookProduct,Price,LastUpdatedDate
                        from ProductOnlineBook ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            IList<ProductOnlineBookInfo> list = new List<ProductOnlineBookInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductOnlineBookInfo model = new ProductOnlineBookInfo();
                        model.Id = reader.GetGuid(0);
                        model.CustomerName = reader.GetString(1);
                        model.ClientType = reader.GetString(2);
                        model.TelPhone = reader.GetString(3);
                        model.MobilePhone = reader.GetString(4);
                        model.Fax = reader.GetString(5);
                        model.Email = reader.GetString(6);
                        model.Address = reader.GetString(7);
                        model.BookProduct = reader.GetString(8);
                        model.Price = reader.GetDecimal(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ProductOnlineBookInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append(@"select Id,CustomerName,ClientType,TelPhone,MobilePhone,Fax,Email,Address,BookProduct,Price,LastUpdatedDate 
			            from ProductOnlineBook
					    order by LastUpdatedDate desc ");

            IList<ProductOnlineBookInfo> list = new List<ProductOnlineBookInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GzxySiteDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductOnlineBookInfo model = new ProductOnlineBookInfo();
                        model.Id = reader.GetGuid(0);
                        model.CustomerName = reader.GetString(1);
                        model.ClientType = reader.GetString(2);
                        model.TelPhone = reader.GetString(3);
                        model.MobilePhone = reader.GetString(4);
                        model.Fax = reader.GetString(5);
                        model.Email = reader.GetString(6);
                        model.Address = reader.GetString(7);
                        model.BookProduct = reader.GetString(8);
                        model.Price = reader.GetDecimal(9);
                        model.LastUpdatedDate = reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
