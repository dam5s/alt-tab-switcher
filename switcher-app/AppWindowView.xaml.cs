using System.Windows.Controls;
using SwitcherLib;

namespace SwitcherApp
{
    public partial class AppWindowView : UserControl
    {
        public AppWindowView(AppWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}