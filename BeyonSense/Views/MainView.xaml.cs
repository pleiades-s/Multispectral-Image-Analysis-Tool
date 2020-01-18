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
        }

        // TODO: ViewModel로 옮겨야 함.
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            //folder dialog
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WinForms.DialogResult result = folderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //Selected Folder
                //< Selected Path >
                string sPath = folderDialog.SelectedPath;
                this.DataContext = new MainViewModel(sPath);

            }
        }


        // TODO: Change this method into MVVM sturcture ;-; Idk
        //private void TreeElement_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    string path = (string)FolderView.SelectedValue;
        //    Console.WriteLine(path);
        //}

    }
}
