﻿using PLP_SystemInfo;
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
            TB_MaschineName.Text = $"{Lang.Lang.tbMaschineName}: {SystemInfo.MachineName}";
            TB_Username.Text = $"{Lang.Lang.tbUsername}: {SystemInfo.UserName}";
            TB_OSwA.Text = $"{Lang.Lang.tbOperatingsystem}: {OSInfo.GetOperatingSystemInfo()}";
            TB_MBManufacturer.Text = $"{Lang.Lang.tbManufacturer}: {mb.Manufacturer}";
            TB_MBModel.Text = $"{Lang.Lang.tbModel}: {mb.Model}";
            TB_BIOSManufacturer.Text = $"{Lang.Lang.tbManufacturer}: {bios.Manufacturer}";
            TB_BiosVersion.Text = $"{Lang.Lang.tbVersion}: {bios.VersionName}";
        }

        void Processors()
        {
            //Processors
            for (int i = 0; i < processors.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => lbCPU.Items.Add($"CPU{i}"));
            }

            //Graphics
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
            TB_CPUName.Text = $"{Lang.Lang.tbName}: {processors[index].Name}";
            TB_CPUArchitecture.Text = $"{Lang.Lang.tbArchitecture}: {processors[index].Architecture}";
            TB_Threads.Text = $"{Lang.Lang.tbThreads}: {processors[index].Threads}";
            TB_Cores.Text = $"{Lang.Lang.tbCores}: {processors[index].Cores}";
            TB_L2Cache.Text = $"L2 Cache: {processors[index].L2Cache} MB";
            TB_L3Cache.Text = $"L3 Cache: {processors[index].L3Cache} MB";
            TB_BaseClockSpeed.Text = $"{Lang.Lang.tbBaseClockSpeed}: {processors[index].MaxClockSpeed} GHz";
        }

        void ShowGPU(int index)
        {
            TB_GraphicsName.Text = $"{Lang.Lang.tbName}: {graphics[index].Name}";
            TB_DriverVersion.Text = $"{Lang.Lang.tbDriverVersion}: {graphics[index].DriverVersion}";
        }

        void Ram()
        {
            //Ram
            TB_InstalledRam.Text = $"{Lang.Lang.tbInstalledRam}: {RamInfo.GetInstalledRAMSize()} GB";
            TB_TotalUsableRam.Text = $"{Lang.Lang.tbTotalUseableRam}: {RamInfo.GetTotalUsableRam()} GB";
            TB_HardwareReserved.Text = $"{Lang.Lang.tbResForHW}: {RamInfo.GetHardwareReservedRam()} MB";
            TB_UsedRam.Text = $"{Lang.Lang.tbRamInUse}: {RamInfo.GetRamInUse()} GB";
            TB_AvailableRam.Text = $"{Lang.Lang.tbFreeRam}: {RamInfo.GetAvailableRam()} GB";

            for (int i = 0; i < ram.Count; i++)
            {
                lbModules.Items.Add($"{Lang.Lang.tbModules}{i}");
            }
            lbModules.SelectedIndex = 0;
        }

        void ShowRamModule(int index)
        {
            TB_RamManufacturer.Text = $"{Lang.Lang.tbManufacturer}: {ram[index].Manufacturer}";
            TB_RamSpeed.Text = $"{Lang.Lang.tbTransferrate}: {ram[index].Frequency}";
            TB_RamVoltage.Text = $"{Lang.Lang.tbVoltage}: {ram[index].Voltage}V";
            TB_ModuleSize.Text = $"{Lang.Lang.tbCapacity}: {ram[index].Size}Gb";
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
                TB_UsedRam.Text = $"{Lang.Lang.tbRamInUse}: {Math.Round(RamInfo.GetRamInUse(), 3)} GB";
                TB_AvailableRam.Text = $"{Lang.Lang.tbFreeRam}: {RamInfo.GetAvailableRam()} GB";
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
