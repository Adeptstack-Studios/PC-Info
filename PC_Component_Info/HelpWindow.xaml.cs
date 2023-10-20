using System.Diagnostics;
using System.Windows;

namespace PC_Component_Info
{
    /// <summary>
    /// Interaktionslogik für HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public string about = "PC-Info is an official, licensed, modern and easy-to-use \nprogram from Adeptstack to find out what \ncomponents are installed in your PC.";
        public string lizenz = "This product is an open source project provided by Adeptstack.\nLicense: AGPL-3.0 license\n© Adeptstack. All rights reserved";

        public HelpWindow()
        {
            InitializeComponent();

            LBL_About_Text.Content = about;
            LBL_Lizenz_Text.Content = lizenz;
            LBL_Version.Content = Vars.version;
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
            Process.Start("https://github.com/Adeptstack-Studios/PC-Info");
        }

        private void privacy_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://adeptstack.vercel.app/privacy");
        }

        private void icons_Click(object sender, RoutedEventArgs e)
        {
            Icons icons = new Icons();
            icons.Show();
        }
    }

}
