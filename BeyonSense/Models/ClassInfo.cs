using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BeyonSense.Models
{
    public class ClassInfo
    {
        public string ClassName { get; set; }

        public List<int[]> Points { get; set; }

        public Color TextColor { get; set; }

        public int NumPoints { get; set; }

        public void PointCalculator()
        {
            NumPoints = 100;
        }
    }
}
