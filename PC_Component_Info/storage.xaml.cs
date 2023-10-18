using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PC_Component_Info
{
    /// <summary>
    /// Interaktionslogik für storage.xaml
    /// </summary>
    public partial class storage : Page
    {
        DriveReader pcdr = new DriveReader();

        public storage()
        {
            InitializeComponent();

            Vars.ready_devices.Clear();
            read();
            timer();

            pcdr.drive_info(Vars.ready_devices[0]);

            LBL_drive_name.Content = pcdr.name_vl;
            LBL_Type_di.Content = "Typ: " + pcdr.dt;
            LBL_format_di.Content = "Datei Format: " + pcdr.df;
            LBL_ts_di.Content = "Gesamter Speicherplatz: " + pcdr.ts + "GB";
            LBL_tfs_di.Content = "Freier Speicherplatz: " + pcdr.tfs + "GB";
            PB_Drive.Tag = Math.Round((((pcdr.ts - pcdr.tfs) / pcdr.ts) * 100), 2) + "%";

            PB_Drive.Maximum = pcdr.ts;
            PB_Drive.Minimum = 0;
            PB_Drive.Value = pcdr.ts - pcdr.tfs;

            LV_1.ItemsSource = Vars.ready_devices;

        }

        private void read()
        {
            for (int i = 0; i < Vars.drives.Length; i++)
            {
                pcdr.Drive_Read(Vars.drives[i]);
            }
        }

        private void timer()
        {
            int it = 0;

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(1000);

            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {

                it += 1;

                Console.WriteLine("control" + it);

                Vars.ready_devices.Clear();
                read();

                DriveInfo di = new DriveInfo(@"C:\");
                Console.WriteLine(di.VolumeLabel);

                LV_1.ItemsSource = null;
                LV_1.ItemsSource = Vars.ready_devices;

                if (Vars.page == 2 || Vars.page == 0)
                {
                    timer.Stop();
                }

            });

            timer.Start();
        }

        private void LV_1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            pcdr.drive_info(Vars.ready_devices[LV_1.SelectedIndex]);

            LBL_drive_name.Content = pcdr.name_vl;
            LBL_Type_di.Content = "Typ: " + pcdr.dt;
            LBL_format_di.Content = "Datei Format: " + pcdr.df;
            LBL_ts_di.Content = "Gesamter Speicherplatz: " + pcdr.ts + "GB";
            LBL_tfs_di.Content = "Freier Speicherplatz: " + pcdr.tfs + "GB";
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
