using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PC_Component_Info
{
    /// <summary>
    /// Interaktionslogik für sysinfo.xaml
    /// </summary>
    public partial class sysinfo : Page
    {
        SystemInfo si = new SystemInfo();

        public sysinfo()
        {
            InitializeComponent();

            LBL_PC_Name.Content = Vars.pc_name;
            LBL_User_Name.Content = Vars.user_name;
            LBL_OS_x86_x64.Content = "Betriebssystem: " + Vars.OSV + " (" + Vars.OSA + ")";
            LBL_OSP.Content = "Betriebsplatform: " + Vars.osp;

            LBL_CPU_Name.Content = "CPU Name: " + Vars.CPU_Name;
            LBL_cpu_threads.Content = "CPU Threads: " + Vars.CPU_Threads;
            LBL_cpu_cores.Content = "CPU Kerne: " + Vars.cpu_cores;
            LBL_cpu_takt.Content = "CPU Basis Takt: " + Vars.cpu_speed + "GHz";

            LBL_ram_ges.Content = "Installierter Ram: " + Vars.installed_ram + "GB";
            total_ram.Content = "Gesamt nutzbarer Ram: " + Vars.total_ram + "GB";
            hardware_ram.Content = "Für Hardware reserviert: " + Vars.hardware_ram + "GB";
            free_ram.Content = "Verfügbar: " + Vars.free_ram + "GB";
            used_ram.Content = "In Verwendung: " + Vars.ram_in_use + "GB";

            PB_1.Maximum = Vars.total_ram;
            PB_1.Value = Vars.ram_in_use;
            //LBL_ram_takt.Content = si.ram_takt;

            LBL_GPU_Name.Content = Vars.GPU_Name;
            LBL_GPU_VRAM.Content = "VRam: " + Vars.GPU_VRAM + "GB";

            if (PB_1.Value / PB_1.Maximum >= 0.85)
            {
                PB_1.Foreground = Brushes.Red;
            }
            else
            {
                PB_1.Foreground = SystemParameters.WindowGlassBrush;
            }
            timer();
        }

        private void timer()
        {
            int i = 0;

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(1000);

            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {

                SystemInfo si = new SystemInfo();

                i += 1;

                si.getRamInfo();
                Console.WriteLine("test" + i);

                free_ram.Content = "Verfügbar: " + Vars.free_ram + "GB";
                used_ram.Content = "In Verwendung: " + Vars.ram_in_use + "GB";

                PB_1.Maximum = Vars.total_ram;
                PB_1.Value = Vars.ram_in_use;

                if (PB_1.Value / PB_1.Maximum >= 0.85)
                {
                    PB_1.Foreground = Brushes.Red;
                }
                else
                {
                    PB_1.Foreground = SystemParameters.WindowGlassBrush;
                }

                if (Vars.page == 1 || Vars.page == 0)
                {
                    timer.Stop();
                }

            });

            timer.Start();
        }

    }
}
