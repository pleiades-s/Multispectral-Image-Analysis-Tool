using Assisticant.Fields;
using BeyonSense.Models;
using BeyonSense.Views;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            set
            {
                bmpPath1 = value;
                NotifyOfPropertyChange(() => BmpPath1);
                DrawLabel();
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

        #region Bitmap image format: 600 x 800

        private readonly int BmpHeight = 600;
        private readonly int BmpWidth = 800;

        #endregion


        #region Main Bitmap image path

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

        #endregion

        #region Selected Parameter Dictionary File Path

        private string pthPath;

        public string PthPath
        {
            get { return pthPath; }
            set
            {
                pthPath = value;
                NotifyOfPropertyChange(() => PthPath);
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
            }
        }
        #endregion

        #region Shuffle color

        public Random random = new Random();

        public Tuple<int, int> RandomNumber(int[,] boolArray, int num)
        {

            Tuple<int, int> _tuple = Tuple.Create(random.Next(0, num), random.Next(0, num));

            int idx1 = Math.Max(_tuple.Item1, _tuple.Item2);
            int idx2 = Math.Min(_tuple.Item1, _tuple.Item2);

            while (boolArray[idx1, idx2] != 0)
            {
                _tuple = Tuple.Create(random.Next(0, num), random.Next(0, num));
                idx1 = Math.Max(_tuple.Item1, _tuple.Item2);
                idx2 = Math.Min(_tuple.Item1, _tuple.Item2);
            }

            return Tuple.Create(idx1, idx2);
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

            int[,] basic_color = new int[6, 3] { { 255, 0, 0 }, { 0, 255, 0 },
                                                { 0, 0, 255 }, { 255, 255, 0 }, { 0, 255, 255 },
                                                { 255, 0, 255 } };

            int[,] generator_color = new int[n, 3];

            // Boolean array to check whether a random value is repeated or not
            int[,] randomBoolean = new int[n, n];

            // Initialize boolean array set to 0
            for (int i = 0; i < n ; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    randomBoolean[i, j] = 0;
                }
            }



            #region Less than 6
            if (n <= 6)
            {

                for (int i = 0; i < n; i++)
                {
                    generator_color[i, 0] = basic_color[i, 0];
                    generator_color[i, 1] = basic_color[i, 1];
                    generator_color[i, 2] = basic_color[i, 2];

                }
            }
            #endregion

            #region eqaul to or more than 6
            else
            {
                // Add basis colors which are eight colors
                for (int i = 0; i < 6; i++)
                {
                    generator_color[i, 0] = basic_color[i, 0];
                    generator_color[i, 1] = basic_color[i, 1];
                    generator_color[i, 2] = basic_color[i, 2];
                }

                // New color is needed to be generated

                // TODO: 

                for (int i = 0; i < n - 6; i++)
                {

                    Tuple<int, int> _tuple = RandomNumber(randomBoolean, 6 + i);
                    int index1 = _tuple.Item1, index2 = _tuple.Item2;
                    randomBoolean[index1, index2] = 1;


                    Console.WriteLine(index1.ToString() + " " + index2.ToString());

                    // Allocate new R, G, B value
                    generator_color[i + 6, 0] = (int)(generator_color[index1, 0] + generator_color[index2, 0]) / 2;
                    generator_color[i + 6, 1] = (int)(generator_color[index1, 1] + generator_color[index2, 1]) / 2;
                    generator_color[i + 6, 2] = (int)(generator_color[index1, 2] + generator_color[index2, 2]) / 2;
                   
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

                    // Dictionary Clear
                    CornerPoint.Clear();

                    // ObservableCollection<ClassPoint> Clear
                    ClassPoints.Clear();

                    // Color, NumPoints Clear
                    Colour = Color.FromArgb(0, 255, 255, 255);
                    NumPoints = "";
                    csvFilePaths.Clear();

                    // Plus Button Reset
                    PlusBool = false;
                    #endregion

                    // Button Rest
                    SaveBool = false;
                    TrainBool = false;
                    OKBool = false;


                    // Calculate inside pixels and update table elements
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
            
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("pack://application:,,,/Pictures/no_image.png");
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            MainBmpImage = src;
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

                //loop for files in the folder
                foreach (FileInfo fileInfo in folder.GetFiles())
                {
                    //each file path
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
                    MainBmpImage = StringToBmpSource(BmpList[0]);

                    // Set public variable CsvPath as the csv file path
                    CsvPath = _csvPath;
                    DrawLabel();
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
            //MainBmpImage = path;

            System.IO.FileInfo fi = new System.IO.FileInfo(@path); //put in a valid path to an image or use your image you get from the array
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(fi.FullName, UriKind.Absolute);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            MainBmpImage = src;

            DrawLabel();

            ImageBool = false;


        }
        #endregion

        #region Open Button Click Handler: Parameter Dictionary File Explorer

        public void FileOpen()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Search file filter by exetension
            openFileDialog.Filter = "State Dict.(*.pth)|*.pth|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Get full path of the selected file
                PthPath = openFileDialog.FileName;
            }
        }
        #endregion

        #region Toggle Click Event Handler

        public void ToggleClick()
        {
            if (!ToggleBool)
            {
                ToggleBool = true;
            }
            else
            {
                ToggleBool = false;

                // Clear file path
                PthPath = "";
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

        private ClassPixels selectedRow;

        public ClassPixels SelectedRow
        {
            get { return selectedRow; }

            set
            {
                selectedRow = value;

                if (selectedRow != null)
                {
                    Colour = selectedRow.ClassColor;
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
        private List<string> _csvFilePaths = new List<string>();
        public List<string> csvFilePaths
        {
            get { return _csvFilePaths; }
            set
            {
                _csvFilePaths = value;
            }
        }
        #endregion

        #region Walking Directory
        /// <summary>
        /// Get every csv file paths and add into csvFilePaths variable
        /// </summary>
        /// <param name="sDir">string _rootDir</param>

        // Warning variables preventing to open deep directory tree
        public int recursiveCount = 0;
        public bool recursiveAlert = false;

        public void DirSearch(string sDir)
        {
            if (recursiveCount > 3) {
                if (!recursiveAlert)
                {
                    MessageBox.Show("Csv files are not successfully loaded.\n Please make sure that you select a correct project folder.");
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
                        //Console.WriteLine(f);

                        // Check it file exetension is csv or not
                        if (System.IO.Path.GetExtension(f) == ".csv")
                        {
                            // Add csv file path in the list
                            csvFilePaths.Add(f);

                            //Console.WriteLine(f);
                        }
                    }
                    DirSearch(d);
                    recursiveCount++;
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine("DirSearch Exception: " + excpt);
                MessageBox.Show("Please choose a correct project folder");
            }

        }
        #endregion

        #region String -> Int Converter
        /// <summary>
        /// Converter: String to Integer
        /// </summary>
        /// <param name="str">string num</param>
        /// <returns>int num</returns>
        public int StrToInt(string str)
        {
            int i = 0;
            if (!Int32.TryParse(str, out i))
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
        
       public int PixelCalculator(ObservableCollection<int[]> _cornerPoint)
        {
            int num_pixel = 0;
            int num_point = _cornerPoint.Count;
            int[,] points = new int[num_point, 2];

            //list를 array로 변화
            for (int i = 0; i < num_point; i++)
            {
                points[i, 0] = _cornerPoint[i][0];
                points[i, 1] = _cornerPoint[i][1];
            }

            //y축 범위 구하기
            int min = points[0,0];//임의의 값
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

            //각 y축에 따른 list 초기화
            List<List<int>> boundary = new List<List<int>>();
            for (int i = 0; i < max - min + 1; i++)
            {
                boundary.Add(new List<int>());
            }

            //첨점은 boundary에서 제외시키기
            if ((points[num_point - 1, 0] - points[0, 0]) * (points[1, 0] - points[0, 0]) <= 0)
                boundary[points[0, 0] - min].Add(points[0, 1]);
            else
                num_pixel += 1;
            for (int i = 1; i < num_point - 1; i++)
            {
                if ((points[i - 1, 0] - points[i, 0]) * (points[i + 1, 0] - points[i, 0]) <= 0)
                    boundary[points[i, 0] - min].Add(points[i, 1]);
                else
                    num_pixel += 1;
            }
            if ((points[0, 0] - points[num_point - 1, 0]) * (points[num_point - 2, 0] - points[num_point - 1, 0]) <= 0)
                boundary[points[num_point - 1, 0] - min].Add(points[num_point - 1, 1]);
            else
                num_pixel += 1;

            //boundary 다 구하기
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
            //list 마지막 성분과 첫 성분 비교
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


            // pixel 갯수 구하기
            for (int i = 0; i < max - min + 1; i++)
            {
                if (boundary[i].Count == 0)
                {
                    num_pixel += 0;
                }
                else
                {
                    boundary[i].Sort();
                    for (int j = 0; j < boundary[i].Count / 2; j++)
                    {
                        num_pixel += boundary[i][2 * j + 1] - boundary[i][2 * j] + 1;
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
        /// 
        public void PointCalculator(string _rootPath)
        {
            // [This method is called when a folder is selected by folder explorer]

            // Traverse all the directory and find all existing csv file paths
            DirSearch(_rootPath);


            if (recursiveAlert)
            {            
                // Set this value to 0
                recursiveCount = 0;
                recursiveAlert = false;

                return;
            }

            // Set this value to 0
            recursiveCount = 0;
            recursiveAlert = false;

            // Read csv file only if Dirsearch is successfully completed
            if (!recursiveAlert)
            {

                // For each csv file
                foreach (string _path in csvFilePaths)
                {
                    // Read class name and corner points
                    ObservableCollection<ClassCornerPoints> _classCorners = new ObservableCollection<ClassCornerPoints>();


                    using (var reader = new StreamReader(_path))
                    {
                        int _numLine = 0;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();

                            if (_numLine != 0)
                            {
                                string _className = "";
                                ObservableCollection<int[]> _cornerPoints = new ObservableCollection<int[]>();

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


                    // Allocate new dictionary { Class name: Corner point Collection<T> } <-- We'd start from here if user add new boundary
                    // Each csv file
                    CornerPoint.Add(_path, _classCorners);


                    // TODO: Calculate the number of inside points and Add ClassPoints 

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
                }

                #region Initialize Class Color
                //Allocate colors as many as the number of ClassPoints.Count
                int k = ClassPoints.Count;

                // Generate colors 
                List<Color> _color = ColorGenerator(k);
                for (int i = 0; i < k; i++)
                {
                    ClassPoints[i].ClassColor = _color[i];
                }
                
                #endregion

                // Enable Buttons
                PlusBool = true;
                TrainBool = true;

                
            }



        }

        #endregion

        // 
        private Color SearchColor(string className)
        {
            foreach(ClassPixels classPixels in ClassPoints)
            {
                if (className == classPixels.ClassName) return classPixels.ClassColor;
            }

            return Colors.Transparent;
            
        }


        #region Allocate colors

        // 새로운 색깔 return 

        public Color AllocateColors()
        {
            // return value
            Color returnColor = Colors.Transparent;

            int count = ClassPoints.Count;
            // Generate colors 
            List<Color> _color = ColorGenerator(count + 1);

            // Color classcolor : int Used
            Dictionary<Color, int> ColorDictionary = new Dictionary<Color, int>();

            // Allocate Dictionary elements
            foreach (Color color in _color)
            {
                ColorDictionary.Add(color, 0);
            }

            for (int i = 0; i < count; i++)
            {
                if (ColorDictionary.ContainsKey(ClassPoints[i].ClassColor))
                {
                    ColorDictionary[ClassPoints[i].ClassColor] += 1;
                }

            }

            foreach (KeyValuePair<Color, int> element in ColorDictionary)
            {
                // Assign unused Color
                if (element.Value == 0)
                {
                    return element.Key;
                }
            }


            return returnColor;
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
            Console.WriteLine("Plus");
            PopupView popupView = new PopupView();

            string _name = "";

            // Popup window is opened and closed by OK button
            if (popupView.ShowDialog() == true)
            {
                Console.WriteLine("Popup window is closed by OK button");
                Console.WriteLine(popupView.Answer);

                // New label name
                _name = popupView.Answer;

                // _name is not null, empty or blank
                if (!String.IsNullOrWhiteSpace(_name))
                {
                    newLabelName = _name;

                    // Add button enable boolean
                    OKBool = true;

                    // Main image is enable to be clicked
                    ImageBool = true;
                }
            }

            // Popup window is closed by Cancel or ESC button
            else
            {
                Console.WriteLine("Popup window is closed by Cancel button or ESC key");

            }
        }
        #endregion

        #region MainImage Bool
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


        #region ActualHeight, ActualWidth properties
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


        private ObservableCollection<double[]> ClickedPosition = new ObservableCollection<double[]>();

        private Color newLabelColor = Colors.Transparent;

        #region TODO: Click points on image
        /// <summary>
        /// Event handler when user draw dots on main image
        /// </summary>
        /// <param name="newLabelName">new label name from Popup window</param>
        public void ClickPoint(MouseEventArgs e, IInputElement elem)
        {
            Console.WriteLine("Actual Width: " + MyWidth.ToString());
            Console.WriteLine("Actual Height: " + MyHeight.ToString());

            var Clicked_X = e.GetPosition(elem).X * BmpWidth / MyWidth;
            var Clicked_Y = e.GetPosition(elem).Y * BmpHeight / MyHeight;

            Console.WriteLine("x: " + Clicked_X.ToString());
            Console.WriteLine("y: " + Clicked_Y.ToString());


            

            if(newLabelColor == Colors.Transparent)
            {
                // 1. searchcolor 
                if (Colors.Transparent == (newLabelColor = SearchColor(newLabelName)))
                {
                    // 2. 없으면 색 새로 만들기
                    newLabelColor = AllocateColors();
                }
                
            }

            SolidColorBrush newlabelBrush = new SolidColorBrush(newLabelColor);
            


            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawImage(MainBmpImage, new Rect(0, 0, BmpWidth, BmpHeight));
                dc.DrawEllipse(newlabelBrush, null, new Point(Clicked_X, Clicked_Y), 5, 5);

                if (ClickedPosition.Count > 0)
                {
                    // ClickedPosition의 마지막 원소 좌표와 현재 클릭된 좌표를 잇는 선 그리기
                    Pen pen = new Pen(newlabelBrush, 5);
                    
                    double _x = ClickedPosition.Last()[0];
                    double _y = ClickedPosition.Last()[1];

                    dc.DrawLine(pen, new Point(_x, _y), new Point(Clicked_X, Clicked_Y));
                    
                }
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(BmpWidth, BmpHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            MainBmpImage = rtb;

            ClickedPosition.Add(new double[2] { Clicked_X, Clicked_Y });
            // [PlusClick 함수에서 이름을 받자마자 바로 ClickPoint 함수 실행]

            // 점 세 개 이상 일 때.
            SaveBool = true;




        }
        #endregion

        private string newLabelName;

        private ObservableCollection<int[]> ConvertToIntPos(ObservableCollection<double[]> corners)
        {
            ObservableCollection<int[]> pos = new ObservableCollection<int[]>();

            foreach ( double[] arr in corners)
            {
                int[] _array = new int[2] { (int)arr[0], (int)arr[1] };

                pos.Add(_array);
            }

            return pos;
        }


        private string GetParentDirPath()
        {
            #region Get parent directory path
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

            #endregion
        }

        #region TODO: Ok Button Handler
        /// <summary>
        /// Add button click event handler
        /// </summary>
        /// <param name="newLabelName">new label name from Popup window</param>
        /// <param name="newCornerPoints">new corner points from ClickPoint</param>
        public void AddNewLabel()
        {
            Console.WriteLine("Ok button is clicked.");

            // 1. Main image에서 포인트 클릭할 수 있게 해야 함.
            // 1-1. ESC key -> exit

            // 2. Add 버튼이 눌리면 끝나야 함

            // 최소 세 개 이상 점을 찍었을 때 

            // AddNewLabel 함수 호출

            // 도형이 만들어지지 못하면 exit
            // SaveBool = false; OKBool = False;

            // [TODO] Exception: 도형을 이루지 못하는 점들의 위지 관계 e.g, 세 점이 한 직선에 나란히

            OKBool = false;
            ImageBool = false;

            SolidColorBrush newlabelBrush = new SolidColorBrush(newLabelColor);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawImage(MainBmpImage, new Rect(0, 0, BmpWidth, BmpHeight));

                Pen pen = new Pen(newlabelBrush, 5);

                double x1 = ClickedPosition.ElementAt(0)[0];
                double y1 = ClickedPosition.ElementAt(0)[1];

                double x2 = ClickedPosition.ElementAt(ClickedPosition.Count - 1)[0];
                double y2 = ClickedPosition.ElementAt(ClickedPosition.Count - 1)[1];

                dc.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));

            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(BmpWidth, BmpHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            MainBmpImage = rtb;



            ObservableCollection<int[]> newCornerPoints = ConvertToIntPos(ClickedPosition);


            // 자료구조에 반영하는 부분


            // 인자는 ClickPoint에서 넘어오는 것을 간주함.

            #region Update table item source

            // Check if new label name exist or not
            int ack = 0;
            for (int i = 0; i < ClassPoints.Count; i++)
            {

                if(ClassPoints[i].ClassName == newLabelName)
                {
                    // If new class name exiset in ClassPoint (table item source)
                    ack = 1;
                    ClassPoints[i].NumPoints += PixelCalculator(newCornerPoints);


                    break;

                }
            }

            // New class name doesn't exist in ClassPoint (table item source)
            if(ack == 0)
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


            // 생성: new ClassCornerPoints() { ClassName = newLabelName, Points = newCornerPoints })
            ClassCornerPoints newLabelPoints = new ClassCornerPoints() { ClassName = newLabelName, Points = newCornerPoints };

            // dictionary에 csv path 가 있는 경우
            
            if (CornerPoint.ContainsKey(dirPath + '\\' + "metadata.csv"))
            {
                // csv path가 있으면,
                // path key 로 value 찾고 
                // value.Add( 생성했던 ClassCornerPoints)
                CornerPoint[dirPath + '\\' + "metadata.csv"].Add(newLabelPoints);
                
            }

            // dictionary에 csv path가 없는 경우: new csv file 생성해야 함. 
            
            // 여기 아직 확인 안 함!
            else
            {
                // dictionary.Add(csv 새로운 path, new ObservableCollection<ClassCornerPoints>{생성한 ClassCornerPoints})
                CornerPoint.Add(dirPath + '\\' + "metadata.csv", new ObservableCollection<ClassCornerPoints> { newLabelPoints });
            }

            ClickedPosition.Clear();
            newLabelColor = Colors.Transparent;



        }
        #endregion


        public void DrawLabel()
        {
            // 1. 현재 csv file path 가져옴

            // 2. dictionary IsContain 해보기

            // 3. 없으면 끝, 있으면 다음

            // 4. ObserableCollection 반복하면서 그림 그리기
            if (String.IsNullOrEmpty(CsvPath))
            {
                CsvPath = GetParentDirPath() + '\\' + "metadata.csv";
            }

            if (CornerPoint.ContainsKey(CsvPath))
            {
                foreach (ClassCornerPoints classCornerPoints in CornerPoint[CsvPath])
                {
                    DrawingVisual dv = new DrawingVisual();
                    using (DrawingContext dc = dv.RenderOpen())
                    {
                        SolidColorBrush classBrush = new SolidColorBrush(SearchColor(classCornerPoints.ClassName));
                        // TODO: ClassPoints 에서 classname 검색해서 해당 색깔 가져오기

                        dc.DrawImage(MainBmpImage, new Rect(0, 0, BmpWidth, BmpHeight));

                        // 점, 선 반복 그리기


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

                        // 마지막 점 이랑 선
                        var x_last = classCornerPoints.Points[i][0];
                        var y_last = classCornerPoints.Points[i][1];
                        var x_first = classCornerPoints.Points[0][0];
                        var y_first = classCornerPoints.Points[0][1];
                        dc.DrawEllipse(classBrush, null, new Point(x_last, y_last), 5, 5);
                        dc.DrawLine(pen, new Point(x_last, y_last), new Point(x_first, y_first));

                    }

                    RenderTargetBitmap rtb = new RenderTargetBitmap(BmpWidth, BmpHeight, 96, 96, PixelFormats.Pbgra32);
                    rtb.Render(dv);

                    MainBmpImage = rtb;
                }
            }
            
        }

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

            // Dictionary loop
            foreach(KeyValuePair<string, ObservableCollection<ClassCornerPoints>> collection in CornerPoint)
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

                        // value.count loop
                        for (int i = 0; i < collection.Value.Count; i++)
                        {
                            //value[i].ClassName
                            string line = collection.Value[i].ClassName;

                            //value[i].Points loop
                            for (int j = 0; j < collection.Value[i].Points.Count; j++)
                            {
                                //value[i].Points[j][,]

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

        }
        #endregion

        private BitmapSource StringToBmpSource(string path)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(@path); //put in a valid path to an image or use your image you get from the array
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(fi.FullName, UriKind.Absolute);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            return src;
        }

    }
}
