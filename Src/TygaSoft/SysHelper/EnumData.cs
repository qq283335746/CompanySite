using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.SysHelper
{
    public class EnumData
    {
        /// <summary>
        /// 客户类型（网上预约）
        /// </summary>
        public enum ClientType : byte { 个人客户 = 1, 机构客户 = 2 }

        /// <summary>
        /// 客户级别（客户登录）
        /// </summary>
        public enum ClientLevel : byte { 客户 = 1, 客户经理 = 2 }

        public enum MenuRoot { ShareMenu, Hkbzg, PageFooter, ProductAgreement }

        /// <summary>
        /// 设备 平台
        /// </summary>
        public enum Platform : byte { PC, Android, IOS }

        /// <summary>
        /// 原、大、中、小缩略图
        /// </summary>
        public enum PictureType : byte { OriginalPicture, BPicture, MPicture, SPicture }

        /// <summary>
        /// 元宝、金币、等级、颜色显示
        /// </summary>
        public enum LevelType : byte { SilverLevel = 1, VIPLevel = 2, GoldLevel = 3, ColorLevel = 4 }

        public enum QuestionLayout { IsRow,NotRow}
    }
}
