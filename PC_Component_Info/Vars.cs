using System.Collections.Generic;
using System.Windows.Media;

namespace PC_Component_Info
{
    internal class Vars
    {
        public static int page;

        public static string OSV;
        public static string OSA;
        public static string pc_name;
        public static string user_name;
        public static string osp;

        //CPU Vstatic ariablen
        public static string CPU_Name;
        public static int CPU_Threads;
        public static string cpu_cores;
        public static double cpu_speed;

        //Ram Variablen
        public static float free_ram;
        public static float total_ram;
        public static float ram_in_use;
        public static float hardware_ram;
        public static long installed_ram;

        public static string GPU_Name;
        public static long GPU_VRAM;


        //Eigenschaften
        public static Brush BGC { get; set; }
        public static string MC { get; set; }
        public static Brush FGC { get; set; } = Brushes.White;

        public static string[] drives = new string[26]
        {
            @"A:\\", @"B:\\", @"C:\\", @"D:\\", @"E:\\", @"F:\\", @"G:\\", @"H:\\", @"I:\\", @"J:\\", @"K:\\", @"L:\\", @"M:\\",
            @"N:\\", @"O:\\", @"P:\\", @"Q:\\", @"R:\\", @"S:\\", @"T:\\", @"U:\\", @"V:\\", @"W:\\", @"X:\\", @"Y:\\", @"Z:\\"
        };

        public static List<string> ready_devices = new List<string>();

    }
}
