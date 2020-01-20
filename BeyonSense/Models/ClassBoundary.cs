using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BeyonSense.Models
{
    public class ClassBoundary
    {
        public string ClassName { get; set; }

        public ObservableCollection<int[]> Points { get; set; }

    }
}
