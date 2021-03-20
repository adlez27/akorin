using Akorin.Models;
using Akorin.Views;
using Avalonia.Controls;
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
            //settings.SplitWhitespace = false;
        }

        public ObservableCollection<RecListItem> RecList
        {
            get
            {
                return settings.RecList;
            }
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