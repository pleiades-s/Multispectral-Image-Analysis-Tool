using System;
using System.Windows;

namespace BeyonSense.Views
{
    /// <summary>
    /// Interaction logic for PopupView.xaml
    /// </summary>
    public partial class PopupView : Window
    {
		public PopupView(string defaultClassName = "")
		{
			InitializeComponent();
			txtAnswer.Text = defaultClassName;
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			txtAnswer.SelectAll();
			txtAnswer.Focus();
		}

		public string Answer
		{
			get { return txtAnswer.Text; }
		}

	}
}
