using BeyonSense.Models;
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
    /// The View Model for the Main View
    /// </summary>
    

    public class MainViewModel : Screen
    {

        #region A list of all directories
        // A list of all directories from a selected directory
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        #endregion

        #region Color variable

        private Color color = Color.FromArgb(0, 255, 255, 255);

        public Color Colour
        {
            get { return color; }
            set { color = value; }
        }

        #endregion

        #region The number of points variable

        private string numPoints;
        
        public string NumPoints
        {
            get { return numPoints; }
            set
            {
                numPoints = value;
                NotifyOfPropertyChange(() => numPoints);
            }
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

        #region Labeling Class Info

        #region Csv Path variables
        private string csvPath;

        public string CsvPath
        {
            get { return csvPath; }
            set
            {
                csvPath = value;

                if (!String.IsNullOrEmpty(csvPath))
                {
                    // Read csv file if there is a csv file in the selecte folder
                    CsvLoader(csvPath);
                }

                else
                {
                    // Reset the table if there is no csv file in the selected folder
                    ClassPoints.Clear();
                }

            }
        }
        #endregion

        #region ClassInfo Collection
        // View is needed to be refreshed if we use List<T>, so we used ObservableCollection<T> instead of List<T>

        private ObservableCollection<ClassInfo> classPoints = new ObservableCollection<ClassInfo>();

        public ObservableCollection<ClassInfo> ClassPoints
        {
            get{ return classPoints; }
            set { 
                classPoints = value;
                NotifyOfPropertyChange(() => ClassPoints);
            }
        }
        #endregion

        #endregion

        #region Selected Row

        private ClassInfo selectedRow;

        public ClassInfo SelectedRow {
            get { return selectedRow; }

            set 
            { 
                selectedRow = value;

                if (selectedRow != null)
                {
                    Colour = selectedRow.TextColor;
                    NumPoints = selectedRow.NumPoints.ToString();
                }
                else
                {
                    Colour = Color.FromArgb(0, 255, 255, 255);
                    NumPoints = "";
                }
                NotifyOfPropertyChange(() => SelectedRow);

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

                random_color.Add(Color.FromArgb(255, r, g, b));

            }

            return random_color;
        }

        #endregion

        #region Selected Root Path
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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {

        }

        #endregion

        #region Open Button Event Handler: Folder Explorer
        /// <summary>
        /// Show a folder explorer when the open button is clicked
        /// </summary>
        public void Open()
        {
            // Folder dialog
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;

            // Selected Path
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WinForms.DialogResult result = folderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //Selected Folder Path
                RootPath = folderDialog.SelectedPath;
            }
        }

        #endregion

        #region Show Directory Tree on MainView
        /// <summary>
        /// Show directory tree from the selected directory
        /// </summary>
        /// <param name="path">Selected path from folder explorer</param>
        public void StartDirectoryTree(string path)
        {
            #region Directory TreeView
            // Get the selected directory path
            var children = DirectoryStructure.GetRootFolder(path);

            // Create the view models from the data
            this.Items = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(root => new DirectoryItemViewModel(root.FullPath, DirectoryItemType.Folder)));

            #endregion
        }
        #endregion

        #region Tree Element Click Event Handler

        /// <summary>
        /// Get a selecte file path when a stackpanel of each treeview item is clicked
        /// </summary>
        /// <param name="path">a path of selected file</param>
        /// 
        public void TreeElementMouseDown(string path)
        {
            Console.WriteLine(path);

            #region Check if the path is for a file or not

            FileAttributes attr = File.GetAttributes(path);

            // The path is a directory path
            if (attr.HasFlag(FileAttributes.Directory))

                return;

            else
            //MessageBox.Show("Its a file");

            #endregion

            #region Get parent directory path

            // If the paht is a file path, get parent directory path

            // Exception: If we have no path, return empty
            if (string.IsNullOrEmpty(path))
                return;

            // Make all slashes back slashes
            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return ;

            //  Remove file name from the file path so we can get a parent directory path
            var dirPath = normalizedPath.Substring(0,lastIndex);

            #endregion

            #region Check the number of files: 6 bitmap images, (Optionally 1 csv file)
            
            // Check the directory has 6 bmp files, and optionally one csv file

            int num_Files = 0;
            int num_bmp = 0;
            int num_csv = 0;
            string _csvPath = "";
            List<string> BmpList = new List<string>();

            // Travere all the files in the dirPath
            DirectoryInfo folder = new DirectoryInfo(dirPath);
            if (folder.Exists)
            {
                
                //loop for files in the folder
                foreach (FileInfo fileInfo in folder.GetFiles())
                {
                    //each file path
                    string filePath = dirPath + '\\' + fileInfo.Name;
                    
                    // Check exetension of each file or folder: using GetExtension(string str)
                    string exetension = Path.GetExtension(filePath);

                    // Count the number of bitmap image and csv file
                    switch (exetension)
                    {
                        case ".bmp":
                            BmpList.Add(filePath);
                            num_bmp++;
                            break;

                        case ".csv":
                            // Save a csv file path to the local variable
                            _csvPath = filePath;
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
                    //MessageBox.Show(csvPath);
                    
                    // Enable clickable thumbnails when bitmap images are successfully loaded.
                    ClickableImage = true;

                    // If six bitmap images, none or one csv file exist, save all the paths in public variables
                    // Change the public picture bitmap images path values to bind image sources for six thumbnails
                    BmpPath1 = BmpList[0];
                    BmpPath2 = BmpList[1];
                    BmpPath3 = BmpList[2];
                    BmpPath4 = BmpList[3];
                    BmpPath5 = BmpList[4];
                    BmpPath6 = BmpList[5];

                    // Automatically, show first bitmap image as a main image
                    MainBmpImage = BmpList[0];

                    // Set public variable CsvPath as the csv file path
                    CsvPath = _csvPath;

                    //MessageBox.Show("bmp: " + num_bmp.ToString() + "\ncsv: " +  num_csv.ToString() + "\nTotal: "+ num_Files.ToString());
                }
            }
            #endregion

        }

        #endregion

        #region Thumbnail Click Event Handler
        /// <summary>
        /// Change main image when a thumbnail is clicked
        /// </summary>
        /// <param name="path"></param>
        public void PopupMainImage(string path)
        {
            //MessageBox.Show(path);
            MainBmpImage = path;
        }
        #endregion

        #region Csv Loader

        public void CsvLoader(string path)
        {
            int numElements = 0;

            // TEST: Assume there are four classes in the csv file
            numElements = 4;

            // Generate colors as many as the number of table elements
            List<Color> textColor = ColorGenerator(numElements);

            ObservableCollection<ClassInfo> DummyList = new ObservableCollection<ClassInfo>();

            for (int i = 0; i < numElements; i++)
            {
                DummyList.Add(new ClassInfo() { ClassName = "Red", TextColor = textColor[i] });
                DummyList[i].PointCalculator();
            }

            // Update table items
            ClassPoints = DummyList;

        }
        #endregion

    }
}
