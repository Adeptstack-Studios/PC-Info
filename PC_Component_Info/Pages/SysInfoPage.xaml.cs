using PLP_SystemInfo;
using PLP_SystemInfo.Collections;
using PLP_SystemInfo.ComponentInfo;
using PLP_SystemInfo.Models;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PC_Component_Info.Pages
{
    /// <summary>
    /// Interaktionslogik für sysinfo.xaml
    /// </summary>
    public partial class SysInfoPage : Page
    {
        ProcessorCollection processors;
        GraphicsCollection graphics;
        RamCollection ram;

        public SysInfoPage()
        {
            InitializeComponent();

            graphics = GraphicsInfo.GetGraphicscardInfo();
            ram = RamInfo.GetRamInfo();
            Thread t = new Thread(GetProcessorInfo);
            t.Start();

            SetValues();
            timer();
        }

        void GetProcessorInfo()
        {
            processors = ProcessorInfo.GetProcessors();
            Processors();
        }

        void SetValues()
        {
            YourPC();
            Ram();
        }

        void YourPC()
        {
            //Your PC
            Board mb = BoardInfo.GetMotherboard();
            BIOS bios = BoardInfo.GetBIOSInfo();
            TB_MaschineName.Text = $"Machinename: {SystemInfo.MachineName}";
            TB_Username.Text = $"Username: {SystemInfo.UserName}";
            TB_OSwA.Text = $"Operatingsystem: {OSInfo.GetOperatingSystemInfo()}";
            TB_MBManufacturer.Text = $"Manufacturer: {mb.Manufacturer}";
            TB_MBModel.Text = $"Model: {mb.Model}";
            TB_BIOSManufacturer.Text = $"Manufacturer: {bios.Manufacturer}";
            TB_BiosVersion.Text = $"Version: {bios.VersionName}";
        }

        void Processors()
        {
            //Processors
            for (int i = 0; i < processors.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => lbCPU.Items.Add($"CPU{i}"));
            }

            for (int i = 0; i < graphics.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => lbGPU.Items.Add($"GPU{i}"));
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                lbCPU.SelectedIndex = 0;
                lbGPU.SelectedIndex = 0;
            });
        }

        void ShowProcessor(int index)
        {
            TB_CPUName.Text = $"Name: {processors[index].Name}";
            TB_CPUArchitecture.Text = $"Architecture: {processors[index].Architecture}";
            TB_Threads.Text = $"Threads: {processors[index].Threads}";
            TB_Cores.Text = $"Cores: {processors[index].Cores}";
            TB_L2Cache.Text = $"L2 Cache: {processors[index].L2Cache} MB";
            TB_L3Cache.Text = $"L3 Cache: {processors[index].L3Cache} MB";
            TB_BaseClockSpeed.Text = $"Base Clockspeed: {processors[index].MaxClockSpeed} GHz";
        }

        void ShowGPU(int index)
        {
            TB_GraphicsName.Text = $"Name: {graphics[index].Name}";
            TB_DriverVersion.Text = $"Driverversion: {graphics[index].DriverVersion}";
        }

        void Ram()
        {
            //Ram
            TB_InstalledRam.Text = $"Installed Ram: {RamInfo.GetInstalledRAMSize()} GB";
            TB_TotalUsableRam.Text = $"Total Usable Ram: {RamInfo.GetTotalUsableRam()} GB";
            TB_HardwareReserved.Text = $"Reserved for hardware: {RamInfo.GetHardwareReservedRam()} MB";
            TB_UsedRam.Text = $"Ram used: {RamInfo.GetRamInUse()} GB";
            TB_AvailableRam.Text = $"Free ram: {RamInfo.GetAvailableRam()} GB";

            for (int i = 0; i < ram.Count; i++)
            {
                lbModules.Items.Add($"Module{i}");
            }
            lbModules.SelectedIndex = 0;
        }

        void ShowRamModule(int index)
        {
            TB_RamManufacturer.Text = $"Manufacturer: {ram[index].Manufacturer}";
            TB_RamSpeed.Text = $"Transferrate: {ram[index].Frequency}";
            TB_RamVoltage.Text = $"Voltage: {ram[index].Voltage}V";
            TB_ModuleSize.Text = $"Capacity: {ram[index].Size}Gb";
        }

        private void timer()
        {
            int i = 0;

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(1000);

            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                i += 1;

                double ramPercent = Math.Round(RamInfo.GetRamInUse() / RamInfo.GetTotalUsableRam(), 4) * 100;
                TB_UsedRam.Text = $"Ram used: {Math.Round(RamInfo.GetRamInUse(), 3)} GB";
                TB_AvailableRam.Text = $"Free ram: {RamInfo.GetAvailableRam()} GB";
                PB_Ram.Value = Math.Round(ramPercent, 2);
                PB_Ram.Tag = $"{Math.Round(ramPercent, 2)}%";

                if (PB_Ram.Value / PB_Ram.Maximum >= 0.85)
                {
                    PB_Ram.Foreground = Brushes.Red;
                }
                else
                {
                    PB_Ram.Foreground = SystemParameters.WindowGlassBrush;
                }
            });

            timer.Start();
        }

        private void lbModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbModules.SelectedIndex >= 0)
            {
                ShowRamModule(lbModules.SelectedIndex);
            }
        }

        private void lbCPU_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbCPU.SelectedIndex >= 0)
            {
                ShowProcessor(lbCPU.SelectedIndex);
            }
        }

        private void lbGPU_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbGPU.SelectedIndex >= 0)
            {
                ShowGPU(lbGPU.SelectedIndex);
            }
        }
    }
}
