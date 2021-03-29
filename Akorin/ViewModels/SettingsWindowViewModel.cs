using Akorin.Models;
using Akorin.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Akorin.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        private Window window;
        private string tab;
        private ISettings settings;

        public ISettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
            }
        }

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

        public string RecListFile
        {
            get => settings.RecListFile;
        }

        public async void SelectRecordingList()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Directory = Path.GetDirectoryName(settings.RecListFile);
            var recListFile = await openFileDialog.ShowAsync(window);
            if (recListFile.Length > 0)
            {
                settings.RecListFile = recListFile[0];
                this.RaisePropertyChanged("RecListFile");
            }
        }

        public string NotesFile
        {
            get => settings.NotesFile;
        }

        public async void SelectNotesFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Directory = Path.GetDirectoryName(settings.NotesFile);
            var notesFile = await openFileDialog.ShowAsync(window);
            if (notesFile.Length > 0)
            {
                settings.NotesFile = notesFile[0];
                this.RaisePropertyChanged("NotesFile");
            }
        }

        public string DestinationFolder
        {
            get => settings.DestinationFolder;
        }

        public async void SelectDestinationFolder()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Directory = settings.DestinationFolder;
            var destinationFolder = await openFolderDialog.ShowAsync(window);
            if (destinationFolder.Length > 0)
            {
                settings.DestinationFolder = destinationFolder;
                this.RaisePropertyChanged("DestinationFolder");
            }
        }

        public int FontSize
        {
            get => settings.FontSize;
            set
            {
                settings.FontSize = value;
                this.RaisePropertyChanged("FontSize");
            }
        }
        
        public bool ReadUnicode
        {
            get => settings.ReadUnicode;
            set
            {
                settings.ReadUnicode = value;
                this.RaisePropertyChanged("ReadUnicode");
            }
        }

        public bool SplitWhiteSpace
        {
            get => settings.SplitWhitespace;
            set
            {
                settings.SplitWhitespace = value;
                this.RaisePropertyChanged("SplitWhiteSpace");
            }
        }

        public void CloseSettings()
        {
            window.Close();
        }
    }

    public class BoolInverterConverter : IValueConverter
    {
        //From https://stackoverflow.com/a/3361553
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }

        #endregion
    }
}