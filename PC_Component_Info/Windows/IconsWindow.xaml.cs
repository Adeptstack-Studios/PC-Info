using System.Diagnostics;
using System.Windows;

namespace PC_Component_Info.Windows
{
    /// <summary>
    /// Interaktionslogik für Icons.xaml
    /// </summary>
    public partial class IconsWindow : Window
    {
        public IconsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.flaticon.com/free-icons/hdd") { UseShellExecute = true });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.flaticon.com/free-icons/usb") { UseShellExecute = true });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.flaticon.com/free-icons/unknown") { UseShellExecute = true });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.flaticon.com/free-icons/cd") { UseShellExecute = true });
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.flaticon.com/free-icons/ram-memory") { UseShellExecute = true });
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.flaticon.com/free-icons/server") { UseShellExecute = true });
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.flaticon.com/free-icons/database") { UseShellExecute = true });
        }
    }
}
