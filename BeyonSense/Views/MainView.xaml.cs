using BeyonSense.ViewModels;
using System.Windows;
namespace BeyonSense.Views
{
    /// <summary>
    /// MainView behind code
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

    }
}
