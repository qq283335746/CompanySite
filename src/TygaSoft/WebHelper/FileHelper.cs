using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TygaSoft.WebHelper
{
    public class FileHelper
    {
        public static string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }
    }
}
