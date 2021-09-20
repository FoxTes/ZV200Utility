using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Prism.Commands;
using Prism.Regions;
using ZV200Utility.Core;
using ZV200Utility.Core.Mvvm;
using ZV200Utility.Modules.StatusRelays.Models;
using ZV200Utility.Services.DeviceManager;
using ZV200Utility.Services.DeviceManager.Model;

namespace ZV200Utility.Modules.StatusRelays.ViewModels
{
    /// <summary>
    /// Модель представления для ConnectWindowViewModel.xaml.
    /// </summary>
    public class ConnectWindowViewModel : RegionViewModelBase
    {
        private static readonly object Lock = new object();

        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ConnectWindowViewModel"/>.
        /// </summary>
        /// <param name="regionManager">Менеджер регионов.</param>
        /// <param name="deviceManager">Менеджер прибора.</param>
        public ConnectWindowViewModel(IRegionManager regionManager, IDeviceManager deviceManager)
        {
            _regionManager = regionManager;
            deviceManager.RegistersRequested += OnDeviceManagerOnRegistersRequested;
            BindingOperations.EnableCollectionSynchronization(Phases, Lock);
            BindingOperations.EnableCollectionSynchronization(Elements, Lock);

            SettingViewCommand = new DelegateCommand(SettingViewSubmit);
        }

        /// <summary>
        /// Группа реле.
        /// </summary>
        public ObservableCollection<Phase> Phases { get; } = new ObservableCollection<Phase>
        {
            new Phase(false),
            new Phase(false),
            new Phase(false),
        };

        /// <summary>
        /// Группа реле.
        /// </summary>
        public ObservableCollection<Phase> Elements { get; } = new ObservableCollection<Phase>
        {
            new Phase(false),
            new Phase(false),
        };

        /// <summary>
        /// Команда переключения представления на настройки.
        /// </summary>
        public DelegateCommand SettingViewCommand { get; }

        private void OnDeviceManagerOnRegistersRequested(object sender, IReadOnlyList<SensorInfoArgs> list)
        {
            var dataSensor = list
                .Select(x => new Phase(x.Status))
                .ToList();

            var selectRange = dataSensor.GetRange(0, 3);
            for (var i = 0; i < selectRange.Count; i++)
                Phases[i] = new Phase(selectRange[i].Status);

            selectRange = dataSensor.GetRange(4, 2);
            for (var i = 0; i < selectRange.Count; i++)
                Elements[i] = new Phase(selectRange[i].Status);
        }

        private void SettingViewSubmit()
        {
            _regionManager.RequestNavigate(RegionNames.MainContent, "SettingWindow");
        }
    }
}