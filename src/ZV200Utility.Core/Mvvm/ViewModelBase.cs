using Prism.Mvvm;
using Prism.Navigation;

namespace ZV200Utility.Core.Mvvm
{
    /// <summary>
    /// Базовая ViewModel.
    /// </summary>
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        /// <inheritdoc />
        public virtual void Destroy()
        {
        }
    }
}