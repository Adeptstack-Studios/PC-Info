using System.Windows;

namespace PC_Component_Info
{
    /// <summary>
    /// Interaktionslogik für cl_win.xaml
    /// </summary>
    public partial class cl_win : Window
    {
        public cl_win()
        {
            InitializeComponent();

            TB_cl.Text = "News:\n" +
                         "- New arrangement of PC data.\n" +
                         "- Further PC data is read out: Motherboard, BIOS, CPU cache, various manufacturers and more.\n" +
                         "- \"Ready Devices\" design revised.\n" +
                         "\n\nFixes & Changes:\n" +
                         "- Progressbar now also displays the percentage.\n" +
                         "- Translated into English.\n" +
                         "- Code refactoring.\n" +
                         "- UI & technical improvements.";

        }
    }
}
