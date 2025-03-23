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

            TB_cl.Text = $"{Lang.Lang.tbNews}:\n" +
                         $"{Lang.Lang.clNews}\n" +
                         $"\n{Lang.Lang.tbFixesChanges}:\n" +
                         $"{Lang.Lang.clChangesFixes}";

        }
    }
}
