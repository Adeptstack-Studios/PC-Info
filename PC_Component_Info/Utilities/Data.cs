using System;
using System.IO;

namespace PC_Component_Info.Utilities
{
    internal class Data
    {
        public static string appPath = @"C:\Users\" + Environment.UserName + @"\Documents\PC-Info";

        public static void Create()
        {
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }

            string icons = Path.Combine(appPath, "AppIcons");
            if (!Directory.Exists(icons))
            {
                Directory.CreateDirectory(icons);
            }
        }
    }
}
