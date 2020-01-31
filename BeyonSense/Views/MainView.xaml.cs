using BeyonSense.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

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
