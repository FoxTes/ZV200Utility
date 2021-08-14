using System;
using System.Windows;
using ModernWpf.Controls;
using ZV200Utility.Core.Services;

namespace ZV200Utility.Views
{
    /// <summary>
    /// Взаимодействие логики для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INavigationJournal _navigationJournal;

        /// <inheritdoc />
        public MainWindow(INavigationJournal navigationJournal)
        {
            _navigationJournal = navigationJournal;
            InitializeComponent();
        }

        private void Window_OnContentRendered(object sender, EventArgs e)
        {
            InvalidateMeasure();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            _navigationJournal.RegionNavigationJournal.GoBack();
            _navigationJournal.UpdateNavigationJournal();
        }
    }
}
