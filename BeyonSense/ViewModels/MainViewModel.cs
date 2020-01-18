using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WinForms = System.Windows.Forms;


namespace BeyonSense.ViewModels
{
    /// <summary>
    /// The view model for the applications main Directory view
    /// </summary>
    

    public class MainViewModel : Screen
    {

        /// <summary>
        /// A list of all directories on the machine
        /// </summary>
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        #region TEST
        
        private Color color;

        public Color Colour
        {
            get { return color; }
            set { color = value; }
        }


        #endregion

        #region Bitmap image paths

        #region Bitmap clickable bool

        private bool clickableImage = false;
        public bool ClickableImage
        {
            get { return clickableImage; }
            set
            {
                clickableImage = value;
                NotifyOfPropertyChange(() => ClickableImage);
            }
        }
        #endregion

        #region Bitmap path 1

        private string bmpPath1 = "/Pictures/no_image.png";
        public string BmpPath1
        {
            get { return bmpPath1; }
            set { 
                bmpPath1 = value;
                NotifyOfPropertyChange(() => BmpPath1);
            }
        }
        #endregion

        #region Bitmap path 2
        private string bmpPath2 = "/Pictures/no_image.png";
        public string BmpPath2
        {
            get { return bmpPath2; }
            set
            {
                bmpPath2 = value;
                NotifyOfPropertyChange(() => BmpPath2);
            }
        }
        #endregion

        #region Bitmap path 3
        private string bmpPath3 = "/Pictures/no_image.png";
        public string BmpPath3
        {
            get { return bmpPath3; }
            set
            {
                bmpPath3 = value;
                NotifyOfPropertyChange(() => BmpPath3);
            }
        }
        #endregion

        #region Bitmap path 4
        private string bmpPath4 = "/Pictures/no_image.png";
        public string BmpPath4
        {
            get { return bmpPath4; }
            set
            {
                bmpPath4 = value;
                NotifyOfPropertyChange(() => BmpPath4);
            }
        }
        #endregion

        #region Bitmap path 5
        private string bmpPath5 = "/Pictures/no_image.png";
        public string BmpPath5
        {
            get { return bmpPath5; }
            set
            {
                bmpPath5 = value;
                NotifyOfPropertyChange(() => BmpPath5);
            }
        }
        #endregion

        #region Bitmap path 6
        private string bmpPath6 = "/Pictures/no_image.png";
        public string BmpPath6
        {
            get { return bmpPath6; }
            set
            {
                bmpPath6 = value;
                NotifyOfPropertyChange(() => BmpPath6);
            }
        }
        #endregion

        #endregion

        #region Main Bitmap image path

        private string mainBmpImage = "/Pictures/no_image.png";

        public string MainBmpImage
        {
            get { return mainBmpImage; }
            set
            {
                mainBmpImage = value;
                NotifyOfPropertyChange(() => MainBmpImage);
            }
        }

        #endregion

        #region Color generator function

        /// <summary>
        /// This function can generate n colors
        /// </summary>
        /// <param name="n"> The number of colors</param>
        /// <returns> int 2-dimension array N x 3 </returns>
        public List<Color> ColorGenerator(int n)
        {

            int[,] basic_color = new int[8, 3] { { 0, 0, 0 }, { 255, 0, 0 }, { 0, 255, 0 },
                                                { 0, 0, 255 }, { 255, 255, 0 }, { 0, 255, 255 },
                                                { 255, 0, 255 }, { 255, 255, 255 } };

            int[,] generator_color = new int[n, 3];


            #region Less than 8
            if (n <= 8)
            {

                for (int i = 0; i < n; i++)
                {
                    generator_color[i, 0] = basic_color[i, 0];
                    generator_color[i, 1] = basic_color[i, 1];
                    generator_color[i, 2] = basic_color[i, 2];

                }
            }
            #endregion

            #region eqaul to or more than 8
            else
            {
                // Add basis colors which are eight colors
                for (int i = 0; i < 8; i++)
                {
                    generator_color[i, 0] = basic_color[i, 0];
                    generator_color[i, 1] = basic_color[i, 1];
                    generator_color[i, 2] = basic_color[i, 2];
                }

                // New color is needed to be generated
                for (int i = 0; i < n - 8; i++)
                {
                    Random random = new Random();

                    // Choose two basic color to be mixed
                    int index1 = random.Next(0, 8 + i);
                    int index2 = random.Next(0, 8 + i);

                    // Allocate new R, G, B value
                    generator_color[i + 8, 0] = (int)(generator_color[index1, 0] + generator_color[index2, 0]) / 2;
                    generator_color[i + 8, 1] = (int)(generator_color[index1, 1] + generator_color[index2, 1]) / 2;
                    generator_color[i + 8, 2] = (int)(generator_color[index1, 2] + generator_color[index2, 2]) / 2;
                }
            }

            #endregion


            List<Color> random_color = new List<Color>();
            
            for (int i = 0; i < n; i++)
            {

                byte r = BitConverter.GetBytes(generator_color[i, 0])[0];
                byte g = BitConverter.GetBytes(generator_color[i, 1])[0];
                byte b = BitConverter.GetBytes(generator_color[i, 2])[0];

                random_color.Add(Color.FromRgb(r, g, b));

            }

            return random_color;
        }

        #endregion

        private string rootPath;

        public string RootPath
        {
            get { return rootPath; }
            set
            {
                rootPath = value;

                // rootPath is not null or empty
                if (!String.IsNullOrEmpty(rootPath))
                {
                    StartDirectoryTree(rootPath);
                }
            }
        }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {

            #region ColorBox Test
            /// TODO: THIS LINES WILL BE REMOVED.
            /// THESE LINES ARE ONLY FOR TEST
            List<Color> new_color = ColorGenerator(10);
            this.Colour = new_color[9];
            ///
            #endregion

        }

        #endregion

        public void StartDirectoryTree(string path)
        {
            #region Directory TreeView
            // Get the logical drives
            var children = DirectoryStructure.GetRootFolder(path);

            // Create the view models from the data
            this.Items = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(root => new DirectoryItemViewModel(root.FullPath, DirectoryItemType.Folder)));

            #endregion
        }


        // open button

        public void Open()
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
                RootPath = folderDialog.SelectedPath;

            }
        }


        // stackpanel click event handler
        public void TreeElementMouseDown(string path)
        {
            Console.WriteLine(path);

            #region Check if the path is for a file or not

            FileAttributes attr = File.GetAttributes(path);

            // The path is a directory path
            if (attr.HasFlag(FileAttributes.Directory))

                return;

            // The path is a directory path
            else
            //MessageBox.Show("Its a file");

            #endregion

            #region Get parent directory path

            // 2. If yes, get parent directory path

            // If we have no path, return empty
            if (string.IsNullOrEmpty(path))
                return;

            // Make all slashes back slashes
            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return ;

            var dirPath = normalizedPath.Substring(0,lastIndex);


            #endregion

            #region Check files: 6 bitmap images, (Optionally 1 csv file)
            // 3. Check the directory has 6 bmp files, and optionally one csv file

            // Travere all the files in the dirPath
            int num_Files = 0;
            int num_bmp = 0;
            int num_csv = 0;
            string csvPath = "";
            List<string> BmpList = new List<string>();

            DirectoryInfo folder = new DirectoryInfo(dirPath);
            if (folder.Exists)
            {
                
                //loop for files in the folder
                foreach (FileInfo fileInfo in folder.GetFiles())
                {
                    //each file path

                    // Check exetension of each file or folder: using GetExtension(string)
                    string filePath = dirPath + '\\' + fileInfo.Name;
                    string exetension = Path.GetExtension(filePath);

                    // Count the number of bitmap image and csv file

                    

                    switch (exetension)
                    {
                        case ".bmp":
                            BmpList.Add(filePath);
                            num_bmp++;
                            break;

                        case ".csv":
                            csvPath = filePath;
                            num_csv++;
                            break;

                        default:
                            break;

                    }

                    num_Files++;

                }
                 
                //Error message to choose correct directory again: six bitmap images, an optional csv file
                if (num_bmp != 6 || num_csv > 1)
                {
                    MessageBox.Show("Wrong Directory\nPleae make sure the folder has six bitmap images and an optional csv file.");
                    return;
                }

                else
                {
                    ClickableImage = true;
                    // If six bitmap images, none or one csv file exist, save all the paths in public variables (csv file path -> local variables)
                    BmpPath1 = BmpList[0];
                    BmpPath2 = BmpList[1];
                    BmpPath3 = BmpList[2];
                    BmpPath4 = BmpList[3];
                    BmpPath5 = BmpList[4];
                    BmpPath6 = BmpList[5];

                    MainBmpImage = BmpList[0];
                    //MessageBox.Show("bmp: " + num_bmp.ToString() + "\ncsv: " +  num_csv.ToString() + "\nTotal: "+ num_Files.ToString());
                }
            }


            


            #endregion

            // 4. Change the public picture path list values -> binding to thumbnails



            // 5. If there is a csv file, change the public value of csv file

            // 5-1. Update table items

            // 5-2. Generate colors as many as the number of table elements

        }

        public void PopupMainImage(string path)
        {
            //MessageBox.Show(path);

            MainBmpImage = path;
        }
    }
}
