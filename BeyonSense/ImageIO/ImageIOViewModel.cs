using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BeyonSense
{
    class ImageIOViewModel
    {
        private Color color;

        public Color colour
        {
            get { return color; }

            set { color = Color.FromRgb(100,100,100); }
        }



    }
}
