using System;
using System.Windows.Controls;
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

            //if (PB_1.Value / PB_1.Maximum >= 0.85)
            //{
            //    PB_1.Foreground = Brushes.Red;
            //}
            //else
            //{
            //    PB_1.Foreground = SystemParameters.WindowGlassBrush;
            //}
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

                //free_ram.Content = "Verfügbar: ";
                //used_ram.Content = "In Verwendung: ";

                //PB_1.Maximum = Vars.total_ram;
                //PB_1.Value = Vars.ram_in_use;

                //if (PB_1.Value / PB_1.Maximum >= 0.85)
                //{
                //    PB_1.Foreground = Brushes.Red;
                //}
                //else
                //{
                //    PB_1.Foreground = SystemParameters.WindowGlassBrush;
                //}

                if (Vars.page == 1 || Vars.page == 0)
                {
                    timer.Stop();
                }

            });

            timer.Start();
        }

    }
}
