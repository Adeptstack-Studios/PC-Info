using System.Diagnostics;
using System.Windows;

namespace PC_Component_Info
{
    /// <summary>
    /// Interaktionslogik für Icons.xaml
    /// </summary>
    public partial class Icons : Window
    {
        public Icons()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.flaticon.com/free-icons/hdd");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.flaticon.com/free-icons/usb");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.flaticon.com/free-icons/unknown");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.flaticon.com/free-icons/cd");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.flaticon.com/free-icons/ram-memory");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.flaticon.com/free-icons/server");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.flaticon.com/free-icons/database");
        }
    }
}
