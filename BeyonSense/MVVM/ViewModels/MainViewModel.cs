using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace BeyonSense
{
    /// <summary>
    /// The view model for the applications main Directory view
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// A list of all directories on the machine
        /// </summary>
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        // Test
        private Color color;

        public Color Colour
        {
            get { return color; }
            set { color = value; }
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



        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel(string path)
        {

            // Get the logical drives
            var children = DirectoryStructure.GetRootFolder(path);

            // Create the view models from the data
            this.Items = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(root => new DirectoryItemViewModel(root.FullPath, DirectoryItemType.Folder)));

            /// TODO: THIS LINES WILL BE REMOVED.
            /// THESE LINES ARE ONLY FOR TEST
            List<Color> new_color = ColorGenerator(10);
            this.Colour = new_color[9];
            ///
        }

        #endregion




    }
}
