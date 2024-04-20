using System.Diagnostics;
using System.Windows;

namespace PC_Component_Info.ListContexts
{
    internal class TaskContext
    {
        public string ProcessName { get; set; }
        public string ImagePath { get; set; }
        public Process BaseProcess { get; set; }
        public Visibility EfficiencyVisibility { get; set; }
    }
}
