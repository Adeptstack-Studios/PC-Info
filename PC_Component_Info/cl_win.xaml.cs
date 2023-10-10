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

            TB_cl.Text = "Neuigkeiten:\n" +
                         "---" +
                         "\n\nFixes & Änderungen:\n" +
                         "- updated links";

        }
    }
}
