using System;
using Prism.Regions;

namespace ZV200Utility.Core.Mvvm
{
    /// <summary>
    /// ViewModel предоставляющая возможность методов навигации.
    /// </summary>
    public class RegionViewModelBase : ViewModelBase, IConfirmNavigationRequest
    {
        /// <inheritdoc/>
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext,
            Action<bool> continuationCallback) => continuationCallback(true);

        /// <inheritdoc/>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <inheritdoc/>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <inheritdoc/>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}