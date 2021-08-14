using System;
using Prism.Regions;

namespace ZV200Utility.Core.Services
{
    /// <inheritdoc />
    public class NavigationJournal : INavigationJournal
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NavigationJournal"/>.
        /// </summary>
        /// <param name="journal">Журнал.</param>
        public NavigationJournal(IRegionNavigationJournal journal)
        {
            RegionNavigationJournal = journal;
        }

        /// <inheritdoc />
        public event EventHandler RegionChanged;

        /// <inheritdoc />
        public IRegionNavigationJournal RegionNavigationJournal { get; set; }

        /// <inheritdoc />
        public void UpdateNavigationJournal() => RegionChanged?.Invoke(this, null!);
    }
}