using Prism.Ioc;
using Prism.Modularity;
using ZV200Utility.Modules.Setting.ViewModels;
using ZV200Utility.Modules.Setting.Views;

namespace ZV200Utility.Modules.Setting
{
    /// <inheritdoc />
    public class SettingModule : IModule
    {
        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SettingWindow, SettingWindowViewModel>();
        }
    }
}