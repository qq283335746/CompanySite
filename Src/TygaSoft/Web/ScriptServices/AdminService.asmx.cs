using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.DBUtility;
using TygaSoft.WebHelper;
using TygaSoft.CustomProvider;
using TygaSoft.CustomExceptions;

namespace TygaSoft.Web.ScriptServices
{
    /// <summary>
    /// AdminService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class AdminService : System.Web.Services.WebService
    {

        #region 菜单导航

        [WebMethod]
        public string GetTreeJsonForMenu()
        {
            string[] roles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
            var roleList = roles.ToList();
            //if (roleList.Contains("Administrators"))
            //{
            //    roleList.Remove("Administrators");
            //}
            //roleList.Add("Users");
            SitemapHelper.Roles = roleList;
            return SitemapHelper.GetTreeJsonForMenu();
        }

        #endregion

        #region 系统日志

        [WebMethod]
        public string DelSyslog(string itemAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MC.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                SysLog bll = new SysLog();
                bll.DeleteBatch(items.ToList<object>());

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg);
        }

        #endregion

        #region 用户角色

        [WebMethod]
        public string SaveRole(RoleInfo model)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MC.Role_InvalidError;
            }

            model.RoleName = model.RoleName.Trim();
            if (string.IsNullOrEmpty(model.RoleName))
            {
                return MC.Submit_Params_InvalidError;
            }

            if (Roles.RoleExists(model.RoleName))
            {
                return MC.Submit_Exist;
            }

            Guid gId = Guid.Empty;
            if (model.RoleId != null)
            {
                Guid.TryParse(model.RoleId.ToString(), out gId);
            }

            try
            {

                Role bll = new Role();

                if (!gId.Equals(Guid.Empty))
                {
                    bll.Update(model);
                }
                else
                {
                    Roles.CreateRole(model.RoleName);
                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string DelRole(string itemAppend)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MC.Role_InvalidError;
            }

            itemAppend = itemAppend.Trim();
            if (string.IsNullOrEmpty(itemAppend))
            {
                return MC.Submit_InvalidRow;
            }
            try
            {
                string[] roleIds = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in roleIds)
                {
                    Roles.DeleteRole(item);
                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string SaveIsLockedOut(string userName)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MC.Role_InvalidError;
            }

            try
            {
                MembershipUser user = Membership.GetUser(userName);
                if (user == null)
                {
                    return "当前用户不存在，请检查";
                }
                if (user.IsLockedOut)
                {
                    if (user.UnlockUser())
                    {
                        return "0";
                    }
                    else
                    {
                        return "操作失败，请联系管理员";
                    }
                }

                return "只有“已锁定”的用户才能执行此操作";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string SaveIsApproved(string userName)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MC.Role_InvalidError;
            }

            try
            {
                MembershipUser user = Membership.GetUser(userName);
                if (user == null)
                {
                    return "当前用户不存在，请检查";
                }
                if (user.IsApproved)
                {
                    user.IsApproved = false;
                }
                else
                {
                    user.IsApproved = true;
                }

                Membership.UpdateUser(user);

                return user.IsApproved ? "1" : "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string SaveUserInRole(string userName, string roleName, bool isInRole)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MC.Role_InvalidError;
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                return MC.GetString(MC.Request_InvalidArgument, "用户名");
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return MC.GetString(MC.Request_InvalidArgument, "角色");
            }
            try
            {
                if (isInRole)
                {
                    if (!Roles.IsUserInRole(userName, roleName))
                    {
                        Roles.AddUserToRole(userName, roleName);
                    }
                }
                else
                {
                    if (Roles.IsUserInRole(userName, roleName))
                    {
                        Roles.RemoveUserFromRole(userName, roleName);
                    }
                }
                return "1";
            }
            catch (System.Configuration.Provider.ProviderException pex)
            {
                return pex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string GetUserInRole(string userName)
        {
            try
            {
                string[] roles = Roles.GetRolesForUser(userName);
                if (roles.Length == 0) return "";

                return string.Join(",", roles);
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }

        [WebMethod]
        public string DelUser(string userName)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MC.Role_InvalidError;
            }

            try
            {
                Membership.DeleteUser(userName);
                return "1";
            }
            catch (Exception ex)
            {
                return "" + MC.AlertTitle_Ex_Error + "：" + ex.Message;
            }
        }

        [WebMethod]
        public string SaveUser(UserInfo model)
        {
            if (!HttpContext.Current.User.IsInRole("Administrators"))
            {
                return MC.Role_InvalidError;
            }

            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                return MC.Submit_Params_InvalidError;
            }
            if (model.Password != model.CfmPsw)
            {
                return MC.Request_InvalidCompareToPassword;
            }
            model.UserName = model.UserName.Trim();
            model.Password = model.Password.Trim();
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                model.Email = model.UserName + "tygaweb.com";
            }

            string errMsg = "";

            try
            {
                model.RoleName = model.RoleName.Trim().Trim(',');
                string[] roles = null;
                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    roles = model.RoleName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }

                MembershipCreateStatus status;
                MembershipUser user;

                using (TransactionScope scope = new TransactionScope())
                {
                    user = Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, model.IsApproved, out status);
                    if (roles != null && roles.Length > 0)
                    {
                        Roles.AddUserToRoles(model.UserName, roles);
                    }

                    scope.Complete();
                }

                if (user == null)
                {
                    return EnumMembershipCreateStatus.GetStatusMessage(status);
                }

                errMsg = "1";
            }
            catch (MembershipCreateUserException ex)
            {
                errMsg = EnumMembershipCreateStatus.GetStatusMessage(ex.StatusCode);
            }
            catch (HttpException ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        #endregion

        #region 数据字典

        [WebMethod]
        public string GetJsonForSysEnum()
        {
            SysEnum bll = new SysEnum();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string GetJsonForDicCode()
        {
            try
            {
                return SysEnumDataProxy.GetJsonForEnumCode("DicCode");
            }
            catch (Exception ex)
            {
                return "异常：" + ex.Message;
            }
        }

        [WebMethod]
        public string SaveSysEnum(SysEnumInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.EnumName))
            {
                return MC.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(model.EnumCode))
            {
                return MC.Submit_Params_InvalidError;
            }
            if (string.IsNullOrWhiteSpace(model.EnumValue))
            {
                return MC.Submit_Params_InvalidError;
            }

            Guid gId = Guid.Empty;
            Guid.TryParse(model.Id.ToString(), out gId);
            model.Id = gId;

            Guid parentId = Guid.Empty;
            Guid.TryParse(model.ParentId.ToString(), out parentId);
            model.ParentId = parentId;

            SysEnum bll = new SysEnum();
            int effect = -1;

            if (!gId.Equals(Guid.Empty))
            {
                effect = bll.Update(model);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Suppress))
                    {
                        if (bll.IsExist(model.EnumCode, model.ParentId, model.Id))
                        {
                            effect = 110;
                        }
                        else
                        {
                            effect = bll.Insert(model);
                        }
                        scope2.Complete();
                    }

                    scope.Complete();
                }
            }
            if (effect == 110)
            {
                return MC.Submit_Exist;
            }
            if (effect > 0)
            {
                return "1";
            }
            else return MC.Submit_Error;
        }

        [WebMethod]
        public string DelSysEnum(string id)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                if (string.IsNullOrWhiteSpace(id))
                {
                    return MC.Submit_InvalidRow;
                }
                Guid gId = Guid.Empty;
                Guid.TryParse(id, out gId);
                if (gId.Equals(Guid.Empty))
                {
                    return MC.GetString(MC.Submit_Params_GetInvalidRegex, "对应标识值");
                }

                SysEnum bll = new SysEnum();

                bll.Delete(gId);

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg);  
        }

        #endregion

        #region 省市区

        [WebMethod]
        public string GetJsonForProvinceCity()
        {
            ProvinceCity bll = new ProvinceCity();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string SaveProvinceCity(ProvinceCityInfo model)
        {
            string errorMsg = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(model.Named))
                {
                    return MC.Submit_Params_InvalidError;
                }
                if (string.IsNullOrWhiteSpace(model.Pinyin))
                {
                    return MC.Submit_Params_InvalidError;
                }
                if (string.IsNullOrWhiteSpace(model.FirstChar))
                {
                    return MC.Submit_Params_InvalidError;
                }

                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                Guid parentId = Guid.Empty;
                Guid.TryParse(model.ParentId.ToString(), out parentId);
                model.ParentId = parentId;
                model.LastUpdatedDate = DateTime.Now;

                ProvinceCity bll = new ProvinceCity();
                int effect = -1;

                using (TransactionScope scope = new TransactionScope())
                {
                    if (!gId.Equals(Guid.Empty))
                    {
                        effect = bll.Update(model);
                    }
                    else
                    {
                        effect = bll.Insert(model);
                    }

                    scope.Complete();
                }

                if (effect == 110)
                {
                    return MC.Submit_Exist;
                }
                if (effect > 0)
                {
                    return "1";
                }
                else return MC.Submit_Error;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg); 
        }

        [WebMethod]
        public string DelProvinceCity(string id)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                if (string.IsNullOrWhiteSpace(id))
                {
                    return MC.Submit_InvalidRow;
                }
                Guid gId = Guid.Empty;
                Guid.TryParse(id, out gId);
                if (gId.Equals(Guid.Empty))
                {
                    return MC.GetString(MC.Submit_Params_GetInvalidRegex, "对应标识值");
                }

                ProvinceCity bll = new ProvinceCity();
                bll.Delete(gId);

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg); 
        }

        #endregion

        #region 基础信息

        [WebMethod]
        public string GetPasswordByRandom()
        {
            Random rdm = new Random();
            int len = rdm.Next(6,10);
            string psw = (rdm.NextDouble() * int.MaxValue).ToString().PadLeft(6,'0');
            if(psw.Length > len) psw = psw.Substring(0,len);
            return psw;
        }

        #endregion

        #region 类别字典

        [WebMethod]
        public string GetJsonForContentType()
        {
            ContentType bll = new ContentType();
            return bll.GetTreeJson();
        }

        [WebMethod]
        public string SaveContentType(ContentTypeInfo model)
        {
            string errorMsg = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(model.TypeName))
                {
                    return MC.Submit_Params_InvalidError;
                }
                if (string.IsNullOrWhiteSpace(model.TypeCode))
                {
                    return MC.Submit_Params_InvalidError;
                }
                if (string.IsNullOrWhiteSpace(model.TypeValue))
                {
                    return MC.Submit_Params_InvalidError;
                }

                Guid gId = Guid.Empty;
                Guid.TryParse(model.Id.ToString(), out gId);
                model.Id = gId;

                Guid parentId = Guid.Empty;
                Guid.TryParse(model.ParentId.ToString(), out parentId);
                model.ParentId = parentId;
                model.LastUpdatedDate = DateTime.Now;

                ContentType bll = new ContentType();
                int effect = -1;

                using (TransactionScope scope = new TransactionScope())
                {
                    if (!gId.Equals(Guid.Empty))
                    {
                        effect = bll.Update(model);
                    }
                    else
                    {
                        model.HasChild = false;
                        effect = bll.Insert(model);
                    }

                    bll.UpdateHasChild(gId);
                    bll.UpdateHasChild(parentId);

                    scope.Complete();
                }

                if (effect == 110)
                {
                    return MC.Submit_Exist;
                }
                if (effect > 0)
                {
                    return "1";
                }
                else return MC.Submit_Error;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg); 
        }

        [WebMethod]
        public string DelContentType(string id)
        {
            string errorMsg = string.Empty;

            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                if (string.IsNullOrWhiteSpace(id))
                {
                    return MC.Submit_InvalidRow;
                }
                Guid gId = Guid.Empty;
                Guid.TryParse(id, out gId);
                if (gId.Equals(Guid.Empty))
                {
                    return MC.GetString(MC.Submit_Params_GetInvalidRegex, "对应标识值");
                }

                ContentType bll = new ContentType();
                var model = bll.GetModel(gId);
                if (model.TypeCode == TygaSoft.SysHelper.EnumData.MenuRoot.ShareMenu.ToString())
                {
                    return "该节点为系统预设值，不能删除！";
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    bll.Delete(gId);

                    if (model != null)
                    {
                        bll.UpdateHasChild(model.ParentId);
                    }

                    scope.Complete();
                }
                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg); 
        }

        [WebMethod]
        public string GetJsonForContentTypeByTypeCode(string typeCode)
        {
            ContentType bll = new ContentType();
            return bll.GetTreeJsonForContentTypeByTypeCode(typeCode);
        }

        [WebMethod]
        public string GetTreeJsonByParentCode(string typeCode)
        {
            ContentType bll = new ContentType();
            return bll.GetTreeJsonByParentCode(typeCode);
        }

        [WebMethod]
        public string CreateShareMenu(string typeCode)
        {
            try
            {
                ContentType bll = new ContentType();
                var list = bll.GetList();
                var model = list.FirstOrDefault(m => m.TypeCode == typeCode);
                if (model == null) return MC.Submit_Data_NotExists;

                string url = "/s/t.html";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"<Xel Url="""" Title=""{0}"" Description=""{1}"" Roles=""{2}"" Id=""{3}"" ParentId=""{4}"">", model.TypeValue, "", "", model.Id, model.ParentId);
                bll.GetShareSiteMap(list, model.Id, ref sb, url);
                sb.Append("</Xel>");

                var newRoot = XElement.Parse(string.Format("<Root>{0}</Root>", sb.ToString()));
                //var indexNode = newRoot.Descendants().FirstOrDefault(x => x.Attribute("Title").Value == "首页");
                //if (indexNode != null)
                //{
                //    indexNode.SetAttributeValue("Url", "/hkb/index.html");
                //}

                var siteMapFile = FileHelper.GetFullPath("/Xml/ShareSitemap.xml");
                var root = XElement.Load(siteMapFile);
                root.Descendants().Remove();

                root.Add(newRoot.Elements());
                root.Save(siteMapFile);

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region 内容公告资讯广告

        [WebMethod]
        public string DelContentDetail(string itemsAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                itemsAppend = itemsAppend.Trim();
                if (string.IsNullOrEmpty(itemsAppend))
                {
                    return MC.Submit_InvalidRow;
                }

                string[] items = itemsAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ContentDetail bll = new ContentDetail();
                bll.DeleteBatch(items.ToList<object>());

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg);
        }

        [WebMethod]
        public string DelAnnouncement(string itemsAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                itemsAppend = itemsAppend.Trim();
                if (string.IsNullOrEmpty(itemsAppend))
                {
                    return MC.Submit_InvalidRow;
                }

                string[] items = itemsAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Announcement bll = new Announcement();
                bll.DeleteBatch(items.ToList<object>());

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg); 
        }

        [WebMethod]
        public string DelNotice(string itemsAppend)
        {
            string errorMsg = string.Empty;
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                itemsAppend = itemsAppend.Trim();
                if (string.IsNullOrEmpty(itemsAppend))
                {
                    return MC.Submit_InvalidRow;
                }

                string[] items = itemsAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                Notice bll = new Notice();
                bll.DeleteBatch(items.ToList<object>());

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return MC.GetString(MC.Submit_Ex_Error, errorMsg); 
        }

        [WebMethod]
        public string DelPictureContent(string itemAppend)
        {
            try
            {
                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MC.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string inIds = "";
                Guid gId = Guid.Empty;
                foreach (string item in items)
                {
                    if (!Guid.TryParse(item, out gId))
                    {
                        throw new ArgumentException(MC.GetString(MC.Submit_Params_GetInvalidRegex, item));
                    }
                    inIds += string.Format("'{0}',", item);
                }

                PictureContent bll = new PictureContent();
                var list = bll.GetList(" and Id in (" + inIds.Trim(',') + ")");
                if (list != null || list.Count() > 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (var model in list)
                        {
                            string dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.RandomFolder));

                            if (Directory.Exists(dir))
                            {
                                string[] subDirArr = Directory.GetDirectories(dir);
                                if (subDirArr != null)
                                {
                                    foreach (string subDir in subDirArr)
                                    {
                                        Directory.Delete(subDir, true);
                                    }
                                }
                                Directory.Delete(dir, true);
                            }
                            dir = Server.MapPath("~" + string.Format("{0}{1}", model.FileDirectory, model.FileName));
                            if (File.Exists(dir))
                            {
                                File.Delete(dir);
                            }
                        }

                        bll.DeleteBatch(items.ToList<object>());

                        scope.Complete();
                    }
                }

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region GzxySiteDb

        [WebMethod]
        public string DelProductOnlineBook(string itemAppend)
        {
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MC.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ProductOnlineBook bll = new ProductOnlineBook();
                bll.DeleteBatch(items.ToList<object>());

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string SaveRiskTestQuestion(string xml,string Id)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return "{\"success\": false,\"message\": \"未找到任何可提交的数据，请检查\"}";
            }

            try
            {
                xml = HttpUtility.HtmlDecode(xml);
                XElement root = XElement.Parse(xml);
            }
            catch
            {
                return "{\"success\": false,\"message\": \"参数值不符合xml格式的字符串，请检查\"}";
            }

            try
            {
                Guid gId = Guid.Empty;
                if(!string.IsNullOrWhiteSpace(Id)) Guid.TryParse(Id,out gId);
                RiskTestQuestionInfo model = new RiskTestQuestionInfo();
                model.Id = gId;
                model.QuestionXml = xml;
                model.LastUpdatedDate = DateTime.Now;

                RiskTestQuestion bll = new RiskTestQuestion();
                int effect = 0;
                if (gId.Equals(Guid.Empty))
                {
                    gId = Guid.NewGuid();
                    model.Id = gId;
                    effect = bll.Insert(model);
                }
                else effect = bll.Update(model);

                if (effect < 1) return "{\"success\": false,\"message\": \"数据库连接操作异常，请稍后再重试\"}";

                return "{\"success\": true,\"message\": \"\",\"data\": \""+gId.ToString()+"\"}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string DelRiskTestQuestionAnswer(string itemAppend)
        {
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrators"))
                {
                    return MC.Role_InvalidError;
                }

                itemAppend = itemAppend.Trim();
                if (string.IsNullOrEmpty(itemAppend))
                {
                    return MC.Submit_InvalidRow;
                }

                string[] items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                RiskTestQuestionAnswer bll = new RiskTestQuestionAnswer();
                bll.DeleteBatch(items.ToList<object>());

                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion
    }
}
