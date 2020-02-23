using BeyonSense.Models;
using BeyonSense.Views;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinForms = System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using BeyonSense.Converters;
using Emgu.CV.ML;
using System.Diagnostics;

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

        #region Default Image

        const string DefaultImagePath = "/Pictures/no_image.png";

        #endregion

        #region Bitmap path 1

        private string bmpPath1 = DefaultImagePath;
        public string BmpPath1
        {
            get { return bmpPath1; }
            set
            {
                bmpPath1 = value;
                NotifyOfPropertyChange(() => BmpPath1);

                // Initialize new class data for data integrity
                newLabelName = "";
                ClickedPosition.Clear();
                newLabelColor = Colors.Transparent;
                DrawLayer.Clear();

                // Reset boolean variables
                OKBool = false;
                ImageBool = false;
                CanBeReverted = false;

            }
        }
        #endregion

        #region Bitmap path 2
        private string bmpPath2 = DefaultImagePath;
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
        private string bmpPath3 = DefaultImagePath;
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
        private string bmpPath4 = DefaultImagePath;
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
        private string bmpPath5 = DefaultImagePath;
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
        private string bmpPath6 = DefaultImagePath;
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

        #region Bitmap image format: 608x 800

        private const int BmpHeight = 608;
        private const int BmpWidth = 800;

        #endregion

        #region Main Bitmap image source


        #region Initialize default image
        /// <summary>
        /// Return bitmap source of default image
        /// </summary>
        /// <returns>Bitmap source of default image </returns>
        private BitmapSource DefaultImageSource()
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("pack://application:,,," + DefaultImagePath);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            return src;
        }
        #endregion

        private BitmapSource mainBmpImage;

        public BitmapSource MainBmpImage
        {
            get { return mainBmpImage; }
            set
            {
                mainBmpImage = value;
                NotifyOfPropertyChange(() => MainBmpImage);
            }
        }

        private BitmapSource overlayImage = BitmapSourceConvert.ToBitmapSource(
            new Image<Bgra, byte>(BmpWidth, BmpHeight, new Bgra(255, 255, 255, 0)));

        public BitmapSource OverlayImage
        {
            get { return overlayImage; }
            set
            {
                overlayImage = value;
                NotifyOfPropertyChange(() => OverlayImage);
            }
        }

        #endregion

        #region Selected File Path by File Explorer

        private string modelPath;

        public string ModelPath
        {
            get { return modelPath; }
            set
            {
                modelPath = value;
                NotifyOfPropertyChange(() => ModelPath);
                ColorFilters.Clear();
            }
        }
        #endregion

        #region Toggle Boolean Variable

        private bool toggleBool = false;

        public bool ToggleBool
        {
            get { return toggleBool; }
            set
            {
                toggleBool = value;
                NotifyOfPropertyChange(() => ToggleBool);

                // Inference mode
                if (toggleBool)
                {
                    // Reset OverlayImage
                    OverlayImage = BitmapSourceConvert.ToBitmapSource(new Image<Bgra, byte>(BmpWidth, BmpHeight, new Bgra(255, 255, 255, 0)));

                    // Show color filter
                    OverlayImage = ColorFilters[GetParentDirPath()];

                    // Disable to draw new label
                    PlusBool = false;

                }

                // Not inference mode
                else
                {
                    // Reset OverlayImage
                    OverlayImage = BitmapSourceConvert.ToBitmapSource(new Image<Bgra, byte>(BmpWidth, BmpHeight, new Bgra(255, 255, 255, 0)));
                    DrawLabel();

                    // Enable to draw new label
                    PlusBool = true;
                }
            }
        }

        private bool toggleIsEnable = false;
        public bool ToggleIsEnable
        {
            get { return toggleIsEnable; }
            set
            {
                toggleIsEnable = value;
                NotifyOfPropertyChange(() => ToggleIsEnable);
            }
        }
        #endregion

        #region Color generator functions

        /// <summary>
        /// Change int[3]{r, g, b} to Color(r, g, b)
        /// </summary>
        /// <param name="arr">int[3]{r, g, b}</param>
        /// <returns>Color(r, g, b)</returns>
        private Color IntArrayToColor(int[] arr)
        {

            byte r = BitConverter.GetBytes(arr[0])[0];
            byte g = BitConverter.GetBytes(arr[1])[0];
            byte b = BitConverter.GetBytes(arr[2])[0];

            return Color.FromArgb(255, r, g, b);
        }

        /// <summary>
        /// Make new colors 
        /// </summary>
        /// <param name="n">How many new colors are needed</param>
        /// <returns>New color list</returns>
        public List<Color> AddColors(int n)
        {


            // The number of basic color
            const int numBasicColor = 6;

            // Define basic color
            int[,] BasicColor = new int[numBasicColor, 3] { { 255, 0, 0 }, { 0, 255, 0 }, { 0, 0, 255 },
                                                            { 255, 255, 0 }, { 0, 255, 255 }, { 255, 0, 255 }};


            // Used colors list
            List<Color> UsedColors = new List<Color>();

            // New colors list which is return variable
            List<Color> NewColors = new List<Color>();

            // The number of classes with actual color
            int count = 0;

            // Allocate used color in the list
            for (int i = 0; i < ClassPoints.Count; i++)
            {
                if (ClassPoints[i].ClassColor != Colors.Transparent)
                {
                    UsedColors.Add(ClassPoints[i].ClassColor);
                    count++;
                }
            }


            // If there is any not used basic colors, allocate them in new color list
            for (int i = numBasicColor - count; i > 0; i--)
            {
                if (NewColors.Count == n)
                {
                    return NewColors;
                }

                int[] arr = new int[3] {BasicColor[numBasicColor - i, 0],
                                        BasicColor[numBasicColor - i, 1],
                                        BasicColor[numBasicColor - i, 2] };

                NewColors.Add(IntArrayToColor(arr));


            }

            // Allocate new colors if there is no basic color we can use
            Random rnd = new Random();

            Color newColor;

            while (true)
            {

                if (NewColors.Count == n) break;

                int[] arr = new int[3] { rnd.Next(255) ,
                                        rnd.Next(255) ,
                                        rnd.Next(255) };

                newColor = IntArrayToColor(arr);

                // For vivid colors becuase #FFFFFF equals to Colors.Transparent
                if (arr[0] + arr[1] + arr[2] < 750)
                {
                    // If new color is not in the used color list
                    if (!UsedColors.Contains(newColor))
                    {
                        UsedColors.Add(newColor);
                        NewColors.Add(newColor);
                    }
                }

            }

            return NewColors;
        }

        #endregion

        #region Selected Root Path from Folder Explorer

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

                    #region Initialize variables for each project

                    // Image reset
                    ResetImages();

                    // Dictionary Clear
                    CornerPoint.Clear();

                    // ObservableCollection<ClassPoint> Clear
                    ClassPoints.Clear();

                    // Color, NumPoints Clear
                    Colour = Color.FromArgb(0, 255, 255, 255);
                    NumPoints = "";
                    CsvFilePaths.Clear();

                    // Plus Button Reset
                    PlusBool = false;
                    #endregion

                    // Button Reset
                    SaveBool = false;
                    TrainBool = false;
                    OKBool = false;

                    // ?????toggle button false 로 바꿔야 함!
                    ToggleBool = false;

                    ToggleIsEnable = false;
                    ModelPath = "";

                    // Initialize color filters
                    ColorFilters.Clear();

                    // Calculate numPoints and update table elements
                    PointCalculator(rootPath);
                }
            }
        }

        #endregion

        #region Default Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            MainBmpImage = DefaultImageSource();
        }

        #endregion

        #region Open Button Event Handler: Folder Explorer
        /// <summary>
        /// Show a folder explorer when the open button is clicked
        /// </summary>
        public void Open()
        {
            // Folder dialog
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog
            {
                ShowNewFolderButton = false,

                // Selected Path
                SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory
            };
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
        private void StartDirectoryTree(string path)
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
            // MessageBox.Show("Its a file");

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
                return;

            //  Remove file name from the file path so we can get a parent directory path
            var dirPath = normalizedPath.Substring(0, lastIndex);

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

                // Loop for files in the folder
                foreach (FileInfo fileInfo in folder.GetFiles())
                {
                    // Each file path
                    string filePath = dirPath + '\\' + fileInfo.Name;

                    // Check exetension of each file or folder: using GetExtension(string str)
                    string exetension = System.IO.Path.GetExtension(filePath);

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

                        case ".bin":
                            // raw ata 
                            return;

                        default:
                            break;

                    }

                    num_Files++;

                }

                // Error message to choose correct directory again: six bitmap images, an optional csv file
                if (num_bmp != 6 || num_csv > 1)
                {
                    ResetImages();
                    Items.Clear();
                    // Disable to load a model
                    FileExplorerBool = false;

                    MessageBox.Show("Wrong Directory\nPleae make sure the folder has six bitmap images and an optional csv file.");

                    return;
                }

                else
                {
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
                    MainBmpImage = StringToBmpSource(BmpList[0]);
                    PlusBool = true;

                    FileExplorerBool = true;

                    // Set public variable CsvPath as the csv file path
                    CsvPath = _csvPath;


                    // Not inference mode
                    if (!ToggleBool)
                    {
                        DrawLabel();
                    }

                    // Inference mode; overlay color filter
                    else
                    {
                        OverlayImage = ColorFilters[GetParentDirPath()];
                    }

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

            // Binding clicked image
            MainBmpImage = StringToBmpSource(path);

            // Not inference mode
            if (!ToggleBool)
            {
                // Draw lines over image
                DrawLabel();
            }

            // Inference mode; overlay color filter
            else
            {
                OverlayImage = ColorFilters[GetParentDirPath()];
            }


            // Disable to click main image
            ImageBool = false;


            // Initialize new class data for data integrity
            newLabelName = "";
            ClickedPosition.Clear();
            newLabelColor = Colors.Transparent;
            DrawLayer.Clear();

            // Reset boolean variables
            OKBool = false;
            ImageBool = false;
            CanBeReverted = false;
            PlusBool = true;

        }
        #endregion

        #region Open Button Click Handler: Parameter Dictionary File Explorer
        /// <summary>
        ///  Open file explorer
        /// </summary>
        public void FileOpen()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Search file filter by exetension
                Filter = "Model (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Get full path of the selected file
                ModelPath = openFileDialog.FileName;

                // Path is not null or empty
                if (!String.IsNullOrEmpty(ModelPath))
                {
                    // Load a model
                    ColorFilterGenerator();
                    // ToggleIsEnable = true;
                }
                else
                {
                    //ToggleIsEnable = false;
                }
            }
        }
        #endregion

        #region Toggle Click Event Handler

        public void ToggleClick()
        {
            // Inference mode
            if (!ToggleBool)
            {
                ToggleBool = true;
                // Reset OverlayImage
                OverlayImage = BitmapSourceConvert.ToBitmapSource(new Image<Bgra, byte>(BmpWidth, BmpHeight, new Bgra(255, 255, 255, 0)));

                // Show color filter
                OverlayImage = ColorFilters[GetParentDirPath()];

                // Disable to draw new label
                PlusBool = false;

            }

            // Not inference mode
            else
            {
                ToggleBool = false;

                // Reset OverlayImage
                OverlayImage = BitmapSourceConvert.ToBitmapSource(new Image<Bgra, byte>(BmpWidth, BmpHeight, new Bgra(255, 255, 255, 0)));
                DrawLabel();

                // Enable to draw new label
                PlusBool = true;
            }
        }

        #endregion

        #region Csv path variable to make boundaray
        /// <summary>
        /// A selected csv file path
        /// </summary>
        private string csvPath;

        public string CsvPath
        {
            get { return csvPath; }
            set
            {
                csvPath = value;
            }
        }
        #endregion

        #region Selected Row

        /// <summary>
        /// A selected row in the table on the MainView
        /// </summary>
        private ClassPixels selectedRow;

        public ClassPixels SelectedRow
        {
            get { return selectedRow; }

            set
            {
                selectedRow = value;

                if (selectedRow != null)
                {
                    // Binding selected color and numPoints
                    Colour = selectedRow.ClassColor;
                    NumPoints = selectedRow.NumPoints.ToString();
                }
                else
                {
                    // Reset color and numPoints
                    Colour = Color.FromArgb(0, 255, 255, 255);
                    NumPoints = "";
                }

                NotifyOfPropertyChange(() => SelectedRow);

            }
        }

        #region Color variable for Color Box

        // Initial Value is Transparent
        private Color color = Color.FromArgb(0, 255, 255, 255);

        public Color Colour
        {
            get { return color; }
            set { color = value; }
        }

        #endregion


        #region The number of points variable for TextBlock

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


        #endregion

        #region ClassInfo Collection
        // View is needed to be refreshed if we use List<T>, so we used ObservableCollection<T> instead of List<T>

        #region Table Item Source

        private ObservableCollection<ClassPixels> classPoints = new ObservableCollection<ClassPixels>();
        public ObservableCollection<ClassPixels> ClassPoints
        {
            get { return classPoints; }
            set
            {
                classPoints = value;
                NotifyOfPropertyChange(() => ClassPoints);
            }
        }
        #endregion

        #region Dictionary<csv path, Collection<ClassCornerPoints>> for LoadBoudary()
        /// <summary>
        /// Managing point position for each csv file
        /// </summary>
        public Dictionary<string, ObservableCollection<ClassCornerPoints>> CornerPoint = new Dictionary<string, ObservableCollection<ClassCornerPoints>>();
        #endregion

        #endregion

        #region All Csv File Paths
        // Every csv file path under the selected folder
        private List<string> CsvFilePaths { get; set; } = new List<string>();
        #endregion

        #region Reset All images

        private void ResetImages()
        {
            #region Reset all the images
            BmpPath1 = DefaultImagePath;
            BmpPath2 = DefaultImagePath;
            BmpPath3 = DefaultImagePath;
            BmpPath4 = DefaultImagePath;
            BmpPath5 = DefaultImagePath;
            BmpPath6 = DefaultImagePath;


            MainBmpImage = DefaultImageSource();

            // Reset Overlay Image
            OverlayImage = BitmapSourceConvert.ToBitmapSource(new Image<Bgra, byte>(BmpWidth, BmpHeight, new Bgra(255, 255, 255, 0)));


            #endregion

        }
        #endregion

        #region Walking Directory
        /// <summary>
        /// Get every csv file paths and add into csvFilePaths variable
        /// </summary>
        /// <param name="sDir">string _rootDir</param>

        // Warning variables preventing to open deep directory tree
        //private int recursiveCount = 0;
        private bool recursiveAlert = false;
        private int bmpcount = 0;

        private void DirSearch(string sDir, int cnt = 0)
        {
            if (cnt > 1)
            {
                if (!recursiveAlert)
                {
                    Items.Clear();
                    ResetImages();
                    FileExplorerBool = false;
                    MessageBox.Show("Please make sure that you select a correct project folder.");

                    recursiveAlert = true;

                    return;
                }
                else
                {
                    return;
                }
            }

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {

                    foreach (string f in Directory.GetFiles(d))
                    {
                        // Check it file exetension is csv or not
                        if (System.IO.Path.GetExtension(f) == ".csv")
                        {
                            // Add csv file path in the l`ist
                            CsvFilePaths.Add(f);
                        }

                        if (System.IO.Path.GetExtension(f) == ".bmp")
                        {
                            bmpcount++;
                        }
                    }
                    DirSearch(d, cnt + 1);
                }
            }

            // In case of stack overflow
            catch (System.Exception excpt)
            {
                Console.WriteLine("DirSearch Exception: " + excpt);
                MessageBox.Show("Please choose a correct project folder");
            }

        }
        #endregion

        #region Convert String into Integer
        /// <summary>
        /// Converter: String to Integer
        /// </summary>
        /// <param name="str">string num</param>
        /// <returns>int num</returns>
        private int StrToInt(string str)
        {

            if (!Int32.TryParse(str, out int i))
            {
                i = -1;
            }
            return i;
        }

        #endregion

        #region PixelCalculator
        /// <summary>
        /// Calculate inside pixels
        /// </summary>
        /// <param name="_cornerPoint">Corner Point Collection</param>
        /// <returns>int the number of pixel</returns>

        private int PixelCalculator(List<int[]> _cornerPoint)
        {
            #region Calculation

            int num_pixel = 0;
            int num_point = _cornerPoint.Count;
            int[,] points = new int[num_point, 2];

            // Change array into list
            for (int i = 0; i < num_point; i++)
            {
                points[i, 0] = _cornerPoint[i][1];
                points[i, 1] = _cornerPoint[i][0];
            }

            // Calculate range of y value
            int min = points[0, 0];
            int max = 0;

            for (int i = 0; i < num_point; i++)
            {
                if (points[i, 0] < min)
                {
                    min = points[i, 0];
                }

                if (points[i, 0] > max)
                {
                    max = points[i, 0];
                }
            }

            // Initialize list for each y values along y axis
            List<List<int>> boundary = new List<List<int>>();
            for (int i = 0; i < max - min + 1; i++)
            {
                boundary.Add(new List<int>());
            }

            // Exclude sharp point from boudary
            if ((points[num_point - 1, 0] - points[0, 0]) * (points[1, 0] - points[0, 0]) <= 0)
            {
                boundary[points[0, 0] - min].Add(points[0, 1]);
            }

            else
            {
                num_pixel += 1;
            }

            for (int i = 1; i < num_point - 1; i++)
            {
                if ((points[i - 1, 0] - points[i, 0]) * (points[i + 1, 0] - points[i, 0]) <= 0)
                {
                    boundary[points[i, 0] - min].Add(points[i, 1]);
                }

                else
                {
                    num_pixel += 1;
                }
            }

            if ((points[0, 0] - points[num_point - 1, 0]) * (points[num_point - 2, 0] - points[num_point - 1, 0]) <= 0)
            {
                boundary[points[num_point - 1, 0] - min].Add(points[num_point - 1, 1]);
            }

            else
            {
                num_pixel += 1;
            }

            // Calcuate boudary positions
            for (int i = 0; i < num_point - 1; i++)
            {
                if (points[i + 1, 0] == points[i, 0])
                {
                    num_pixel += 0;
                }

                else if (points[i + 1, 0] - points[i, 0] > 0)
                {
                    for (int j = 1; j < points[i + 1, 0] - points[i, 0]; j++)
                    {
                        boundary[points[i, 0] - min + j].Add(points[i, 1] + (j * (points[i + 1, 1] - points[i, 1])) / (points[i + 1, 0] - points[i, 0]));
                    }
                }

                else //points[i+1,0] - points[i,0] < 0
                {
                    for (int j = 1; j < points[i, 0] - points[i + 1, 0]; j++)
                    {
                        boundary[points[i, 0] - min - j].Add(points[i, 1] + (j * (points[i + 1, 1] - points[i, 1])) / (points[i, 0] - points[i + 1, 0]));
                    }
                }
            }


            // Compare the first element to the last one
            if (points[num_point - 1, 0] == points[0, 0])
            {
                num_pixel += 0;
            }

            else if (points[0, 0] - points[num_point - 1, 0] > 0)
            {
                for (int j = 1; j < points[0, 0] - points[num_point - 1, 0]; j++)
                {
                    boundary[points[num_point - 1, 0] - min + j].Add(points[num_point - 1, 1] + (j * (points[0, 1] - points[num_point - 1, 1])) / (points[0, 0] - points[num_point - 1, 0]));
                }
            }

            else //points[i+1,0] - points[i,0] < 0
            {
                for (int j = 1; j < points[num_point - 1, 0] - points[0, 0]; j++)
                {
                    boundary[points[num_point - 1, 0] - min - j].Add(points[num_point - 1, 1] + (j * (points[0, 1] - points[num_point - 1, 1])) / (points[num_point - 1, 0] - points[0, 0]));
                }
            }

            #endregion



            // Calculate the number of pixels and write the pixel value to binary file
            for (int i = 0; i < max - min + 1; i++)
            {
                if (boundary[i].Count == 0)
                {
                    num_pixel += 0;
                }
                else
                {
                    boundary[i].Sort();
                    bool inner = false;
                    for (int j = boundary[i].Min(); j < boundary[i].Max() + 1; j++)
                    {
                        if (boundary[i].Contains(j))
                        {
                            inner = !inner;
                        }
                        if (inner)
                        {
                            num_pixel += 1;

                        }

                    }
                }

            }

            return num_pixel;
        }
        #endregion

        #region Point Calculator
        /// <summary>
        /// Make ClassPoints Collection and Corner Points Dictionary
        /// </summary>
        /// <param name="_rootPath">A selected path by folder explorer</param>
        /// [This method is called when a folder is selected by folder explorer]
        private void PointCalculator(string _rootPath)
        {
            // Traverse all the directory and find all existing csv file paths
            DirSearch(_rootPath);

            // The folder has to have at least six Bmp files
            if (bmpcount / 6 > 0 && bmpcount % 6 == 0)
            {
                // Read csv file only if Dirsearch is successfully completed
                if (!recursiveAlert)
                {
                    // For each csv file
                    foreach (string _path in CsvFilePaths)
                    {
                        // Read class name and corner points
                        ObservableCollection<ClassCornerPoints> _classCorners = new ObservableCollection<ClassCornerPoints>();

                        #region Read Csv Format
                        // Read csv file
                        using (var reader = new StreamReader(_path))
                        {
                            int _numLine = 0;
                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();

                                if (_numLine != 0)
                                {
                                    string _className = "";
                                    List<int[]> _cornerPoints = new List<int[]>();

                                    var values = line.Split(',');

                                    _className = values[0];
                                    //Console.WriteLine(values[0]);

                                    for (int i = 1; i < values.Length; i += 2)
                                    {

                                        int[] _position = new int[2];
                                        // x position
                                        _position[0] = StrToInt(values[i]);

                                        // y position
                                        try
                                        {
                                            _position[1] = StrToInt(values[i + 1]);
                                        }

                                        // Exception handler: Wrong csv format
                                        catch (IndexOutOfRangeException e)
                                        {
                                            MessageBox.Show("Your csv files might have wrong format.\n");
                                            ClassPoints.Clear();
                                            Console.WriteLine("CSV Fromat Error: " + e);
                                            return;
                                        }

                                        //Console.WriteLine("x: " + values[i] + " y: " + values[i + 1] + '\n');
                                        _cornerPoints.Add(_position);
                                    }

                                    // Each line = each class
                                    _classCorners.Add(new ClassCornerPoints() { ClassName = _className, Points = _cornerPoints });
                                }
                                _numLine++;
                            }
                        }
                        #endregion

                        #region Add data to Dictionary
                        // Allocate new element into the dictionary for each csv file
                        CornerPoint.Add(_path, _classCorners);
                        #endregion

                        #region Add data to Item Source
                        // Calculate the number of inside points and Add ClassPoints 
                        for (int i = 0; i < _classCorners.Count; i++)
                        {
                            // Check if ClassPoints has same class name as _classCorners[i].ClassName
                            int ack = 0;

                            for (int j = 0; j < ClassPoints.Count; j++)
                            {
                                if (_classCorners[i].ClassName == ClassPoints[j].ClassName)
                                {
                                    // If there is same class name, add the value
                                    ack = 1;
                                    ClassPoints[j].NumPoints += PixelCalculator(_classCorners[i].Points);
                                }
                            }

                            // New class
                            // If not, make new class and allocate the value
                            if (ack == 0)
                            {
                                ClassPoints.Add(new ClassPixels()
                                {
                                    ClassName = _classCorners[i].ClassName,
                                    NumPoints = PixelCalculator(_classCorners[i].Points)
                                });
                            }
                        }
                        #endregion
                    }

                    #region Initialize Class Color
                    //Allocate colors as many as the number of ClassPoints.Count
                    int k = ClassPoints.Count;

                    // Generate colors 
                    List<Color> _color = AddColors(k);
                    for (int i = 0; i < k; i++)
                    {
                        ClassPoints[i].ClassColor = _color[i];
                    }

                    #endregion

                    // Enable Buttons
                    TrainBool = true;
                }

            }

            else
            {
                ResetImages();
                Items.Clear();
                if (!recursiveAlert)
                    MessageBox.Show("Please choose a correct project folder");
            }

            // Set this value to 0
            bmpcount = 0;
            //recursiveCount = 0;
            recursiveAlert = false;
        }

        #endregion

        #region Search Color by ClassName
        /// <summary>
        /// Search used color by class name
        /// </summary>
        /// <param name="className">class name</param>
        /// <returns>the class's color</returns>

        private Color SearchColor(string className)
        {
            foreach (ClassPixels classPixels in ClassPoints)
            {
                if (className == classPixels.ClassName)
                    return classPixels.ClassColor;
            }
            return Colors.Transparent;
        }

        #endregion

        #region Plus Button Enable Bool

        private bool plusBool = false;

        public bool PlusBool
        {
            get { return plusBool; }
            set
            {
                plusBool = value;
                NotifyOfPropertyChange(() => PlusBool);
            }
        }

        #endregion

        #region Plus Button Click Event Handler

        public void PlusClick()
        {
            PopupView popupView = new PopupView();

            // Popup window is opened and closed by OK button
            if (popupView.ShowDialog() == true)
            {
                Console.WriteLine("Popup window is closed by OK button");
                Console.WriteLine(popupView.Answer);

                // New label name
                string _name = popupView.Answer;

                // _name is not null, empty or blank
                if (!String.IsNullOrWhiteSpace(_name))
                {
                    newLabelName = _name;

                    // Add button enable boolean
                    OKBool = true;

                    // Main image is enable to be clicked
                    ImageBool = true;

                    PlusBool = false;
                    ToggleIsEnable = false;

                    // Push the first image 
                    DrawLayer.Push(OverlayImage);
                }
            }

            // Popup window is closed by Cancel or ESC button
            else
            {
                Console.WriteLine("Popup window is closed by Cancel button or ESC key");
            }
        }
        #endregion

        #region OverlayImage Bool
        private bool imageBool = false;
        public bool ImageBool
        {
            get { return imageBool; }
            set
            {
                imageBool = value;
                NotifyOfPropertyChange(() => ImageBool);
            }
        }

        #endregion

        #region Read Only Properties

        #region ActualHeight
        private double myheight;
        public double MyHeight
        {
            get { return myheight; }
            set
            {
                myheight = value;
                NotifyOfPropertyChange(() => MyHeight);
                //Console.WriteLine("MyHeight: " + myheight.ToString());

            }
        }

        #endregion

        #region ActualWidth
        private double mywidth;
        public double MyWidth
        {
            get { return mywidth; }
            set
            {
                mywidth = value;
                NotifyOfPropertyChange(() => MyWidth);
                //Console.WriteLine("MyWidth: " + mywidth.ToString());
            }
        }
        #endregion

        #endregion

        #region New Class Name, Clicked Position and its Color

        private string newLabelName;

        private ObservableCollection<double[]> ClickedPosition = new ObservableCollection<double[]>();

        private Color newLabelColor = Colors.Transparent;

        #endregion

        #region Click points on image
        /// <summary>
        /// Event handler when user draw dots on main image
        /// </summary>

        public void ClickPoint(MouseEventArgs e, IInputElement elem)
        {
            #region Get clicked pixel positions
            var Clicked_X = e.GetPosition(elem).X * BmpWidth / MyWidth;
            var Clicked_Y = e.GetPosition(elem).Y * BmpHeight / MyHeight;

            Console.WriteLine("x: " + Clicked_X.ToString());
            Console.WriteLine("y: " + Clicked_Y.ToString());
            #endregion

            #region Allocate color for new class
            if (newLabelColor == Colors.Transparent)
            {
                // 1. searchcolor 
                if (Colors.Transparent == (newLabelColor = SearchColor(newLabelName)))
                {
                    // 2. Make new color if the new class has no color
                    newLabelColor = AddColors(1)[0];
                }
            }
            #endregion

            #region Draw Dot and line
            SolidColorBrush newlabelBrush = new SolidColorBrush(newLabelColor);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawImage(OverlayImage, new Rect(0, 0, BmpWidth, BmpHeight));
                dc.DrawEllipse(newlabelBrush, null, new Point(Clicked_X, Clicked_Y), 5, 5);

                if (ClickedPosition.Count > 0)
                {
                    // Draw a line between the last element of clickedPosition list and new clicked position
                    Pen pen = new Pen(newlabelBrush, 5);

                    double _x = ClickedPosition.Last()[0];
                    double _y = ClickedPosition.Last()[1];

                    dc.DrawLine(pen, new Point(_x, _y), new Point(Clicked_X, Clicked_Y));
                }
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(BmpWidth, BmpHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            OverlayImage = rtb;

            #endregion

            #region Push into stack

            // Enable revert button
            if (DrawLayer.Count == 1)
            {
                CanBeReverted = true;
            }

            // Push rendered bitmap image
            DrawLayer.Push(rtb);

            #endregion

            #region Add clicked pixel position to the list

            ClickedPosition.Add(new double[2] { Clicked_X, Clicked_Y });

            #endregion

            #region Basic requirement for being clolsed shape
            if (ClickedPosition.Count > 3)
            {
                OKBool = true;
            }
            #endregion

        }
        #endregion

        #region Covert double collection into integer collection
        private List<int[]> ConvertToIntPos(ObservableCollection<double[]> corners)
        {
            List<int[]> pos = new List<int[]>();

            foreach (double[] arr in corners)
            {
                int[] _array = new int[2] { (int)arr[0], (int)arr[1] };

                pos.Add(_array);
            }

            return pos;
        }
        #endregion

        #region Get Parent Directory Path
        /// <summary>
        /// Read loaded bitmap path and get the parent directory path
        /// </summary>
        /// <returns>string path</returns>
        /// 
        private string GetParentParentDirPath()
        {
            // Get current directory path
            string _path = BmpPath1;

            // If the paht is a file path, get parent directory path

            // Exception: If we have no path, return empty
            if (string.IsNullOrEmpty(_path))
                return "";

            // Make all slashes back slashes
            var normalizedPath = _path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            normalizedPath = normalizedPath.Substring(0, lastIndex);
            lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return "";

            var projectPath = normalizedPath.Substring(0, lastIndex);

            //  Remove file name from the file path so we can get a parent directory path
            return projectPath;
        }

        private string GetParentDirPath()
        {
            // Get current directory path
            string _path = BmpPath1;

            // If the paht is a file path, get parent directory path

            // Exception: If we have no path, return empty
            if (string.IsNullOrEmpty(_path))
                return "";

            // Make all slashes back slashes
            var normalizedPath = _path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backslash, return the path itself
            if (lastIndex <= 0)
                return "";

            //  Remove file name from the file path so we can get a parent directory path
            return normalizedPath.Substring(0, lastIndex);
        }
        #endregion

        #region Ok Button Handler
        /// <summary>
        /// Add button click event handler
        /// </summary>

        public void AddNewLabel()
        {
            Console.WriteLine("Ok button is clicked.");

            // Reset boolean variables
            OKBool = false;
            ImageBool = false;
            CanBeReverted = false;

            PlusBool = true;
            ToggleIsEnable = true;
            SaveBool = true;


            // [TODO] Exception: 도형을 이루지 못하는 점들의 위지 관계 e.g, 세 점이 한 직선에 나란히
            if (ClickedPosition.Count < 3)
            {
                MessageBox.Show("Wrong shape");

                for (int i = 0; i < ClickedPosition.Count; i++)
                {
                    DrawLayer.Pop();
                }

                OverlayImage = (BitmapSource)DrawLayer.Peek();

            }


            else
            {

                #region Draw the last line
                SolidColorBrush newlabelBrush = new SolidColorBrush(newLabelColor);

                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawImage(OverlayImage, new Rect(0, 0, BmpWidth, BmpHeight));

                    Pen pen = new Pen(newlabelBrush, 5);

                    double x1 = ClickedPosition.ElementAt(0)[0];
                    double y1 = ClickedPosition.ElementAt(0)[1];

                    double x2 = ClickedPosition.ElementAt(ClickedPosition.Count - 1)[0];
                    double y2 = ClickedPosition.ElementAt(ClickedPosition.Count - 1)[1];

                    dc.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));

                }

                RenderTargetBitmap rtb = new RenderTargetBitmap(BmpWidth, BmpHeight, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(dv);

                OverlayImage = rtb;

                #endregion

                #region Update table item source

                List<int[]> newCornerPoints = ConvertToIntPos(ClickedPosition);

                // Check if new label name exist or not
                int ack = 0;
                for (int i = 0; i < ClassPoints.Count; i++)
                {
                    if (ClassPoints[i].ClassName == newLabelName)
                    {
                        // If new class name exiset in ClassPoint (table item source)
                        ack = 1;
                        ClassPoints[i].NumPoints += PixelCalculator(newCornerPoints);
                        break;
                    }
                }

                // New class name doesn't exist in ClassPoint (table item source)
                if (ack == 0)
                {
                    ClassPoints.Add(new ClassPixels() { ClassName = newLabelName, NumPoints = PixelCalculator(newCornerPoints), ClassColor = newLabelColor });
                }

                #endregion

                #region Get parent directory path
                // Get current directory path
                string _path = BmpPath1;

                // If the paht is a file path, get parent directory path

                // Exception: If we have no path, return empty
                if (string.IsNullOrEmpty(_path))
                    return;

                // Make all slashes back slashes
                var normalizedPath = _path.Replace('/', '\\');

                // Find the last backslash in the path
                var lastIndex = normalizedPath.LastIndexOf('\\');

                // If we don't find a backslash, return the path itself
                if (lastIndex <= 0)
                    return;

                //  Remove file name from the file path so we can get a parent directory path
                var dirPath = normalizedPath.Substring(0, lastIndex);

                #endregion

                #region Update Dictionary
                ClassCornerPoints newLabelPoints = new ClassCornerPoints() { ClassName = newLabelName, Points = newCornerPoints };

                // If there is alreaey csv path in the dictionary
                if (CornerPoint.ContainsKey(dirPath + '\\' + "metadata.csv"))
                {
                    CornerPoint[dirPath + '\\' + "metadata.csv"].Add(newLabelPoints);
                }

                // If there isn't, we gotta make new csv file
                else
                {
                    CornerPoint.Add(dirPath + '\\' + "metadata.csv", new ObservableCollection<ClassCornerPoints> { newLabelPoints });
                }

                #endregion
            }
            #region Reset Temporary Variables

            ClickedPosition.Clear();
            newLabelColor = Colors.Transparent;
            DrawLayer.Clear();

            #endregion
        }

        #endregion

        #region Draw Labels
        private void DrawLabel()
        {
            #region Get csv file path

            if (String.IsNullOrEmpty(CsvPath))
            {
                //If there is no csv file, set a csv file path
                CsvPath = GetParentDirPath() + '\\' + "metadata.csv";
            }
            #endregion

            // Reset OverlayImage
            OverlayImage = BitmapSourceConvert.ToBitmapSource(new Image<Bgra, byte>(BmpWidth, BmpHeight, new Bgra(255, 255, 255, 0)));

            #region Traversal dictionary and draw
            if (CornerPoint.ContainsKey(CsvPath))
            {
                foreach (ClassCornerPoints classCornerPoints in CornerPoint[CsvPath])
                {
                    #region Draw each shape
                    DrawingVisual dv = new DrawingVisual();
                    using (DrawingContext dc = dv.RenderOpen())
                    {
                        // Make Brush using class color
                        SolidColorBrush classBrush = new SolidColorBrush(SearchColor(classCornerPoints.ClassName));

                        dc.DrawImage(OverlayImage, new Rect(0, 0, BmpWidth, BmpHeight));

                        // Draw dot and line repeatedly

                        Pen pen = new Pen(classBrush, 5);

                        int i = 0;

                        for (i = 0; i < classCornerPoints.Points.Count - 1; i++)
                        {
                            var x1 = classCornerPoints.Points[i][0];
                            var y1 = classCornerPoints.Points[i][1];
                            var x2 = classCornerPoints.Points[i + 1][0];
                            var y2 = classCornerPoints.Points[i + 1][1];

                            dc.DrawEllipse(classBrush, null, new Point(x1, y1), 5, 5);
                            dc.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));

                        }

                        // Draw line between the first dot and the last one
                        var x_last = classCornerPoints.Points[i][0];
                        var y_last = classCornerPoints.Points[i][1];
                        var x_first = classCornerPoints.Points[0][0];
                        var y_first = classCornerPoints.Points[0][1];
                        dc.DrawEllipse(classBrush, null, new Point(x_last, y_last), 5, 5);
                        dc.DrawLine(pen, new Point(x_last, y_last), new Point(x_first, y_first));

                    }

                    RenderTargetBitmap rtb = new RenderTargetBitmap(BmpWidth, BmpHeight, 96, 96, PixelFormats.Pbgra32);
                    rtb.Render(dv);

                    OverlayImage = rtb;

                    #endregion
                }
            }
            #endregion
        }
        #endregion

        #region Color Filter Dictionary
        private Dictionary<string, BitmapSource> ColorFilters = new Dictionary<string, BitmapSource>();

        #endregion

        #region Generate Color filter by inference

        private void ColorFilterGenerator()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Timer is started");

            #region Get paths
            // Get project folder, model path
            string modelPath = ModelPath;
            string projectPath = GetParentParentDirPath();

            List<string> picturePaths = new List<string>();

            foreach (string dir in Directory.GetDirectories(projectPath))
            {
                picturePaths.Add(dir);
            }

            #endregion

            #region Label Data | Color dictionary
            // TODO: 일단 ClassPoints에 나열된 순서대로 배치해보자

            //{ 0, "background"}, , { 1, "one" }, { 2, "two" }, { 3 , "three" }, { 4 , "four" }

            Dictionary<int, Color> colorDict = new Dictionary<int, Color> ();

            // Add classes color
            for (int i = 0; i < ClassPoints.Count; i++)
            {
                colorDict.Add(i + 1, ClassPoints[i].ClassColor);
            }

            #endregion

            #region Load a model
            SVM svm = new SVM();
            try
            {
                FileStorage file = new FileStorage(modelPath, FileStorage.Mode.Read);
                svm.Read(file.GetNode("opencv_ml_svm"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Loading a model: " + e.Message);
                ToggleIsEnable = false;
                MessageBox.Show("This model cannot predict current dataset");
                return;
            }
            #endregion

            // Get color filter dictionary
            #region Access every pixel

            // 사진 마다 for loop
            foreach (string path in picturePaths)
            {

                try
                {
                    #region Image variable
                    Image<Gray, Byte> img1 = new Image<Gray, Byte>(path + '\\' + "600.bmp");
                    Image<Gray, Byte> img2 = new Image<Gray, Byte>(path + '\\' + "725.bmp");
                    Image<Gray, Byte> img3 = new Image<Gray, Byte>(path + '\\' + "825.bmp");
                    Image<Gray, Byte> img4 = new Image<Gray, Byte>(path + '\\' + "875.bmp");
                    Image<Gray, Byte> img5 = new Image<Gray, Byte>(path + '\\' + "930.bmp");
                    Image<Gray, Byte> img6 = new Image<Gray, Byte>(path + '\\' + "985.bmp");

                    List<Mat> MatList = new List<Mat>
                    {
                        CvInvoke.Imread(path + '\\' + "600.bmp", Emgu.CV.CvEnum.ImreadModes.Grayscale),
                        CvInvoke.Imread(path + '\\' + "725.bmp", Emgu.CV.CvEnum.ImreadModes.Grayscale),
                        CvInvoke.Imread(path + '\\' + "825.bmp", Emgu.CV.CvEnum.ImreadModes.Grayscale),
                        CvInvoke.Imread(path + '\\' + "875.bmp", Emgu.CV.CvEnum.ImreadModes.Grayscale),
                        CvInvoke.Imread(path + '\\' + "930.bmp", Emgu.CV.CvEnum.ImreadModes.Grayscale),
                        CvInvoke.Imread(path + '\\' + "985.bmp", Emgu.CV.CvEnum.ImreadModes.Grayscale)
                    };

                    #endregion

                    #region Threshold
                    // Global threshold
                    foreach (Mat mat in MatList)
                    {
                        CvInvoke.Threshold(mat, mat, 100, 255, Emgu.CV.CvEnum.ThresholdType.Binary | Emgu.CV.CvEnum.ThresholdType.Otsu);
                    }

                    #endregion

                    #region morphologyEx

                    // morphologyEx
                    Mat kernel1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle,
                        new System.Drawing.Size(3, 3), new System.Drawing.Point(-1, -1));

                    foreach (Mat mat in MatList)
                    {
                        CvInvoke.MorphologyEx(mat, mat, Emgu.CV.CvEnum.MorphOp.Open, kernel1,
                        new System.Drawing.Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar());
                    }

                    #endregion

                    #region Adding layers

                    int cnt = 2;
                    int rows = MatList[0].Rows;
                    int cols = MatList[0].Cols;

                    Mat addedMat = Mat.Zeros(rows, cols, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
                    for (int i = 0; i < MatList.Count; i++)
                    {
                        Mat mat = MatList[i] / cnt;
                        addedMat += mat;
                    }

                    CvInvoke.Threshold(addedMat, addedMat, 200, 255, Emgu.CV.CvEnum.ThresholdType.Binary);

                    Mat label = new Mat();
                    Mat stats = new Mat();
                    Mat centroids = new Mat();

                    CvInvoke.ConnectedComponentsWithStats(addedMat, label, stats, centroids);

                    int[] _stats = new int[stats.Rows * stats.Cols];
                    stats.CopyTo(_stats);

                    List<System.Drawing.Rectangle> RecList = new List<System.Drawing.Rectangle>();

                    for (int i = 1; i < stats.Rows; i++)
                    {
                        if (_stats[i * stats.Cols + 4] > 10)
                        {
                            var x = _stats[i * stats.Cols + 0];
                            var y = _stats[i * stats.Cols + 1];
                            var width = _stats[i * stats.Cols + 2];
                            var height = _stats[i * stats.Cols + 3];

                            RecList.Add(new System.Drawing.Rectangle(x, y, width, height));
                        }
                    }

                    Image<Gray, byte> addedImage = addedMat.ToImage<Gray, byte>();
                    Image<Bgra, byte> colorfilter = new Image<Bgra, byte>(cols, rows, new Bgra(255, 255, 255, 0));

                    var a = BitConverter.GetBytes(100)[0];

                    foreach (System.Drawing.Rectangle rectangle in RecList)
                    {
                        // rectangle 안에 있는 모든 좌표 접근

                        // Column
                        for (int i = rectangle.X; i < rectangle.X + rectangle.Width; i++)
                        {
                            // Row
                            for (int j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++)
                            {
                                if (addedImage.Data[j, i, 0] != 0)
                                {
                                    float[,] _vector = new float[1, 6];
                                    // Data[col(i), row(j), 0]
                                    _vector[0, 0] = (float)img1.Data[j, i, 0];
                                    _vector[0, 1] = (float)img2.Data[j, i, 0];
                                    _vector[0, 2] = (float)img3.Data[j, i, 0];
                                    _vector[0, 3] = (float)img4.Data[j, i, 0];
                                    _vector[0, 4] = (float)img5.Data[j, i, 0];
                                    _vector[0, 5] = (float)img6.Data[j, i, 0];

                                    // matrix를 만든다.
                                    Matrix<float> matrix = new Matrix<float>(_vector);

                                    // svm.predict 에 넣는다.
                                    int prediction = (int)svm.Predict(matrix);

                                    // prediction에 따라 해당 matrix에 255 할당
                                    colorfilter.Data[j, i, 0] = colorDict[prediction].B;
                                    colorfilter.Data[j, i, 1] = colorDict[prediction].G;
                                    colorfilter.Data[j, i, 2] = colorDict[prediction].R;
                                    colorfilter.Data[j, i, 3] = a;
                                }

                            }
                        }

                    }
                    #endregion

                    ColorFilters.Add(path, BitmapSourceConvert.ToBitmapSource(colorfilter));
                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine("Inference error: " + e.Message);
                    ToggleIsEnable = false;
                    MessageBox.Show("This model cannot predict current dataset");
                    return;

                }
            }

            // 여기서 모델 inference가 잘 되면 ToggleIsEnable true이고 아니면 false
            ToggleIsEnable = true;

            stopwatch.Stop();
            Console.WriteLine("Elapsed time: " + (stopwatch.ElapsedMilliseconds / 1000).ToString() + "seconds");

        }

        #endregion


        #region File explorer bool
        private bool fileExplorerBool = false;
        public bool FileExplorerBool
        {
            get { return fileExplorerBool; }
            set
            {
                fileExplorerBool = value;
                NotifyOfPropertyChange(() => FileExplorerBool);
            }
        }
        #endregion


        #region Save Button Enable Bool

        private bool saveBool = false;

        public bool SaveBool
        {
            get { return saveBool; }
            set
            {
                saveBool = value;
                NotifyOfPropertyChange(() => SaveBool);
            }
        }

        #endregion

        #region Train Button Enable Bool

        private bool trainBool = false;

        public bool TrainBool
        {
            get { return trainBool; }
            set
            {
                trainBool = value;
                NotifyOfPropertyChange(() => TrainBool);
            }
        }

        #endregion

        #region Save Button Enable Bool

        private bool oKBool = false;

        public bool OKBool
        {
            get { return oKBool; }
            set
            {
                oKBool = value;
                NotifyOfPropertyChange(() => OKBool);
            }
        }

        #endregion

        #region Save Button Click Event Handler
        /// <summary>
        /// Write metadata in csv format
        /// </summary>
        public void Save()
        {
            Console.WriteLine("Save button");
            SaveBool = false;

            #region Save corner points in csv format
            // Dictionary loop
            foreach (KeyValuePair<string, ObservableCollection<ClassCornerPoints>> collection in CornerPoint)
            {
                try
                {
                    // csv stream
                    using (var w = new StreamWriter(collection.Key))
                    {
                        // Write header
                        string header = "class,pixels";
                        w.WriteLine(header);
                        w.Flush();

                        for (int i = 0; i < collection.Value.Count; i++)
                        {
                            string line = collection.Value[i].ClassName;

                            for (int j = 0; j < collection.Value[i].Points.Count; j++)
                            {
                                //x position
                                line += ",";
                                line += collection.Value[i].Points[j][0].ToString();

                                // y position
                                line += ",";
                                line += collection.Value[i].Points[j][1].ToString();
                            }
                            w.WriteLine(line);
                            w.Flush();
                        }

                    }
                }
                catch (IOException e)
                {
                    MessageBox.Show("Saving Error");
                    Console.WriteLine("Write Csv Error" + e);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Saving Error");
                    Console.WriteLine("Write Csv Error" + e);
                }
            }
            #endregion

            #region Save pixel vector to binary files for each class

            #region Image variable
            Image<Gray, Byte> img1 = new Image<Gray, Byte>(bmpPath1);
            Image<Gray, Byte> img2 = new Image<Gray, Byte>(bmpPath2);
            Image<Gray, Byte> img3 = new Image<Gray, Byte>(bmpPath3);
            Image<Gray, Byte> img4 = new Image<Gray, Byte>(bmpPath4);
            Image<Gray, Byte> img5 = new Image<Gray, Byte>(bmpPath5);
            Image<Gray, Byte> img6 = new Image<Gray, Byte>(bmpPath6);
            #endregion


            // loop: class name
            foreach (ClassPixels classPixels in ClassPoints)
            {
                // Temporary variables for each class's shapes
                List<List<int[]>> shapes = new List<List<int[]>>();

                // Get all shapes which have same class name with classPixels.ClassName
                foreach (KeyValuePair<string, ObservableCollection<ClassCornerPoints>> keyValue in CornerPoint)
                {
                    foreach (ClassCornerPoints classCornerPoints in keyValue.Value)
                    {
                        if (classCornerPoints.ClassName == classPixels.ClassName)
                        {
                            shapes.Add(classCornerPoints.Points);
                        }
                    }
                }

                // Configure save path

                FileStream fs = File.Open(GetParentParentDirPath() + '\\' + classPixels.ClassName + ".bin", FileMode.Create);
                BinaryWriter wr = new BinaryWriter(fs);

                foreach (List<int[]> shape in shapes)
                {

                    #region pixel vector

                    #region Calculate boundary
                    int num_pixel = 0;
                    int num_point = shape.Count;
                    int[,] points = new int[num_point, 2];

                    // Change array into list
                    for (int i = 0; i < num_point; i++)
                    {
                        points[i, 0] = shape[i][1];
                        points[i, 1] = shape[i][0];
                    }

                    // Calculate range of y value
                    int min = points[0, 0];
                    int max = 0;

                    for (int i = 0; i < num_point; i++)
                    {
                        if (points[i, 0] < min)
                        {
                            min = points[i, 0];
                        }

                        if (points[i, 0] > max)
                        {
                            max = points[i, 0];
                        }
                    }

                    // Initialize list for each y values along y axis
                    List<List<int>> boundary = new List<List<int>>();
                    for (int i = 0; i < max - min + 1; i++)
                    {
                        boundary.Add(new List<int>());
                    }

                    // Exclude sharp point from boudary
                    if ((points[num_point - 1, 0] - points[0, 0]) * (points[1, 0] - points[0, 0]) <= 0)
                    {
                        boundary[points[0, 0] - min].Add(points[0, 1]);
                    }

                    else
                    {
                        num_pixel += 1;
                    }

                    for (int i = 1; i < num_point - 1; i++)
                    {
                        if ((points[i - 1, 0] - points[i, 0]) * (points[i + 1, 0] - points[i, 0]) <= 0)
                        {
                            boundary[points[i, 0] - min].Add(points[i, 1]);
                        }

                        else
                        {
                            num_pixel += 1;
                        }
                    }

                    if ((points[0, 0] - points[num_point - 1, 0]) * (points[num_point - 2, 0] - points[num_point - 1, 0]) <= 0)
                    {
                        boundary[points[num_point - 1, 0] - min].Add(points[num_point - 1, 1]);
                    }

                    else
                    {
                        num_pixel += 1;
                    }

                    // Calcuate boudary positions
                    for (int i = 0; i < num_point - 1; i++)
                    {
                        if (points[i + 1, 0] == points[i, 0])
                        {
                            num_pixel += 0;
                        }

                        else if (points[i + 1, 0] - points[i, 0] > 0)
                        {
                            for (int j = 1; j < points[i + 1, 0] - points[i, 0]; j++)
                            {
                                boundary[points[i, 0] - min + j].Add(points[i, 1] + (j * (points[i + 1, 1] - points[i, 1])) / (points[i + 1, 0] - points[i, 0]));
                            }
                        }

                        else //points[i+1,0] - points[i,0] < 0
                        {
                            for (int j = 1; j < points[i, 0] - points[i + 1, 0]; j++)
                            {
                                boundary[points[i, 0] - min - j].Add(points[i, 1] + (j * (points[i + 1, 1] - points[i, 1])) / (points[i, 0] - points[i + 1, 0]));
                            }
                        }
                    }


                    // Compare the first element to the last one
                    if (points[num_point - 1, 0] == points[0, 0])
                    {
                        num_pixel += 0;
                    }

                    else if (points[0, 0] - points[num_point - 1, 0] > 0)
                    {
                        for (int j = 1; j < points[0, 0] - points[num_point - 1, 0]; j++)
                        {
                            boundary[points[num_point - 1, 0] - min + j].Add(points[num_point - 1, 1] + (j * (points[0, 1] - points[num_point - 1, 1])) / (points[0, 0] - points[num_point - 1, 0]));
                        }
                    }

                    else //points[i+1,0] - points[i,0] < 0
                    {
                        for (int j = 1; j < points[num_point - 1, 0] - points[0, 0]; j++)
                        {
                            boundary[points[num_point - 1, 0] - min - j].Add(points[num_point - 1, 1] + (j * (points[0, 1] - points[num_point - 1, 1])) / (points[num_point - 1, 0] - points[0, 0]));
                        }
                    }

                    #endregion

                    #region Write pixel vector to binary file 
                    // Write the pixel value to binary file
                    for (int i = 0; i < max - min + 1; i++)
                    {
                        if (boundary[i].Count == 0)
                        {
                            num_pixel += 0;
                        }
                        else
                        {
                            boundary[i].Sort();
                            bool inner = false;
                            for (int j = boundary[i].Min(); j < boundary[i].Max() + 1; j++)
                            {
                                if (boundary[i].Contains(j))
                                {
                                    inner = !inner;
                                }
                                if (inner)
                                {
                                    wr.Write((float)img1.Data[min + i, j, 0]);
                                    wr.Write((float)img2.Data[min + i, j, 0]);
                                    wr.Write((float)img3.Data[min + i, j, 0]);
                                    wr.Write((float)img4.Data[min + i, j, 0]);
                                    wr.Write((float)img5.Data[min + i, j, 0]);
                                    wr.Write((float)img6.Data[min + i, j, 0]);

                                }

                            }
                        }

                    }
                    #endregion

                    #endregion
                }

                wr.Close();
                fs.Close();


            }

            Console.WriteLine("Binary files are successfully saved to your project directory.");
            MessageBox.Show("Binary files are successfully saved to your project directory.");

            #endregion
        }
        #endregion

        #region String Path to Bitmap Source
        private BitmapSource StringToBmpSource(string path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(@path); //put in a valid path to an image or use your image you get from the array
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(fi.FullName, UriKind.Absolute);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawImage(src, new Rect(0, 0, BmpWidth, BmpHeight));
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(BmpWidth, BmpHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            return rtb;
        }
        #endregion

        #region Bitmap Rewind Stack

        private Stack DrawLayer = new Stack();

        #endregion

        #region Revert Boolean Variable

        private bool canBeReverted = false;

        public bool CanBeReverted
        {
            get { return canBeReverted; }
            set
            {
                canBeReverted = value;
                NotifyOfPropertyChange(() => canBeReverted);
            }
        }

        #endregion

        #region Revert Button Handler

        public void Revert()
        {
            // Nothing to be poped
            if (DrawLayer.Count < 2) return;

            // Revert main image
            DrawLayer.Pop();
            OverlayImage = (BitmapSource)DrawLayer.Peek();

            // The last element
            if (DrawLayer.Count < 2)
            {
                CanBeReverted = false;
            }

            // Remove the last element of ClickedPosition
            int count = ClickedPosition.Count;

            if (count > 0)
                ClickedPosition.RemoveAt(ClickedPosition.Count - 1);

            else
                return;

        }

        #endregion


    }
}
