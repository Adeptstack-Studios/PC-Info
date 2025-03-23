using PC_Component_Info.ListContexts;
using System.Collections.Generic;

namespace PC_Component_Info.Utilities
{
    internal class Vars
    {
        public static string version = "v3.2.0";

        public static string[] drives = new string[23]
        {
            @"A:\", @"B:\", @"C:\", @"D:\", @"E:\", @"F:\", @"G:\", @"H:\", @"I:\", @"J:\", @"K:\", @"L:\", @"M:\",
            @"O:\", @"P:\", @"Q:\", @"R:\", @"S:\", @"T:\", @"U:\", @"V:\", @"W:\", @"X:\"
        };

        //public static string[] drives2 = new string[26] { @"A:\", @"B:\", @"C:\", @"D:\", @"E:\", @"F:\", @"G:\", @"H:\", @"I:\", @"J:\", @"K:\", @"L:\", @"M:\", @"N:\", @"O:\", @"P:\", @"Q:\", @"R:\", @"S:\", @"T:\", @"U:\", @"V:\", @"W:\", @"X:\", @"Y:\", @"Z:\" };

        public static List<Drives> ready_devices = new List<Drives>();

    }
}
