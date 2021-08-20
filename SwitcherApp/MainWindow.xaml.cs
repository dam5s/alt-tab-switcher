using System.Windows;
using SwitcherLib;

namespace SwitcherApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var factory = new AppWindowWidgetFactory();
            MainWindowHelper.configure(this, Panel, factory);
        }
    }

    internal class AppWindowWidgetFactory : MainWindowHelper.IAppWindowViewFactory
    {
        public UIElement Create(AppWindowViewModel viewModel) => new AppWindowView(viewModel);
    }
}
