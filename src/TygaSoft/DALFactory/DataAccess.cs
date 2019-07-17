using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using TygaSoft.IDAL;

namespace TygaSoft.DALFactory
{
    public sealed class DataAccess
    {
        private static readonly string[] paths = ConfigurationManager.AppSettings["WebDAL"].Split(',');

        #region GzxySiteDb

        public static IProductOnlineBook CreateProductOnlineBook()
        {
            string className = paths[0] + ".ProductOnlineBook";
            return (IProductOnlineBook)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IRiskTestQuestion CreateRiskTestQuestion()
        {
            string className = paths[0] + ".RiskTestQuestion";
            return (IRiskTestQuestion)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IRiskTestQuestionAnswer CreateRiskTestQuestionAnswer()
        {
            string className = paths[0] + ".RiskTestQuestionAnswer";
            return (IRiskTestQuestionAnswer)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IPictureContent CreatePictureContent()
        {
            string className = paths[0] + ".PictureContent";
            return (IPictureContent)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static INotice CreateNotice()
        {
            string className = paths[0] + ".Notice";
            return (INotice)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IAnnouncement CreateAnnouncement()
        {
            string className = paths[0] + ".Announcement";
            return (IAnnouncement)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IContentType CreateContentType()
        {
            string className = paths[0] + ".ContentType";
            return (IContentType)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IContentDetail CreateContentDetail()
        {
            string className = paths[0] + ".ContentDetail";
            return (IContentDetail)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IProvinceCity CreateProvinceCity()
        {
            string className = paths[0] + ".ProvinceCity";
            return (IProvinceCity)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ISysEnum CreateSysEnum()
        {
            string className = paths[0] + ".SysEnum";
            return (ISysEnum)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        public static IRole CreateRole()
        {
            string className = paths[0] + ".Role";
            return (IRole)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #region ϵͳ
        
        public static ISysLog CreateSysLog()
        {
            string className = paths[0] + ".SysLog";
            return (ISysLog)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        #endregion

    }
}
