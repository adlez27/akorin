using Akorin.Models;
using Akorin.Views;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Akorin.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IView _view;
        private ISettings settings;
        public MainWindowViewModel() { }

        public MainWindowViewModel(IView view, ISettings s)
        {
            _view = view;
            settings = s;
        }

        public int FontSize
        {
            get { return settings.FontSize; }
        }

        public ObservableCollection<RecListItem> RecList
        {
            get
            {
                return settings.RecList;
            }
        }

        private RecListItem selectedLine;
        public RecListItem SelectedLine
        {
            get => selectedLine;
            set => this.RaiseAndSetIfChanged(ref selectedLine, value);
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void OpenSettings(string tab)
        {
            var settingsWindow = new SettingsWindow(tab, settings);
            settingsWindow.ShowDialog((Window)_view);
        }
    }
}