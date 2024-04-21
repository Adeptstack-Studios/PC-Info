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
using System.Threading.Tasks;
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
            timer();
        }

        void GetActiveProcesses()
        {
            Task.Run(() =>
            {
                List<TaskContext> newApps = new List<TaskContext>();
                List<TaskContext> newBgApps = new List<TaskContext>();

                var processes = Process.GetProcesses();
                foreach (Process item in processes)
                {
                    try
                    {
                        if (!IsBannedProcess(item))
                        {
                            if (!IsCriticalProcess(item))
                            {
                                if (item.MainWindowHandle != IntPtr.Zero)
                                {
                                    newApps.Add(new TaskContext
                                    {
                                        BaseProcess = item,
                                        ProcessName = !string.IsNullOrEmpty(item.MainWindowTitle) ? item.MainWindowTitle + " - " + item.MainModule.ModuleName : item.MainModule.ModuleName,
                                        ImagePath = GetIcon(item.MainModule.FileName),
                                        EfficiencyVisibility = item.PriorityClass == ProcessPriorityClass.Idle ? Visibility.Visible : Visibility.Collapsed,
                                    });
                                }
                                else
                                {
                                    newBgApps.Add(new TaskContext
                                    {
                                        BaseProcess = item,
                                        ProcessName = !string.IsNullOrEmpty(item.MainWindowTitle) ? item.MainWindowTitle + " - " + item.MainModule.ModuleName : item.MainModule.ModuleName,
                                        ImagePath = GetIcon(item.MainModule.FileName),
                                        EfficiencyVisibility = item.PriorityClass == ProcessPriorityClass.Idle ? Visibility.Visible : Visibility.Collapsed,
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " - pN: " + item.ProcessName);// + " - " + ex.InnerException + " - " + ex.Source + " - " + ex.StackTrace);
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    apps = newApps.OrderBy(a => a.ProcessName).ToList();
                    bgApps = newBgApps.OrderBy(a => a.ProcessName).ToList();
                    int i1 = lbTasks.SelectedIndex;
                    int i2 = lbBgTasks.SelectedIndex;
                    lbTasks.ItemsSource = null;
                    lbTasks.ItemsSource = apps;
                    lbBgTasks.ItemsSource = null;
                    lbBgTasks.ItemsSource = bgApps;
                    lbTasks.SelectedIndex = i1;
                    lbBgTasks.SelectedIndex = i2;
                });
            });
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
            switch (p.ProcessName.ToLower())
            {
                case "svchost":
                    return true;
                case "conhost":
                    return true;
                case "symsrvhost":
                    return true;
                case "sihost.exe":
                    return true;
                case "taskhostw":
                    return true;
                case "textinputhost":
                    return true;
                case "applicationframehost":
                    return true;
                case "runtimebroker":
                    return true;
                case "backgroundtaskhost":
                    return true;
                case "dllhost":
                    return true;
                case "crashpad_handler":
                    return true;
                case "sdmicmute":
                    return true;
                case "idle":
                    return true;
                case "system":
                    return true;
                case "secure system":
                    return true;
                case "registry":
                    return true;
                case "smss":
                    return true;
                case "csrss":
                    return true;
                case "wininit":
                    return true;
                case "services":
                    return true;
                case "lsaiso":
                    return true;
                case "lsass":
                    return true;
                case "amdfendrsr":
                    return true;
                case "atiesrxx":
                    return true;
                case "memory compression":
                    return true;
                case "audiodg":
                    return true;
                case "spoolsv":
                    return true;
                case "wlanext":
                    return true;
                case "vmcompute":
                    return true;
                case "ijplmsvc":
                    return true;
                case "sqlwriter":
                    return true;
                case "tmshinstall":
                    return true;
                case "tminstall":
                    return true;
                case "jhi_service":
                    return true;
                case "dashost":
                    return true;
                case "msmpeng":
                    return true;
                case "mpdefendercoreservice":
                    return true;
                case "fontdrvhost":
                    return true;
                case "saservice":
                    return true;
                case "aggregatorhost":
                    return true;
                case "nissrv":
                    return true;
                case "vmmemcmzygote":
                    return true;
                case "searchindexer":
                    return true;
                case "securityhealthservice":
                    return true;
                case "atieclxx":
                    return true;
                case "dwm":
                    return true;
                case "winlogon":
                    return true;
                case "wudfhost":
                    return true;
                case "wmiprvse":
                    return true;
                case "intelcphdcpsvc":
                    return true;
                case "ctfmon":
                    return true;
                case "rpmdaemon":
                    return true;
                case "graphicscardengine":
                    return true;
                case "amdrsserv":
                    return true;
                case "amdow":
                    return true;
                case "searchprotocolhost":
                    return true;
                case "searchfilterhost":
                    return true;
                case "standardcollector.service":
                    return true;
                case "sihost":
                    return true;
                default:
                    return false;
            }
        }

        public string GetIcon(string fileName)
        {
            string path = Path.Combine(Data.appPath, "AppIcons");
            string file = Path.Combine(path, Path.GetFileNameWithoutExtension(fileName) + ".png");
            if (!File.Exists(file))
            {
                Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
                Bitmap bmp = icon.ToBitmap();
                bmp.Save(file, System.Drawing.Imaging.ImageFormat.Png);
            }
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
                Process.Start("explorer.exe", $"/select,\"{apps[lbTasks.SelectedIndex].BaseProcess.MainModule.FileName}\"");
            }
            else if (lbBgTasks.SelectedIndex >= 0)
            {
                Process.Start("explorer.exe", $"/select,\"{bgApps[lbBgTasks.SelectedIndex].BaseProcess.MainModule.FileName}\"");
            }
        }

        private void timer()
        {
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(2000);

            timer.Tick += new EventHandler(delegate (object s, EventArgs a)
            {
                GetActiveProcesses();
            });

            timer.Start();
        }
    }
}
