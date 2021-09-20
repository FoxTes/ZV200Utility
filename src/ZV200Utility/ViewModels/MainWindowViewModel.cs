using System;
using Prism.Commands;
using ZV200Utility.Core.Mvvm;
using ZV200Utility.Core.Services;

namespace ZV200Utility.ViewModels
{
    /// <summary>
    /// Модель представления для MainWindow.xaml.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationJournal _navigationJournal;

        private bool _isGoBack;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainWindowViewModel"/>.
        /// </summary>
        /// <param name="navigationJournal">Журнал навигации.</param>
        public MainWindowViewModel(
            INavigationJournal navigationJournal)
        {
            _navigationJournal = navigationJournal;
            _navigationJournal.RegionChanged += OnNavigationJournalOnRegionChanged;

            GoBackCommand = new DelegateCommand(GoBackSubmit);
        }

        /// <summary>
        /// Команда возврата страницы.
        /// </summary>
        public DelegateCommand GoBackCommand { get; }

        /// <summary>
        /// Флаг, указывающий на возможность обратной навигации.
        /// </summary>
        public bool IsGoBack
        {
            get => _isGoBack;
            set => SetProperty(ref _isGoBack, value);
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            _navigationJournal.RegionChanged -= OnNavigationJournalOnRegionChanged;
            base.Destroy();
        }

        private void GoBackSubmit()
        {
            _navigationJournal.RegionNavigationJournal.GoBack();
            _navigationJournal.UpdateNavigationJournal();
        }

        private void OnNavigationJournalOnRegionChanged(object sender, EventArgs args) 
            => IsGoBack = _navigationJournal.RegionNavigationJournal.CanGoBack;
    }
}
