using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.SysHelper
{
    public class EnumHelper
    {
        /// <summary>
        /// 内容类型
        /// </summary>
        public enum ContentType 
        {
            Announcement,Notice,AdvertisementCategory,AdvertisementPosition
        }

        /// <summary>
        /// 广告作用类型
        /// </summary>
        public enum AdvertisementCategory 
        {
            LinkToProduct, LinkToOutSite,ImageText
        }

        /// <summary>
        /// 广告放置位置
        /// </summary>
        public enum AdvertisementPosition
        {
            InTodaySpecial, InTenPrice
        }

        /// <summary>
        /// 商城菜单根编号
        /// </summary>
        public enum ShopMenu
        {
            CustomMenu
        }
    }
}
