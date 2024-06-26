﻿using System.Windows;

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
                         "- Added Tasks section, for managing tasks.\n" +
                         "- The information from more than one device will now be read out.\n" +
                         "\n\nFixes & Changes:\n" +
                         "- changed splashscreen\n" +
                         "- upgraded to .Net 8.\n" +
                         "- PC-Info is now based on PLP-SystemInfo.\n" +
                         "- minor design changes.\n" +
                         "- Code refactoring.\n" +
                         "- UI & technical improvements.";

        }
    }
}
