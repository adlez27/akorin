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
        private MainWindowViewModel main;

        public SettingsWindowViewModel(Window window, string tab, ISettings s, MainWindowViewModel m)
        {
            this.window = window;
            this.tab = tab;
            settings = s;
            main = m;

            validDict = new Dictionary<string, bool>();

            validDict.Add("RecListFile", true);
            recListFile = settings.RecListFile;
            reclistContentValid = "";

            validDict.Add("ReadUnicode", true);
            readUnicode = settings.ReadUnicode;

            validDict.Add("SplitWhitespace", true);
            splitWhitespace = settings.SplitWhitespace;

            validDict.Add("DestinationFolder", true);
            destinationFolder = settings.DestinationFolder;
            newFolder = false;

            validDict.Add("AudioInputDevice", true);
            audioInputDevice = settings.AudioInputDevice;

            validDict.Add("AudioInputLevel", true);
            audioInputLevel = settings.AudioInputLevel;

            validDict.Add("AudioOutputDevice", true);
            audioOutputDevice = settings.AudioOutputDevice;

            validDict.Add("AudioOutputLevel", true);
            audioOutputLevel = settings.AudioOutputLevel;

            fontSize = settings.FontSize;
            validDict.Add("FontSize", true);

            validDict.Add("WaveformEnabled", true);
            waveformEnabled = settings.WaveformEnabled;
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
            set
            {
                this.RaiseAndSetIfChanged(ref recListFile, value);
            }
        }

        private bool reclistFromFile
        {
            get
            {
                return (RecListFile != "List loaded from default.") && (RecListFile != "List loaded from project file.");
            }
        }

        private bool newRecList;
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
                LoadRecList(recListFile[0]);
            }
        }

        private void LoadRecList(string path)
        {
            Settings temp = new Settings(true);
            temp.ReadUnicode = ReadUnicode;
            temp.SplitWhitespace = SplitWhitespace;

            bool validContent = true;
            try
            {
                temp.RecListFile = path;
            }
            catch (Exception e)
            {
                validContent = false;
            }

            string invalid = "";

            if (validContent)
            {
                foreach (RecListItem item in temp.RecList)
                {
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        if (item.Text.Contains(c))
                        {
                            validContent = false;
                            invalid += c;
                            break;
                        }
                    }
                }
            }

            if (validContent)
            {
                ReclistContentValid = "";
                validDict["RecListFile"] = true;
                RecListFile = path;
                newRecList = true;
            }
            else
            {
                ReclistContentValid = $"Reclist contains invalid character: {invalid}";
                validDict["RecListFile"] = false;
            }
            this.RaisePropertyChanged("Valid");
        }

        private string reclistContentValid;
        public string ReclistContentValid
        {
            get { return reclistContentValid; }
            set { this.RaiseAndSetIfChanged(ref reclistContentValid, value); }
        }

        private bool readUnicode;
        public bool ReadUnicode
        {
            get => readUnicode;
            set 
            {
                this.RaiseAndSetIfChanged(ref readUnicode, value);
                if (reclistFromFile)
                {
                    LoadRecList(RecListFile);
                }
            }
        }

        private bool splitWhitespace;
        public bool SplitWhitespace
        {
            get => splitWhitespace;
            set 
            { 
                this.RaiseAndSetIfChanged(ref splitWhitespace, value);
                if (reclistFromFile)
                {
                    LoadRecList(RecListFile);
                }
            }
        }

        private bool newFolder;

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
                newFolder = true;
                this.RaisePropertyChanged("DestinationFolder");
                main.RaisePropertyChanged("RecList");
            }
        }

        private int audioInputDevice;
        public int AudioInputDevice
        {
            get => audioInputDevice;
            set { this.RaiseAndSetIfChanged(ref audioInputDevice, value); }
        }

        private int audioInputLevel;
        public int AudioInputLevel
        {
            get => audioInputLevel;
            set { this.RaiseAndSetIfChanged(ref audioInputLevel, value); }
        }

        private int audioOutputDevice;
        public int AudioOutputDevice
        {
            get => audioOutputDevice;
            set { this.RaiseAndSetIfChanged(ref audioOutputDevice, value); }
        }

        private int audioOutputLevel;
        public int AudioOutputLevel
        {
            get => audioOutputLevel;
            set { this.RaiseAndSetIfChanged(ref audioOutputLevel, value); }
        }

        private int fontSize;
        public int FontSize
        {
            get => fontSize;
            set
            { this.RaiseAndSetIfChanged(ref fontSize, value); }
        }

        private bool waveformEnabled;
        public bool WaveformEnabled
        {
            get => waveformEnabled;
            set { this.RaiseAndSetIfChanged(ref waveformEnabled, value); }
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
            settings.ReadUnicode = ReadUnicode;
            settings.SplitWhitespace = SplitWhitespace;
            if (newRecList)
            {
                settings.RecList.Clear();
                settings.RecListFile = RecListFile;
                main.SelectedLine = settings.RecList[0];
            }
            if (newFolder)
            {
                settings.DestinationFolder = DestinationFolder;
                main.SelectedLine = main.SelectedLine;
            }

            settings.AudioInputDevice = AudioInputDevice;
            settings.AudioInputLevel = AudioInputLevel;
            settings.AudioOutputDevice = AudioOutputDevice;
            settings.AudioOutputLevel = AudioOutputLevel;
            settings.FontSize = FontSize;
            settings.WaveformEnabled = WaveformEnabled;

            main.FontSize = FontSize;
            
            settings.SaveSettings(path);
        }

        public void CloseSettings()
        {
            window.Close();
        }
    }
}