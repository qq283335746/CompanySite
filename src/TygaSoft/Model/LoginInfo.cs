using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    public class LoginInfo
    {
        public LoginInfo() { }

        public LoginInfo(string username,DateTime lastUpdatedDate)
        {
            UserName = username;
            LastUpdatedDate = lastUpdatedDate;
        }

        public string UserName { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
