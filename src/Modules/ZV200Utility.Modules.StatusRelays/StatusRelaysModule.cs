using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ZV200Utility.Core;
using ZV200Utility.Modules.StatusRelays.ViewModels;
using ZV200Utility.Modules.StatusRelays.Views;

namespace ZV200Utility.Modules.StatusRelays
{
    /// <inheritdoc />
    public class StatusRelaysModule : IModule
    {
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StatusRelaysModule"/>.
        /// </summary>
        /// <param name="regionManager">Менеджер регионов.</param>
        public StatusRelaysModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.MainContent, "StatusRelaysWindow");
            _regionManager.RequestNavigate(RegionNames.StatusRelayContent, "NoConnectWindow");
        }

        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StatusRelaysWindow, StatusRelaysWindowViewModel>();
            containerRegistry.RegisterForNavigation<ConnectWindow, ConnectWindowViewModel>();
            containerRegistry.RegisterForNavigation<NoConnectWindow, NoConnectWindowViewModel>();
        }
    }
}