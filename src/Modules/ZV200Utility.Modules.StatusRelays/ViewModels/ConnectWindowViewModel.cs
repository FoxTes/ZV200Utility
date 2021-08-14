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

        private ObservableCollection<Phase> _phases = new ObservableCollection<Phase>
        {
            new Phase(false),
            new Phase(false),
            new Phase(false),
        };
        private ObservableCollection<Phase> _elements = new ObservableCollection<Phase>
        {
            new Phase(false),
            new Phase(false),
        };

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

            SettingViewCommand = new DelegateCommand(SettingViewSubmit);
        }

        /// <summary>
        /// Группа реле.
        /// </summary>
        public ObservableCollection<Phase> Phases
        {
            get => _phases;
            set => SetProperty(ref _phases, value);
        }

        /// <summary>
        /// Группа реле.
        /// </summary>
        public ObservableCollection<Phase> Elements
        {
            get => _elements;
            set => SetProperty(ref _elements, value);
        }

        /// <summary>
        /// Команда переключения представления на настройки.
        /// </summary>
        public DelegateCommand SettingViewCommand { get; }

        private void OnDeviceManagerOnRegistersRequested(object sender, IReadOnlyList<SensorInfoArgs> list)
        {
            var dataSensor = list
                .Select(x => new Phase(x.Status))
                .ToList();

            _phases.Clear();
            Phases.AddRange(dataSensor.GetRange(0, 3));

            _elements.Clear();
            Elements.AddRange(dataSensor.GetRange(3, 2));
        }

        private void SettingViewSubmit()
        {
            _regionManager.RequestNavigate(RegionNames.MainContent, "SettingWindow");
        }
    }
}