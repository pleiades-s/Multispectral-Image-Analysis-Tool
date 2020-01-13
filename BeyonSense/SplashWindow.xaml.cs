using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BeyonSense
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        DispatcherTimer dT = new DispatcherTimer();

        public SplashWindow()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Splash screen when the window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region animation setting

            DoubleAnimation doubleanimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(3)));
            doubleanimation.EasingFunction = new QuarticEase();

            // Close splash screen after the animation ends
            doubleanimation.Completed += doubleanimation_Completed;

            SplashImage.BeginAnimation(OpacityProperty, doubleanimation);

            #endregion

        }

        #region Naviage MainWindow
        /// <summary>
        /// Navigation Function is called right after the splash animation ends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void doubleanimation_Completed(object sender, EventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();


        }
        #endregion
    }
}
