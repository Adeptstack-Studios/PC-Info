using Microsoft.Win32;
using PC_Component_Info.ListContexts;
using PC_Component_Info.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PC_Component_Info.Pages
{
    /// <summary>
    /// Interaktionslogik für TasksPage.xaml
    /// </summary>
    public partial class TasksPage : Page
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool IsProcessCritical(IntPtr hProcess, ref bool Critical);

        List<TaskContext> apps = new List<TaskContext>();
        List<TaskContext> bgApps = new List<TaskContext>();

        public TasksPage()
        {
            InitializeComponent();
            GetActiveProcesses();
            timer();
        }

        void GetActiveProcesses()
        {
            apps.Clear();
            bgApps.Clear();

            var processes = Process.GetProcesses();
            foreach (Process item in processes)
            {
                try
                {
                    if (!IsCriticalProcess(item) && !IsBannedProcess(item))
                    {
                        if (item.MainWindowHandle != IntPtr.Zero)
                        {
                            apps.Add(new TaskContext
                            {
                                BaseProcess = item,
                                ProcessName = !string.IsNullOrEmpty(item.MainWindowTitle) ? item.MainWindowTitle + " - " + item.MainModule.ModuleName : item.MainModule.ModuleName,
                                ImagePath = GetIcon(item.MainModule.FileName),
                                EfficiencyVisibility = item.PriorityClass == ProcessPriorityClass.Idle ? Visibility.Visible : Visibility.Collapsed,
                            });
                        }
                        else
                        {
                            bgApps.Add(new TaskContext
                            {
                                BaseProcess = item,
                                ProcessName = !string.IsNullOrEmpty(item.MainWindowTitle) ? item.MainWindowTitle + " - " + item.MainModule.ModuleName : item.MainModule.ModuleName,
                                ImagePath = GetIcon(item.MainModule.FileName),
                                EfficiencyVisibility = item.PriorityClass == ProcessPriorityClass.Idle ? Visibility.Visible : Visibility.Collapsed,
                            });
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            apps = apps.OrderBy(a => a.ProcessName).ToList();
            bgApps = bgApps.OrderBy(a => a.ProcessName).ToList();
        }

        bool IsCriticalProcess(Process p)
        {
            bool criticalProcess = true;

            if (!IsProcessCritical(p.Handle, ref criticalProcess))
            {
                Console.WriteLine("Could not retrieve process information");
            }

            if (criticalProcess)
            {
                // This is a critical process, it should be listed
                // in the "Windows processes" section.
                return true;
            }
            else
            {
                // Backgroundprocess
                return false;
            }
        }

        bool IsBannedProcess(Process p)
        {
            switch (p.MainModule.ModuleName.ToLower())
            {
                case "svchost.exe":
                    return true;
                case "conhost.exe":
                    return true;
                case "symsrvhost.exe":
                    return true;
                case "sihost.exe":
                    return true;
                case "taskhostw.exe":
                    return true;
                case "textinputhost.exe":
                    return true;
                case "applicationframehost.exe":
                    return true;
                case "runtimebroker.exe":
                    return true;
                case "backgroundtaskhost.exe":
                    return true;
                case "dllhost.exe":
                    return true;
                case "crashpad_handler.exe":
                    return true;
                case "sdmicmute.exe":
                    return true;
                //case "":
                //    return true;
                //case "":
                //    return true;
                //case "":
                //    return true;
                //case "":
                //    return true;
                //case "":
                //return true;
                default:
                    return false;
            }
        }

        public string GetIcon(string fileName)
        {
            string path = Path.Combine(Data.appPath, "AppIcons");
            string file = Path.Combine(path, Path.GetFileNameWithoutExtension(fileName) + ".png");
            Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
            Bitmap bmp = icon.ToBitmap();
            bmp.Save(file, System.Drawing.Imaging.ImageFormat.Png);
            return file;
            //return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
            //            icon.Handle,
            //            new Int32Rect(0, 0, icon.Width, icon.Height),
            //            BitmapSizeOptions.FromEmptyOptions());
        }

        private void lbTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbTasks.SelectedIndex >= 0)
            {
                lbBgTasks.SelectedIndex = -1;
            }
        }

        private void lbBgTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbBgTasks.SelectedIndex >= 0)
            {
                lbTasks.SelectedIndex = -1;
            }
        }

        private void newTaskBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Open";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(openFileDialog.FileName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void killTaskBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lbTasks.SelectedIndex >= 0)
            {
                apps[lbTasks.SelectedIndex].BaseProcess.Kill();
            }
            else if (lbBgTasks.SelectedIndex >= 0)
            {
                bgApps[lbBgTasks.SelectedIndex].BaseProcess.Kill();
            }
        }

        private void efficiencyModeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lbTasks.SelectedIndex >= 0)
            {
                apps[lbTasks.SelectedIndex].BaseProcess.PriorityClass = ProcessPriorityClass.Idle;
            }
            else if (lbBgTasks.SelectedIndex >= 0)
            {
                bgApps[lbBgTasks.SelectedIndex].BaseProcess.PriorityClass = ProcessPriorityClass.Idle;
            }
        }

        private void openFilePathBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lbTasks.SelectedIndex >= 0)
            {
                Process.Start(new ProcessStartInfo(apps[lbTasks.SelectedIndex].BaseProcess.MainModule.FileName) { UseShellExecute = true });
            }
            else if (lbBgTasks.SelectedIndex >= 0)
            {
                Process.Start(new ProcessStartInfo(bgApps[lbBgTasks.SelectedIndex].BaseProcess.MainModule.FileName) { UseShellExecute = true });
            }
        }

        private void timer()
        {
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(1000);

            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                Thread thread = new Thread(new ThreadStart(GetActiveProcesses));
                int i1 = lbTasks.SelectedIndex;
                int i2 = lbBgTasks.SelectedIndex;
                lbTasks.ItemsSource = null;
                lbTasks.ItemsSource = apps;
                lbBgTasks.ItemsSource = null;
                lbBgTasks.ItemsSource = bgApps;
                lbTasks.SelectedIndex = i1;
                lbBgTasks.SelectedIndex = i2;
            });

            timer.Start();
        }
    }
}
