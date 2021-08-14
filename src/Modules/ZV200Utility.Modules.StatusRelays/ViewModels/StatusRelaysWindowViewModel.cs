using System;
using System.Windows;
using System.Windows.Threading;
using Prism.Regions;
using ZV200Utility.Core;
using ZV200Utility.Core.Enums;
using ZV200Utility.Core.Mvvm;
using ZV200Utility.Services.DeviceManager;

namespace ZV200Utility.Modules.StatusRelays.ViewModels
{
    /// <summary>
    /// Модель представления для StatusRelaysWindow.xaml.
    /// </summary>
    public class StatusRelaysWindowViewModel : RegionViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IDeviceManager _deviceManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StatusRelaysWindowViewModel"/>.
        /// </summary>
        /// <param name="regionManager">Менеджер регионов.</param>
        /// <param name="deviceManager">Менеджер прибора.</param>
        public StatusRelaysWindowViewModel(IRegionManager regionManager, IDeviceManager deviceManager)
        {
            _regionManager = regionManager;
            _deviceManager = deviceManager;
            _deviceManager.StatusConnectChanged += OnDeviceManagerOnStatusConnectChanged;
        }

        private void OnDeviceManagerOnStatusConnectChanged(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    _regionManager.RequestNavigate(RegionNames.StatusRelayContent,
                        _deviceManager.StatusConnect == StatusConnect.Connected ? "ConnectWindow" : "NoConnectWindow");
                },
                DispatcherPriority.Background);
        }
    }
}
