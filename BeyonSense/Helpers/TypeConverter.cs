using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BeyonSense.Helpers
{
    public static class TypeConverter
    {
        #region Convert String into Integer
        /// <summary>
        /// Converter: String to Integer
        /// </summary>
        /// <param name="str">string num</param>
        /// <returns>int num</returns>
        public static int StrToInt(string str)
        {

            if (!Int32.TryParse(str, out int i))
            {
                i = -1;
            }
            return i;
        }

        #endregion

        #region String Path to Bitmap Source
        public static BitmapSource StringToBmpSource(string path, int BmpWidth = 800, int BmpHeight = 608)
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

        #region Covert double collection into integer collection
        public static List<int[]> ConvertToIntPos(ObservableCollection<double[]> corners)
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
    }
}
