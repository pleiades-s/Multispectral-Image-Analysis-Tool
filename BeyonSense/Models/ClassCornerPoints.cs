using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BeyonSense.Models
{
    /// <summary>
    /// ClassName and Corner points to draw over image
    /// </summary>
    public class ClassCornerPoints
    {
        public string ClassName { get; set; }

        public ObservableCollection<int[]> Points { get; set; }

    }
}
