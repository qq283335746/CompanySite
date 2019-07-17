using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class Announcement
    {
        #region Announcement Member

        public IList<AnnouncementInfo> GetListByTypeId(object typeId, int pageIndex, int pageSize)
        {
            string sqlWhere = "and ContentTypeId = @ContentTypeId ";
            SqlParameter parm = new SqlParameter("@ContentTypeId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(typeId.ToString());

            return dal.GetList(pageIndex, pageSize, sqlWhere, parm);
        }

        public IList<AnnouncementInfo> GetListByTypeId(object typeId, int pageIndex, int pageSize, out int totalRecords)
        {
            string sqlWhere = "and ContentTypeId = @ContentTypeId ";
            SqlParameter parm = new SqlParameter("@ContentTypeId", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(typeId.ToString());

            return dal.GetList(pageIndex, pageSize,out totalRecords, sqlWhere, parm);
        }

        public IList<AnnouncementInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }

        #endregion
    }
}
