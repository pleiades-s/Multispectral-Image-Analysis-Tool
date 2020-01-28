using BeyonSense.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace BeyonSense.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
