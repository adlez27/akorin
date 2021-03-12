using Akorin.Views;
using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Akorin.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        private Window window;

        public SettingsWindowViewModel(Window window)
        {
            this.window = window;
        }
        public void CloseSettings()
        {
            window.Close();
        }
    }
}
