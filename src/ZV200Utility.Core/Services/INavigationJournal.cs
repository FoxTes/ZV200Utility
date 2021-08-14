using System;
using Prism.Regions;

namespace ZV200Utility.Core.Services
{
    /// <summary>
    /// Предоставляет общий журнал навигации для приложения.
    /// </summary>
    public interface INavigationJournal
    {
        /// <summary>
        /// Событие, возникающие при изменение состояние подключения к прибору.
        /// </summary>
        event EventHandler RegionChanged;

        /// <summary>
        /// Предоставляет или устанавливает журнал навигации.
        /// </summary>
        IRegionNavigationJournal RegionNavigationJournal { get; set; }

        /// <summary>
        /// Запускает событие <see cref="NavigationJournal.RegionChanged"/>.
        /// </summary>
        void UpdateNavigationJournal();
    }
}