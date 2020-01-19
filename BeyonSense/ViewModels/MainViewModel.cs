using BeyonSense.Models;
using Caliburn.Micro;
using Microsoft.Win32;
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
                    PointCalculator(rootPath);

                    // TODO: Dictionary Clear
                    // TODO: ObservableCollection<ClassPoint> Clear

                    // Color, NumPoints Clear
                    Colour = Color.FromArgb(0, 255, 255, 255);
                    NumPoints = "";
                    csvFilePaths.Clear();
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

        #region Csv path variables
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
                    LoadBoundary(csvPath);
                }

                else
                {
                    // Reset the table if there is no csv file in the selected folder
                    // ClassPoints.Clear();
                }

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

        public Dictionary<string, ObservableCollection<ClassBoundary>> BoundaryPoint = new Dictionary<string, ObservableCollection<ClassBoundary>>();

        #endregion

        #region Walking Directory
        private List<string> _csvFilePaths = new List<string>();
        public List<string> csvFilePaths
        {
            get { return _csvFilePaths; }
            set
            {
                _csvFilePaths = value;
            }
        }

        public void DirSearch(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        Console.WriteLine(f);

                        // Csv file
                        if (Path.GetExtension(f) == ".csv")
                        {
                            csvFilePaths.Add(f);
                            Console.WriteLine(f);
                        }
                    }
                    DirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
        #endregion

        public int StrToInt(string str)
        {
            int i = 0;
            if (!Int32.TryParse(str, out i))
            {
                i = -1;
            }
            return i;
        }

        #region Point Calculator

        // https://csharpindepth.com/Articles/Random

        public Random rnd = new Random();

        public int PixelCalculator(ObservableCollection<int[]> _boundaryPoints)
        {
            // TODO
            int returnvalue = rnd.Next(100, 300);
            Console.WriteLine("Random " + returnvalue.ToString());
            return returnvalue;
        }

        public void PointCalculator(string _rootPath)
        {
            // [This method is called when a folder is selected by folder explorer]

            // Traverse all the directory and find all existing csv file paths
            DirSearch(_rootPath);

            // For each csv file
            foreach (string _path in csvFilePaths)
            {
                // Read class name and boundary points
                ObservableCollection<ClassBoundary> _classBoundaries = new ObservableCollection<ClassBoundary>();


                using (var reader = new StreamReader(_path))
                {
                    string _className="";
                    ObservableCollection<int[]> _boundaryPoints = new ObservableCollection<int[]>();
                    int _numLine = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (_numLine != 0)
                        {
                            var values = line.Split(',');
                                                  
                            _className = values[0];
                            //Console.WriteLine(values[0]);

                            for (int i = 1; i < values.Length; i += 2)
                            {

                                int[] _position = new int[2];
                                // x position
                                _position[0] = StrToInt(values[i]);

                                // y position
                                _position[1] = StrToInt(values[i + 1]);

                                //Console.WriteLine("x: " + values[i] + " y: " + values[i + 1] + '\n');
                                _boundaryPoints.Add(_position);
                            }

                            // Each line = each class
                            _classBoundaries.Add(new ClassBoundary() { ClassName = _className, Points = _boundaryPoints });
                        }

                        _numLine++;


                    }
                }


                // Allocate new dictionary { Class name: Boundary point Collection<T> } <-- We'd start from here if user add new boundary
                // Each csv file
                BoundaryPoint.Add(_path, _classBoundaries);


                // TODO: Calculate the number of inside points and Add ClassPoints 
                
                for(int i = 0; i < _classBoundaries.Count; i++)
                {

                    //_classBoundaries 에서 ClassPoints에 이름이 있는지 없는지 확인
                    int ack = 0;

                    for(int j = 0; j < ClassPoints.Count; j++)
                    {
                        if(_classBoundaries[i].ClassName == ClassPoints[j].ClassName)
                        {
                            // If there is same class name, add the value
                            ack = 1;
                            ClassPoints[j].NumPoints += PixelCalculator(_classBoundaries[i].Points);

                        }
                    }

                    // New class
                    // If not, make new class and allocate the value
                    if (ack == 0)
                    {
                        ClassPoints.Add(new ClassPixels()
                        {
                            ClassName = _classBoundaries[i].ClassName,
                            NumPoints = PixelCalculator(_classBoundaries[i].Points)
                        });
                    }

                }
                
                

            }

            //_classBoundaries의 원소만큼 color 생성
            int k = ClassPoints.Count;
            List<Color> _color = ColorGenerator(k);

            for(int i = 0; i < k; i++)
            {
                ClassPoints[i].ClassColor = _color[i];
            }


        }

        #endregion

        #region Load Boundary

        public void LoadBoundary(string path)
        {

            // Read Dictionary

            // For each class
                
                // Find value of boundary points in the dictionary

                // Show the boundary on MainView

                // Specify color of boundary is same with the class
            
        }
        #endregion


    }
}
