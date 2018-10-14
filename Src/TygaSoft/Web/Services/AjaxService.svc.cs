using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using TygaSoft.Model;
using TygaSoft.BLL;

namespace TygaSoft.Web.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AjaxService : IAjaxService
    {
        #region IAjaxService Member

        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResult SaveProductOnlineBook(string customerName, string clientType, string telPhone, string mobilePhone, string fax, string email, string address, string bookProduct, decimal price)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerName))
                {
                    return ResError("客户名称不能为空字符串");
                }
                if (string.IsNullOrWhiteSpace(clientType))
                {
                    return ResError("客户类型不能为空字符串");
                }
                if (string.IsNullOrWhiteSpace(bookProduct))
                {
                    return ResError("预约产品不能为空字符串");
                }
                if (price < 1)
                {
                    return ResError("预约金额值无效");
                }
                if (string.IsNullOrWhiteSpace(telPhone) && string.IsNullOrWhiteSpace(mobilePhone) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(fax))
                {
                    return ResError("未找到任何联系方式，请至少填写一个联系方式");
                }

                ProductOnlineBookInfo model = new ProductOnlineBookInfo();
                model.CustomerName = customerName.Trim();
                model.ClientType = clientType.Trim();
                model.TelPhone = telPhone.Trim();
                model.MobilePhone = mobilePhone.Trim();
                model.Fax = fax.Trim();
                model.Email = email.Trim();
                model.Address = address.Trim();
                model.BookProduct = bookProduct.Trim();
                model.Price = price;
                model.LastUpdatedDate = DateTime.Now;

                ProductOnlineBook bll = new ProductOnlineBook();
                int effect = bll.Insert(model);
                if (effect < 1) return ResError("连接数据库操作异常，请稍后再重试");

                return ResSuccess("操作成功");
            }
            catch (Exception ex)
            {
                return ResError(ex.Message);
            }
        }

        [WebInvoke(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResult SaveRiskTestQuestionAnswer(string username,object questionId,  string xmlAnswer)
        {
            Guid qId = Guid.Empty;
            if (questionId != null) Guid.TryParse(questionId.ToString(), out qId);
            if (qId.Equals(Guid.Empty)) return ResError("未找到任何题ID，请正确操作");

            try
            {
                XElement root = XElement.Parse(xmlAnswer);
            }
            catch
            {
                return ResError("参数xmlAnswer值不合法");
            }

            try
            {
                RiskTestQuestionAnswerInfo model = new RiskTestQuestionAnswerInfo();
                model.UserId = Guid.Empty;
                model.QuestionId = qId;
                model.AnswerResult = xmlAnswer;
                model.LastUpdatedDate = DateTime.Now;
                RiskTestQuestionAnswer bll = new RiskTestQuestionAnswer();
                int effect = bll.Insert(model);
                if (effect < 1) return ResError("数据库连接操作异常，请稍后再重试");
                return ResSuccess("操作成功！");
            }
            catch (Exception ex)
            {
                return ResError(ex.Message);
            }
        }

        #endregion

        #region 辅助方法

        private ResResult ResError(string msg)
        {
            ResResult result = new ResResult();
            result.ResCode = (int)EnumData.ResCode.失败;
            result.Msg = msg;
            return result;
        }

        private ResResult ResSuccess(string msg)
        {
            ResResult result = new ResResult();
            result.ResCode = (int)EnumData.ResCode.成功;
            result.Msg = msg;
            return result;
        }

        #endregion
    }
}
