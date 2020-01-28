﻿using System;
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
    /// Interaction logic for TablePlusButton.xaml
    /// </summary>
    public partial class TablePlusButton : UserControl
    {
        public TablePlusButton()
        {
            InitializeComponent();
            PlusBool = false;
        }

        private bool plusBool = false;
        public bool PlusBool { get => plusBool; set => plusBool = value; }

        private void Plus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PlusBool = !PlusBool;

        }
    }
}
