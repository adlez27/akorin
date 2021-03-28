using Akorin.Models;
using Akorin.Views;
using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using ReactiveUI;
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

        public async void SelectRecordingList()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Directory = settings.RecListFile;
            var recListFile = await openFileDialog.ShowAsync(window);
            if (recListFile.Length > 0)
            {
                settings.RecListFile = recListFile[0];
            }
        }
        public async void SelectNotesFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Directory = settings.NotesFile;
            var notesFile = await openFileDialog.ShowAsync(window);
            if (notesFile.Length > 0)
            {
                settings.NotesFile = notesFile[0];
            }
        }
        public async void SelectDestinationFolder()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Directory = settings.DestinationFolder;
            var destinationFolder = await openFolderDialog.ShowAsync(window);
            if (destinationFolder.Length > 0)
            {
                settings.DestinationFolder = destinationFolder;
            }
        }

        public void CloseSettings()
        {
            window.Close();
        }
    }
}