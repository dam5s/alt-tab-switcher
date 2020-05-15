using System.Windows;
using SwitcherLib;

namespace SwitcherApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowHelper.configure(this, AppsGrid);
        }
    }
}
