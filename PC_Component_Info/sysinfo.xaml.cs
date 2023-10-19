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

            SetValues();
            timer();
        }

        void SetValues()
        {
            YourPC();
            Processors();
            Ram();
        }

        void YourPC()
        {
            //Your PC
            var names = SystemInfo.GetNames();
            var mb = SystemInfo.GetMotherboard();
            var bios = SystemInfo.GetBIOSInfo();
            TB_MaschineName.Text = $"Machinename: {names.machineName}";
            TB_Username.Text = $"Username: {names.userName}";
            TB_OSwA.Text = $"Operatingsystem: {SystemInfo.GetOSInfo()}";
            TB_OSPlatform.Text = $"OS Platform: {SystemInfo.GetOSPlatform()}";
            TB_MBManufacturer.Text = $"Manufacturer: {mb.manufacturer}";
            TB_MBModel.Text = $"Model: {mb.model}";
            TB_BIOSManufacturer.Text = $"Manufacturer: {bios.manufacturer}";
            TB_BiosVersion.Text = $"Version: {bios.versionName}";
        }

        void Processors()
        {
            //Processors
            var processor = SystemInfo.GetProcessorInfo();
            var coresThreads = SystemInfo.GetCoresAndThreads();
            var cache = SystemInfo.GetCacheSize();
            var clockspeed = SystemInfo.GetCPUClockSpeed();
            TB_CPUName.Text = $"Name: {processor.name}";
            TB_CPUArchitecture.Text = $"Architecture: {processor.architecture}";
            TB_Threads.Text = $"Threads: {coresThreads.threads}";
            TB_Cores.Text = $"Cores: {coresThreads.cores}";
            TB_L2Cache.Text = $"L2 Cache: {cache.l2cache} MB";
            TB_L3Cache.Text = $"L3 Cache: {cache.l3cahce} MB";
            TB_BaseClockSpeed.Text = $"Base Clockspeed: {clockspeed.maxSpeed} GHz";

            var graphics = SystemInfo.GetGraphicscardInfo();
            TB_GraphicsName.Text = $"Name: {graphics.name}";
            TB_Vram.Text = $"Vram: {graphics.vram} GB";
            TB_DriverVersion.Text = $"Driverversion: {graphics.DriverVersion}";
        }

        void Ram()
        {
            //Ram
            var ramInfo = SystemInfo.GetRamInfo();
            TB_InstalledRam.Text = $"Installed Ram: {SystemInfo.GetInstalledRam()} GB";
            TB_TotalUsableRam.Text = $"Total Usable Ram: {SystemInfo.GetTotalUsableRam()} GB";
            TB_HardwareReserved.Text = $"Reserved for hardware: {SystemInfo.GetHardwareReservedRam()} GB";
            TB_UsedRam.Text = $"Ram used: {SystemInfo.GetUsedRam()} GB";
            TB_AvailableRam.Text = $"Free ram: {SystemInfo.GetAvailableRam()} GB";
            TB_RamManufacturer.Text = $"Manufacturer: {ramInfo.manufacturer}";
            TB_RamSpeed.Text = $"Transferrate: {ramInfo.frequency}";
            TB_RamVoltage.Text = $"Voltage: {ramInfo.voltage}";
        }

        private void timer()
        {
            int i = 0;

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(1000);

            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                i += 1;

                double ramPercent = Math.Round(SystemInfo.GetUsedRam() / SystemInfo.GetTotalUsableRam(), 4) * 100;
                TB_UsedRam.Text = $"Ram used: {SystemInfo.GetUsedRam()} GB";
                TB_AvailableRam.Text = $"Free ram: {SystemInfo.GetAvailableRam()} GB";
                PB_Ram.Value = ramPercent;
                PB_Ram.Tag = $"{ramPercent}%";

                if (PB_Ram.Value / PB_Ram.Maximum >= 0.85)
                {
                    PB_Ram.Foreground = Brushes.Red;
                }
                else
                {
                    PB_Ram.Foreground = SystemParameters.WindowGlassBrush;
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
