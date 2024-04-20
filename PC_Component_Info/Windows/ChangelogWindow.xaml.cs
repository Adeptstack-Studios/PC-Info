using System.Windows;

namespace PC_Component_Info.Windows
{
    /// <summary>
    /// Interaktionslogik für cl_win.xaml
    /// </summary>
    public partial class ChangelogWindow : Window
    {
        public ChangelogWindow()
        {
            InitializeComponent();

            TB_cl.Text = "News:\n" +
                         "- The information from more than one device will now be read out.\n" +
                         "- \n" +
                         "\n\nFixes & Changes:\n" +
                         "- upgraded to .Net 8.\n" +
                         "- PC-Info is now based on PLP-SystemInfo.\n" +
                         "- minor design changes.\n" +
                         "- Code refactoring.\n" +
                         "- UI & technical improvements.";

        }
    }
}
