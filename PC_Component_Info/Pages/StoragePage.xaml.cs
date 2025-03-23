using PC_Component_Info.ListContexts;
using PC_Component_Info.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PC_Component_Info.Pages
{
    /// <summary>
    /// Interaktionslogik für storage.xaml
    /// </summary>
    public partial class StoragePage : Page
    {
        DriveReader pcdr = new DriveReader();

        public StoragePage()
        {
            InitializeComponent();
            timer();
            LV_1.SelectedIndex = 0;
        }

        private void read()
        {
            Thread t = new Thread(readThread);
            t.Start();
        }

        void readThread()
        {
            for (int i = 0; i < Vars.drives.Length; i++)
            {
                pcdr.DriveRead(Vars.drives[i]);
            }
        }

        private void timer()
        {
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(1000);

            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                List<Drives> drives = new List<Drives>(Vars.ready_devices);
                LV_1.ItemsSource = null;
                LV_1.ItemsSource = drives;
                Vars.ready_devices.Clear();
                read();
            });

            timer.Start();
        }

        private void LV_1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LV_1.SelectedIndex >= 0 && LV_1.SelectedIndex < Vars.ready_devices.Count)
            {
                pcdr.DriveInfo(Vars.ready_devices[LV_1.SelectedIndex].DriveLetter);

                LBL_drive_name.Content = pcdr.name_vl;
                LBL_Type_di.Content = $"{Lang.Lang.tbType}: " + pcdr.dt;
                LBL_format_di.Content = $"{Lang.Lang.File_Format}: " + pcdr.df;
                LBL_ts_di.Content = $"{Lang.Lang.tbTotalMem}: " + pcdr.ts + " GB";
                LBL_tfs_di.Content = $"{Lang.Lang.tbFreeMem}: " + pcdr.tfs + " GB";
                PB_Drive.Tag = Math.Round((((pcdr.ts - pcdr.tfs) / pcdr.ts) * 100), 2) + "%";

                PB_Drive.Maximum = pcdr.ts;
                PB_Drive.Minimum = 0;
                PB_Drive.Value = pcdr.ts - pcdr.tfs;

                if ((pcdr.ts - pcdr.tfs) / pcdr.ts * 100 >= 85)
                {
                    PB_Drive.Foreground = Brushes.Red;
                }
                else
                {
                    PB_Drive.Foreground = SystemParameters.WindowGlassBrush;
                }
            }
        }
    }
}
