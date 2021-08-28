using System;
using System.Windows;

namespace ZV200Utility.Views
{
    /// <summary>
    /// Взаимодействие логики для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_OnContentRendered(object sender, EventArgs e)
        {
            InvalidateMeasure();
        }
    }
}
