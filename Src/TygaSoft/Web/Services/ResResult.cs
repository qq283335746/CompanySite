using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace TygaSoft.Web.Services
{
    public class ResResult
    {
        [DataMember]
        public int ResCode { get; set; }

        [DataMember]
        public string Msg { get; set; }

        [DataMember]
        public object Data { get; set; }
    }

    public class EnumData
    {
        public enum ResCode { 成功 = 1000, 失败 = 1001 };
    }
}