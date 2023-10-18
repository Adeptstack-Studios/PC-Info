using System.Collections.Generic;

namespace PC_Component_Info
{
    internal class Vars
    {
        public static int page;
        public static string version = "v1.3.0";

        public static string[] drives = new string[26]
        {
            @"A:\\", @"B:\\", @"C:\\", @"D:\\", @"E:\\", @"F:\\", @"G:\\", @"H:\\", @"I:\\", @"J:\\", @"K:\\", @"L:\\", @"M:\\",
            @"N:\\", @"O:\\", @"P:\\", @"Q:\\", @"R:\\", @"S:\\", @"T:\\", @"U:\\", @"V:\\", @"W:\\", @"X:\\", @"Y:\\", @"Z:\\"
        };

        public static List<string> ready_devices = new List<string>();

    }
}
