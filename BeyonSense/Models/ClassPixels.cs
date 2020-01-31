using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BeyonSense.Models
{
    /// <summary>
    /// Each item for the table on MainView
    /// </summary>
    public class ClassPixels : Screen
    {
        public string ClassName { get; set; }

        // ObservableCollection will notify the changed only if any element is added or removed, not any changed variable in the element.

        private int numPoints;

        public int NumPoints 
        {
            get { return numPoints; }
            set
            {
                numPoints = value;
                NotifyOfPropertyChange(() => NumPoints);
            }
        
        }


        private Color classColor = Colors.Transparent;

        public Color ClassColor
        {
            get { return classColor; }
            set
            {
                classColor = value;
            }
        }

    }
}
