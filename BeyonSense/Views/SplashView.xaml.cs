using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BeyonSense.Views
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashView : Window
    {
        DispatcherTimer dT = new DispatcherTimer();

        public SplashView()
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

            DoubleAnimation doubleanimation = new DoubleAnimation(1, 1, new Duration(TimeSpan.FromSeconds(2)));
            //doubleanimation.EasingFunction = new QuarticEase();

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
            MainView mainView = new MainView();
            mainView.Show();


            this.Close();


        }
        #endregion


    }
}
