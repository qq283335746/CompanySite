using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TygaSoft.Web.Services
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IHkbAsset”。
    [ServiceContract(Namespace = "http://TygaSoft.Services")]
    public interface IAjaxService
    {
        #region IAjaxService Member

        [OperationContract(Name = "SaveProductOnlineBook")]
        ResResult SaveProductOnlineBook(string customerName, string clientType, string telPhone, string mobilePhone, string fax, string email, string address, string bookProduct, decimal price);

        [OperationContract(Name = "SaveRiskTestQuestionAnswer")]
        ResResult SaveRiskTestQuestionAnswer(string username, object questionId, string xmlAnswer);

        #endregion
    }
}
