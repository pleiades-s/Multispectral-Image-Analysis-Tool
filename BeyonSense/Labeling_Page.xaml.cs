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
    /// Interaction logic for Labeling_Page.xaml
    /// </summary>
    public partial class Labeling_Page : Page
    {
        public Labeling_Page()
        {
            InitializeComponent();
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Navigation_Page navigation_page = new Navigation_Page();

            this.NavigationService.Navigate(navigation_page);
        }

    }
}
