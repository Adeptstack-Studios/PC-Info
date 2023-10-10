using System.Diagnostics;
using System.Windows;

namespace PC_Component_Info
{
    /// <summary>
    /// Interaktionslogik für HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public string about = "PC-Info ist ein offizielles, lizensiertes, modern \ngestalltetes und leicht zu benutztendes Programm von \nAdeptstack um zu erfahren, welche Komponenten\nin ihrem PC verbaut sind.";
        public string lizenz = "Dieses Produkt wurde von Adeptstack zur Verfügen gestellt \nNutzung nur für privat Anwender erlaubt! Siehe Lizenz Informationen. \n© Adeptstack. All rights reserved";
        public string version_text = "Version: 1.2.2";

        public HelpWindow()
        {
            InitializeComponent();

            LBL_About_Text.Content = about;
            LBL_Lizenz_Text.Content = lizenz;
            LBL_Version.Content = version_text;
            //LBL_OS_Name.Content = "Betriebsystem: " + sysinfo.OSN + " (" + sysinfo.OSA + ")";
            //LBL_CPU_Name.Content = sysinfo.cpuCores;
        }
        private void Exit_BTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void H_LI_BTN_Click(object sender, RoutedEventArgs e)
        {
            cl_win clw = new cl_win();
            clw.Show();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://adeptstack.vercel.app/PC-InfoDownload");
        }

        private void website_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://adeptstack.vercel.app");
        }

        private void github_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/ProgrammerLP/PC-Info");
        }

        private void privacy_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://adeptstack.vercel.app/privacy");
        }
    }

}
