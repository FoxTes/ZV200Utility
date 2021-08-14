using Prism.Commands;
using Prism.Regions;
using ZV200Utility.Core;
using ZV200Utility.Core.Mvvm;

namespace ZV200Utility.Modules.StatusRelays.ViewModels
{
    /// <summary>
    /// Модель представления для NoConnectWindowViewModel.xaml.
    /// </summary>
    public class NoConnectWindowViewModel : RegionViewModelBase
    {
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NoConnectWindowViewModel"/>.
        /// </summary>
        /// <param name="regionManager">Менеджер регионов.</param>
        public NoConnectWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            SettingViewCommand = new DelegateCommand(SettingViewSubmit);
        }

        /// <summary>
        /// Команда переключения представления на настройки.
        /// </summary>
        public DelegateCommand SettingViewCommand { get; }

        private void SettingViewSubmit()
        {
            _regionManager.RequestNavigate(RegionNames.MainContent, "SettingWindow");
        }
    }
}