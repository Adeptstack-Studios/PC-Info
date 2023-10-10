using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Threading;

namespace PC_Component_Info
{
    class Timer1
    {
        public int sleep_time = 1000;

        public void timer()
        {
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(sleep_time);

            timer.Tick += new EventHandler(delegate (object s, EventArgs a) {

                SystemInfo si = new SystemInfo();
                PC_Drive_Reader pcdr = new PC_Drive_Reader();
                page_index_bridge pib = new page_index_bridge();

                si.getGeneralSystemInfo();
                si.getProcessorInfo();
                si.getRamInfo();
                pcdr.Drive_Read();

                pib.page_index();

                /*statusText.Text = string.Format("Timer Ticked: {0}ms",
                  Environment.TickCount);*/
            });

            timer.Start();

        }

        /*public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            
            
        }*/
    }
}
