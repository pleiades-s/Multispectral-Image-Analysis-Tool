using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using WinForms = System.Windows.Forms;
using System.Diagnostics;


namespace BeyonSense
{
    /// <summary>
    /// Interaction logic for Labeling_Page.xaml
    /// </summary>
    public partial class Labeling_Page : Page
    {
        public Labeling_Page()
        {
            InitializeComponent();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {

            Dictionary<int, string> photo_dict = new Dictionary<int, string>();
            String sPath = "";

            //folder dialog
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WinForms.DialogResult result = folderDialog.ShowDialog();

            if (result == WinForms.DialogResult.OK)
            {
                //Selected Folder
                //< Selected Path >
                sPath = folderDialog.SelectedPath;

                //</ Selected Path >

                //Traverse in the selected floder
                //TODO: 예외처리 해야 함. 이미지 파일만 6개여야 하고, metadata.csv가 있을 수도 있다.

                DirectoryInfo folder = new DirectoryInfo(sPath);
                if (folder.Exists)
                {
                    int i = 0;
                    //loop for files in the folder
                    foreach (FileInfo fileInfo in folder.GetFiles())
                    {
                        //TODO: 잘못된 폴더 선택할 때, 혹은 metadata 있을 때 고려해서 catch 해야 함!!
                       
                        //each file path
                        photo_dict.Add(i, sPath + '/' + fileInfo.Name);
                        i++;

                    }
                    
                }
                

            }

            main_photo.Source = new BitmapImage(new Uri(photo_dict[0]));

            first_photo.Source = new BitmapImage(new Uri(photo_dict[0]));
            second_photo.Source = new BitmapImage(new Uri(photo_dict[1]));
            third_photo.Source = new BitmapImage(new Uri(photo_dict[2]));
            fourth_photo.Source = new BitmapImage(new Uri(photo_dict[3]));
            fifth_photo.Source = new BitmapImage(new Uri(photo_dict[4]));
            sixth_photo.Source = new BitmapImage(new Uri(photo_dict[5]));


        }

            private void Back_Click(object sender, RoutedEventArgs e)
        {
            Navigation_Page navigation_page = new Navigation_Page();

            this.NavigationService.Navigate(navigation_page);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
        
        }

    }
}
