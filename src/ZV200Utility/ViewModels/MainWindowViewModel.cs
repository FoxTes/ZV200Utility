using System;
using System.Windows;
using System.Windows.Threading;
using Prism.Commands;
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
        /// <param name="navigationJournal">Журнал навигации.</param>
        /// <param name="deviceManager">Менеджер прибора.</param>
        /// <param name="regionManager">Менеджер регионов.</param>
        public MainWindowViewModel(
            INavigationJournal navigationJournal,
            IDeviceManager deviceManager,
            IRegionManager regionManager)
        {
            _navigationJournal = navigationJournal;
            _navigationJournal.RegionChanged += OnNavigationJournalOnRegionChanged;

            _regionManager = regionManager;
            _deviceManager = deviceManager;
            _deviceManager.StatusConnectChanged += OnDeviceManagerOnStatusConnectChanged;

            GoBackCommand = new DelegateCommand(GoBackSubmit);
        }

        /// <summary>
        /// Команда возврата страницы.
        /// </summary>
        public DelegateCommand GoBackCommand { get; }

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

            Application.Current.Dispatcher.BeginInvoke(
                () => { _regionManager.RequestNavigate(RegionNames.MainContent, "StatusRelaysWindow"); },
                DispatcherPriority.Background);

            _navigationJournal.RegionNavigationJournal.Clear();
            _navigationJournal.UpdateNavigationJournal();
        }

        private void GoBackSubmit()
        {
            _navigationJournal.RegionNavigationJournal.GoBack();
            _navigationJournal.UpdateNavigationJournal();
        }

        private void OnNavigationJournalOnRegionChanged(object sender, EventArgs args) 
            => IsGoBack = _navigationJournal.RegionNavigationJournal.CanGoBack;
    }
}
