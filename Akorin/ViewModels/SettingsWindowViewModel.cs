using Akorin.Models;
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
        private string tab;
        private ISettings settings;

        public SettingsWindowViewModel(Window window, string tab, ISettings s)
        {
            this.window = window;
            this.tab = tab;
            settings = s;
        }

        public SettingsWindowViewModel() { }

        public bool FilesSelected
        {
            get
            {
                return tab == "files";
            }
        }

        public bool AudioSelected
        {
            get
            {
                return tab == "audio";
            }
        }

        public bool DisplaySelected
        {
            get
            {
                return tab == "display";
            }
        }

        public void CloseSettings()
        {
            window.Close();
        }
    }
}