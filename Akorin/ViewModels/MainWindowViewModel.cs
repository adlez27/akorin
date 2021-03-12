using Akorin.Views;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Akorin.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // ref: https://stackoverflow.com/questions/47266184/getting-window-from-view-model
        private readonly IView _view;
        public MainWindowViewModel(IView view)
        {
            _view = view;
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void OpenSettings(string tab)
        {
            var settings = new SettingsWindow();
            settings.ShowDialog((Window)_view);
        }
    }
}
