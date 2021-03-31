using Akorin.Models;
using Akorin.Views;
using Avalonia;
using Avalonia.Controls;
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
            recListFile = settings.RecListFile;
            reclistContentValid = "";
            valid = false;
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

        private string recListFile;
        public string RecListFile
        {
            get => recListFile;
            set { this.RaiseAndSetIfChanged(ref recListFile, value); }
        }

        public async void SelectRecordingList()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AllowMultiple = false;

            var all = new FileDialogFilter();
            all.Name = "All supported formats";
            all.Extensions = new List<string>() { "arl", "txt" };

            var arl = new FileDialogFilter();
            arl.Name = "Akorin recording list";
            arl.Extensions = new List<string>() { "arl" };

            var txt = new FileDialogFilter();
            txt.Name = "Plain text";
            txt.Extensions = new List<string>() { "txt" };

            openFileDialog.Filters = new List<FileDialogFilter>() { all, arl, txt };

            openFileDialog.Directory = Path.GetDirectoryName(settings.RecListFile);
            var recListFile = await openFileDialog.ShowAsync(window);
            if (recListFile.Length > 0)
            {
                var oldFile = settings.RecListFile;
                var oldList = settings.RecList;
                settings.RecListFile = recListFile[0];

                bool validContent = true;
                foreach (RecListItem item in settings.RecList)
                {
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        if (item.Text.Contains(c))
                        {
                            validContent = false;
                            break;
                        }
                    }
                }

                if (validContent)
                {
                    ReclistContentValid = "";
                    RecListFile = recListFile[0];
                }
                else
                {
                    RecListFile = oldFile;
                    settings.RecListFile = oldFile;
                    settings.RecList = oldList;
                    ReclistContentValid = "Reclist contains invalid characters.";
                }
            }
        }

        private string reclistContentValid;
        public string ReclistContentValid
        {
            get { return reclistContentValid; }
            set { this.RaiseAndSetIfChanged(ref reclistContentValid, value); }
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

        private bool valid;
        public bool Valid
        {
            get { return valid; }
            set
            {
                this.RaiseAndSetIfChanged(ref valid, value);
            }
        }

        public void CloseSettings()
        {
            window.Close();
        }
    }
}