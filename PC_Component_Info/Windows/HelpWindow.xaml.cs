using PC_Component_Info.Utilities;
using System.Diagnostics;
using System.Windows;

namespace PC_Component_Info.Windows
{
    /// <summary>
    /// Interaktionslogik für HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public string about = Lang.Lang.tbAboutText;
        public string lizenz = Lang.Lang.tbLicense;

        public HelpWindow()
        {
            InitializeComponent();

            LBL_About_Text.Text = about;
            LBL_Lizenz_Text.Text = lizenz;
            LBL_Version.Content = Vars.version;
        }
        private void Exit_BTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void H_LI_BTN_Click(object sender, RoutedEventArgs e)
        {
            ChangelogWindow clw = new();
            clw.Show();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://adeptstack.net/PC-InfoDownload") { UseShellExecute = true });
        }

        private void website_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://adeptstack.net") { UseShellExecute = true });
        }

        private void github_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/Adeptstack-Studios/PC-Info") { UseShellExecute = true });
        }

        private void privacy_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://adeptstack.net/privacy") { UseShellExecute = true });
        }

        private void icons_Click(object sender, RoutedEventArgs e)
        {
            IconsWindow icons = new();
            icons.Show();
        }
    }

}
