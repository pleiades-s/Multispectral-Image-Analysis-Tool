using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeyonSense
{
    /// <summary>
    /// Interaction logic for Navigation_Page.xaml
    /// </summary>
    public partial class Navigation_Page : Page
    {
        public Navigation_Page()
        {
            InitializeComponent();
        }
        private void Labeling_Click(object sender, RoutedEventArgs e)
        {
            Labeling_Page labeling_page = new Labeling_Page();

            this.NavigationService.Navigate(labeling_page);
        }
    }
}
