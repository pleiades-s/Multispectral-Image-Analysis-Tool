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
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {
        public ToggleSwitch()
        {
            InitializeComponent();
            Back.Fill = Off;
            toggled = false;
            Toggle.Margin = LeftSide;
        }

        Thickness LeftSide = new Thickness(-39, 0, 0, 0);
        Thickness RightSide = new Thickness(39, 0, 0, 0);

        SolidColorBrush On = new SolidColorBrush(Color.FromRgb(20, 160, 255));
        SolidColorBrush Off = new SolidColorBrush(Color.FromRgb(160, 160, 160));

        private bool toggled = false;
        public bool Toggled { get => toggled; set => toggled = value; }

        private void Toggle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // On
            if (!toggled)
            {
                Back.Fill = On;
                toggled = true;
                Toggle.Margin = RightSide;
            }

            // Off
            else
            {
                Back.Fill = Off;
                toggled = false;
                Toggle.Margin = LeftSide;
            }
        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // On
            if (!toggled)
            {
                Back.Fill = On;
                toggled = true;
                Toggle.Margin = RightSide;
            }

            // Off
            else
            {
                Back.Fill = Off;
                toggled = false;
                Toggle.Margin = LeftSide;
            }
        }
    }
}
