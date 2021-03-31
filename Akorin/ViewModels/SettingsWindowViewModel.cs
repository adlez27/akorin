using Akorin.Models;
using Akorin.Views;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
            destinationFolder = settings.DestinationFolder;

            validDict = new Dictionary<string, bool>();
            validDict.Add("RecListFile", true);
            //validDict.Add("ReadUnicode", false);
            //validDict.Add("SplitWhitespace", false);
            validDict.Add("DestinationFolder", true);
            //validDict.Add("AudioInputDevice", false);
            //validDict.Add("AudioInputLevel", false);
            //validDict.Add("AudioOutputDevice", false);
            //validDict.Add("AudioOutputLevel", false);
            //validDict.Add("FontSize", false);
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
                    validDict["RecListFile"] = true;
                    RecListFile = recListFile[0];
                }
                else
                {
                    RecListFile = oldFile;
                    settings.RecListFile = oldFile;
                    settings.RecList = oldList;
                    ReclistContentValid = "Reclist contains invalid characters.";
                    validDict["RecListFile"] = false;
                }
                this.RaisePropertyChanged("Valid");
            }
        }

        private string reclistContentValid;
        public string ReclistContentValid
        {
            get { return reclistContentValid; }
            set { this.RaiseAndSetIfChanged(ref reclistContentValid, value); }
        }

        private string destinationFolder;
        public string DestinationFolder
        {
            get => destinationFolder;
        }

        public async void SelectDestinationFolder()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Directory = settings.DestinationFolder;
            var selectedFolder = await openFolderDialog.ShowAsync(window);
            if (selectedFolder.Length > 0)
            {
                destinationFolder = selectedFolder;
                validDict["DestinationFolder"] = true;
                this.RaisePropertyChanged("DestinationFolder");
            }
        }

        private Dictionary<string, bool> validDict;
        public bool Valid
        {
            get
            {
                return !validDict.ContainsValue(false);
            }
        }

        public void SetDefault()
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var defaultSettings = Path.Combine(currentDirectory, "default.arp");
            SaveSettings(defaultSettings);
        }

        public async void OKSettings()
        {
            if (settings.ProjectFile == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Directory = settings.DestinationFolder;
                saveFileDialog.DefaultExtension = "arp";

                var arp = new FileDialogFilter();
                arp.Name = "Akorin recording project";
                arp.Extensions = new List<string>() { "arp" };
                saveFileDialog.Filters = new List<FileDialogFilter>() { arp };

                settings.ProjectFile = await saveFileDialog.ShowAsync(window);
            }
            SaveSettings(settings.ProjectFile);
            window.Close();
        }

        public void SaveSettings(string path)
        {
            // set reclistfile
            // set readunicode
            // set splitwhitespace
            settings.DestinationFolder = destinationFolder;
            // set input device
            // set input level
            // set output device
            // set output level
            // set font size

            settings.SaveSettings(path);
        }

        public void CloseSettings()
        {
            window.Close();
        }
    }
}