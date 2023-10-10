using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace PC_Component_Info
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = false;
                    break;
            }
            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;
            /// <summary>y coordinate of point.</summary>
            public int y;
            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();
            public int Width { get { return Math.Abs(right - left); } }
            public int Height { get { return bottom - top; } }
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty { get { return left >= right || top >= bottom; } }
            public override string ToString()
            {
                if (this == Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }
            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode() => left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2) { return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom); }
            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2) { return !(rect1 == rect2); }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        public int select_index = 2;
        public int selected_index;

        SystemInfo si = new SystemInfo();

        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += (s, e) =>
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };
            min_btn.Click += (s, e) =>
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Minimized;
            };
            //max_btn.Click += (s, e) =>
            //{
            //    WindowStyle = WindowStyle.SingleBorderWindow;
            //    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            //};
            close_btn.Click += (s, e) =>
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                Close();
            };

            si.getGeneralSystemInfo();
            si.getProcessorInfo();
            si.getRamInfo();
            si.getOthersInfo();

            if (select_index == 1)
            {
                PageFrame_1.Content = new storage();
                //cpu_btn.Background = Brushes.White;
                //ram_btn.Background = Brushes.White;
                storage_btn.Background = SystemParameters.WindowGlassBrush;
                sys_info_btn.Background = Brushes.Transparent;
                selected_index = 1;
                Vars.page = 0;
            }

            if (select_index == 2)
            {
                PageFrame_1.Content = new sysinfo();
                //cpu_btn.Background = Brushes.White;
                //ram_btn.Background = Brushes.White;
                storage_btn.Background = Brushes.Transparent;
                sys_info_btn.Background = SystemParameters.WindowGlassBrush;
                selected_index = 2;
                Vars.page = 0;
            }
            page_index_bg();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (WindowStyle != WindowStyle.None)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (DispatcherOperationCallback)delegate (object unused)
                {
                    WindowStyle = WindowStyle.None;
                    return null;
                }
                , null);
            }
        }

        private void mainwindow_StateChanged(object sender, EventArgs e)
        {
            if (mainwindow.WindowState == WindowState.Normal)
            {
                //mainwindow.WindowState = WindowState.Normal;
            }
            else if (mainwindow.WindowState == WindowState.Maximized)
            {
                mainwindow.WindowState = WindowState.Normal;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void storage_btn_Click(object sender, RoutedEventArgs e)
        {
            selected_index = 1;
            page_index_bg();
            //Menu_Click();
        }

        private void sys_info_btn_Click(object sender, RoutedEventArgs e)
        {
            selected_index = 2;
            page_index_bg();
            //Menu_Click();
        }

        private void File_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //Environment.Exit(0);
        }

        private void help_about_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();

            hw.Show();
            //Menu_Click();
        }

        private void refresh_speed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void default_card_Click(object sender, RoutedEventArgs e)
        {

        }

        public void page_index_bg()
        {

            if (selected_index == 1)
            {

                PageFrame_1.Content = new storage();
                //cpu_btn.Background = Brushes.White;
                //ram_btn.Background = Brushes.White;
                storage_btn.Background = SystemParameters.WindowGlassBrush;
                sys_info_btn.Background = Brushes.Transparent;
                Vars.page = 1;
            }

            if (selected_index == 2)
            {
                PageFrame_1.Content = new sysinfo();
                //cpu_btn.Background = Brushes.White;
                //ram_btn.Background = Brushes.White;
                storage_btn.Background = Brushes.Transparent;
                sys_info_btn.Background = SystemParameters.WindowGlassBrush;
                Vars.page = 2;
            }
        }

        //private void test_Click(object sender, RoutedEventArgs e)
        //{
        //    Menu_Click();
        //}

        //public void Menu_Click()
        //{
        //    if (SP_1.ActualWidth > 100)
        //    {

        //        BeginStoryboard sb_t50 = this.FindResource("SB_sp_m_t50") as BeginStoryboard;
        //        sb_t50.Storyboard.Completed += new EventHandler(delegate (object sender1, EventArgs a)
        //        {
        //            //SP_1.Width = 50;
        //            storage_btn.Visibility = Visibility.Collapsed;
        //            sys_info_btn.Visibility = Visibility.Collapsed;
        //            CHB_DM.Visibility = Visibility.Collapsed;
        //            HuÜI.Visibility = Visibility.Collapsed;
        //            Exit.Visibility = Visibility.Collapsed;
        //            PageFrame_1.Opacity = 1;
        //        });
        //        sb_t50.Storyboard.Begin();
        //    }
        //    else
        //    {
        //        BeginStoryboard sb_t200 = this.FindResource("SB_sp_m_t200") as BeginStoryboard;
        //        sb_t200.Storyboard.Begin();
        //        //SP_1.Width = 200;
        //        storage_btn.Visibility = Visibility.Visible;
        //        sys_info_btn.Visibility = Visibility.Visible;
        //        CHB_DM.Visibility = Visibility.Visible;
        //        HuÜI.Visibility = Visibility.Visible;
        //        Exit.Visibility = Visibility.Visible;
        //        PageFrame_1.Opacity = 0.65;
        //    }
        //}

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CHB_DM_Checked(object sender, RoutedEventArgs e)
        {
            Vars.page = 0;
            main_sp.Background = new SolidColorBrush(Color.FromRgb(34, 34, 34));
            Vars.BGC = new SolidColorBrush(Color.FromRgb(34, 34, 34));
            Vars.FGC = Brushes.White;
            pu();
        }

        private void CHB_DM_Unchecked(object sender, RoutedEventArgs e)
        {
            Vars.page = 0;
            main_sp.Background = Brushes.White;
            Vars.BGC = Brushes.White;
            Vars.FGC = Brushes.Black;
            pu();
        }

        private void pu()
        {

            if (selected_index == 1)
            {
                PageFrame_1.Content = new storage();
                Vars.page = 1;
            }
            else if (selected_index == 2)
            {
                PageFrame_1.Content = new sysinfo();
                Vars.page = 2;
            }
        }

        private void PageFrame_1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (SP_1.ActualWidth > 100)
            //{

            //    BeginStoryboard sb_t50 = this.FindResource("SB_sp_m_t50") as BeginStoryboard;
            //    sb_t50.Storyboard.Completed += new EventHandler(delegate (object sender1, EventArgs a)
            //    {
            //        storage_btn.Visibility = Visibility.Collapsed;
            //        sys_info_btn.Visibility = Visibility.Collapsed;
            //        HuÜI.Visibility = Visibility.Collapsed;
            //        Exit.Visibility = Visibility.Collapsed;
            //        PageFrame_1.Opacity = 1;
            //    });
            //    sb_t50.Storyboard.Begin();
            //}
        }
        /*public void timer()
{
   int i = 0;

   DispatcherTimer timer = new DispatcherTimer();

   timer.Interval = TimeSpan.FromMilliseconds(1000);

   timer.Tick += new EventHandler(delegate (object s, EventArgs a) {

       SystemInfo si = new SystemInfo();
       PC_Drive_Reader pcdr = new PC_Drive_Reader();

       storage st = new storage(); 

       Console.WriteLine(st.LBL_format_d1.Content + " " + i);

       i += 1;
       st.LBL_format_d1.Content = i.ToString();

       //si.getGeneralSystemInfo();
       //si.getProcessorInfo();
       si.getRamInfo();
       pcdr.Drive_Read();
       Console.WriteLine("test");

       page_index_bg();

       /*statusText.Text = string.Format("Timer Ticked: {0}ms",
         Environment.TickCount);
   });

   timer.Start();
}*/
    }
}
