using System;
using System.Windows;
using System.Windows.Threading;
using Prism.Regions;
using ZV200Utility.Core;
using ZV200Utility.Core.Enums;
using ZV200Utility.Core.Mvvm;
using ZV200Utility.Core.Services;
using ZV200Utility.Services.DeviceManager;

namespace ZV200Utility.ViewModels
{
    /// <summary>
    /// Модель представления для MainWindow.xaml.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationJournal _navigationJournal;
        private readonly IDeviceManager _deviceManager;
        private readonly IRegionManager _regionManager;

        private bool _isGoBack;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainWindowViewModel"/>.
        /// </summary>
        /// <param name="navigationJournal">Журнал.</param>
        /// <param name="deviceManager">2</param>
        /// <param name="regionManager">3</param>
        public MainWindowViewModel(
            INavigationJournal navigationJournal,
            IDeviceManager deviceManager,
            IRegionManager regionManager)
        {
            _navigationJournal = navigationJournal;
            _navigationJournal.RegionChanged += OnNavigationJournalOnRegionChanged;

            _deviceManager = deviceManager;
            _regionManager = regionManager;
            _deviceManager.StatusConnectChanged += OnDeviceManagerOnStatusConnectChanged;
        }

        /// <summary>
        /// Флаг, указывающий на возможность обратной навигации.
        /// </summary>
        public bool IsGoBack
        {
            get => _isGoBack;
            set => SetProperty(ref _isGoBack, value);
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            _navigationJournal.RegionChanged -= OnNavigationJournalOnRegionChanged;
            base.Destroy();
        }

        private void OnDeviceManagerOnStatusConnectChanged(object sender, EventArgs args)
        {
            if (_deviceManager.StatusConnect != StatusConnect.Disconnected)
                return;

            _navigationJournal.RegionNavigationJournal = new RegionNavigationJournal();
            _navigationJournal.UpdateNavigationJournal();

            Application.Current.Dispatcher.BeginInvoke(
                () => { _regionManager.RequestNavigate(RegionNames.MainContent, "StatusRelaysWindow"); },
                DispatcherPriority.Background);
        }

        private void OnNavigationJournalOnRegionChanged(object sender, EventArgs args)
        {
            IsGoBack = _navigationJournal.RegionNavigationJournal.CanGoBack;
        }
    }
}
