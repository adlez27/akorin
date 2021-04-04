using Akorin.Models;
using Akorin.Views;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Akorin.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        private Window window;
        private string tab;
        private ISettings settings { get; set; }
        private MainWindowViewModel main;

        public SettingsWindowViewModel(Window window, string tab, ISettings s, MainWindowViewModel m, bool newProject)
        {
            this.window = window;
            this.tab = tab;
            settings = s;
            main = m;

            validDict = new Dictionary<string, bool>();

            validDict.Add("RecListFile", true);
            recListFile = settings.RecListFile;
            reclistContentValid = "";
            newRecList = false;

            validDict.Add("ReadUnicode", true);
            readUnicode = settings.ReadUnicode;

            validDict.Add("SplitWhitespace", true);
            splitWhitespace = settings.SplitWhitespace;

            recListFromFolder = false;

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

            validDict.Add("FontSize", true);
            fontSize = settings.FontSize;

            validDict.Add("WaveformColor", true);
            waveformColor = settings.WaveformColor;

            validDict.Add("VisualizerEnabled", true);
            waveformEnabled = settings.WaveformEnabled;

            projectFile = settings.ProjectFile;

            if (newProject)
                NewProject();
        }

        public SettingsWindowViewModel() { }

        private bool filesSelected;
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
            all.Extensions = new List<string>() { "arl", "csv", "reclist", "ust", "txt" };

            var arl = new FileDialogFilter();
            arl.Name = "Akorin recording list";
            arl.Extensions = new List<string>() { "arl" };

            var oremo = new FileDialogFilter();
            oremo.Name = "OREMO comment";
            oremo.Extensions = new List<string>() { "txt" };

            var csv = new FileDialogFilter();
            csv.Name = "ARPAsing index";
            csv.Extensions = new List<string>() { "csv" };

            var wct = new FileDialogFilter();
            wct.Name = "WavConfigTool reclist";
            wct.Extensions = new List<string>() { "reclist" };

            var ust = new FileDialogFilter();
            ust.Name = "UTAU sequence text";
            ust.Extensions = new List<string>() { "ust" };

            var txt = new FileDialogFilter();
            txt.Name = "Plain text";
            txt.Extensions = new List<string>() { "txt" };

            openFileDialog.Filters = new List<FileDialogFilter>() { all, arl, oremo, csv, wct, ust, txt };

            openFileDialog.Directory = Path.GetDirectoryName(settings.RecListFile);
            var recListFile = await openFileDialog.ShowAsync(window);
            if (recListFile.Length > 0)
            {
                LoadRecList(recListFile[0]);
            }
            else
            {
                validDict["RecListFile"] = false;
                this.RaisePropertyChanged("Valid");
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

            string invalidChar = "";
            bool containsInvalidChar = false;

            if (validContent)
            {
                foreach (RecListItem item in temp.RecList)
                {
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        if (item.Text.Contains(c))
                        {
                            validContent = false;
                            containsInvalidChar = true;
                            invalidChar += c;
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
                recList = temp.RecList;
            }
            else
            {
                if (containsInvalidChar)
                    ReclistContentValid = $"Reclist contains invalid character: {invalidChar}";
                else
                    ReclistContentValid = "Could not load reclist.";
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
            set
            {
                this.RaiseAndSetIfChanged(ref destinationFolder, value);
                newFolder = true;
            }
        }

        public async void SelectDestinationFolder()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Directory = settings.DestinationFolder;
            var selectedFolder = await openFolderDialog.ShowAsync(window);
            if (selectedFolder.Length > 0)
            {
                DestinationFolder = selectedFolder;
                validDict["DestinationFolder"] = true;
                main.RaisePropertyChanged("RecList");
            } else
            {
                validDict["DestinationFolder"] = false;
            }
            this.RaisePropertyChanged("Valid");
        }

        private bool recListFromFolder;
        private ObservableCollection<RecListItem> recList;
        public void GenerateRecListFromFolder()
        {
            recList = new ObservableCollection<RecListItem>();
            var allFiles = Directory.GetFiles(DestinationFolder);
            foreach(var file in allFiles)
            {
                if (Path.GetExtension(file) == ".wav")
                    recList.Add(new RecListItem(settings,Path.GetFileNameWithoutExtension(file)));
            }
            newRecList = true;
            recListFromFolder = true;
            RecListFile = "List generated from files in folder.";
        }

        public async void ExportRecList()
        {
            var arlContent = new Dictionary<string, string>();
            if (newRecList)
            {
                foreach (var item in recList)
                    arlContent.Add(item.Text, item.Note);
            }
            else
            {
                foreach (var item in settings.RecList)
                    arlContent.Add(item.Text, item.Note);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Directory = settings.DestinationFolder;
            saveFileDialog.DefaultExtension = "arl";

            var arl = new FileDialogFilter();
            arl.Name = "Akorin recording list";
            arl.Extensions = new List<string>() { "arl" };
            saveFileDialog.Filters = new List<FileDialogFilter>() { arl };

            var filename = await saveFileDialog.ShowAsync(window);
            using (StreamWriter file = new(filename))
            {
                var serializer = new YamlDotNet.Serialization.Serializer();
                serializer.Serialize(file, arlContent);
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
            set
            {
                this.RaiseAndSetIfChanged(ref audioOutputDevice, value);
                validDict["AudioOutputDevice"] = value != 0;
                this.RaisePropertyChanged("Valid");
            }
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
            set
            {
                this.RaiseAndSetIfChanged(ref waveformEnabled, value);
                validDict["VisualizerEnabled"] = visualizerEnabled;
                this.RaisePropertyChanged("Valid");
            }
        }

        public List<string> WaveformColorList
        {
            get
            {
                Type type = typeof(Color);
                PropertyInfo[] propInfoList = type.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
                var colorList = new List<string>();
                foreach (PropertyInfo c in propInfoList)
                {
                    colorList.Add(c.Name);
                }
                return colorList;
            }
        }

        private string waveformColor;
        public string WaveformColor
        {
            get => waveformColor;
            set
            {
                this.RaiseAndSetIfChanged(ref waveformColor, value);
                validDict["WaveformColor"] = waveformColor != "Transparent";
                this.RaisePropertyChanged("Valid");
            }
        }

        private bool visualizerEnabled
        {
            get
            {
                return WaveformEnabled; // later on it'll be WaveformEnabled or SpectrogramEnabled
            }
        }

        private string projectFile { get; set; }

        private Dictionary<string, bool> validDict;
        public bool Valid
        {
            get
            {
                return !validDict.ContainsValue(false);
            }
        }

        public void NewProject()
        {
            Settings newProject = new Settings(true);

            RecListFile = newProject.RecListFile;
            ReadUnicode = newProject.ReadUnicode;
            SplitWhitespace = newProject.SplitWhitespace;
            DestinationFolder = newProject.DestinationFolder;

            AudioInputDevice = newProject.AudioInputDevice;
            AudioInputLevel = newProject.AudioInputLevel;
            AudioOutputDevice = newProject.AudioOutputDevice;
            AudioOutputLevel = newProject.AudioOutputLevel;

            FontSize = newProject.FontSize;
            WaveformEnabled = newProject.WaveformEnabled;
            WaveformColor = newProject.WaveformColor;

            projectFile = "";
        }

        public void SetDefault()
        {
            var currentDirectory = AppContext.BaseDirectory;
            var defaultSettings = Path.Combine(currentDirectory, "default.arp");
            SaveSettings(defaultSettings);
        }

        public async void OKSettings()
        {
            if (projectFile == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Directory = settings.DestinationFolder;
                saveFileDialog.DefaultExtension = "arp";

                var arp = new FileDialogFilter();
                arp.Name = "Akorin recording project";
                arp.Extensions = new List<string>() { "arp" };
                saveFileDialog.Filters = new List<FileDialogFilter>() { arp };

                projectFile = await saveFileDialog.ShowAsync(window);
            }
            if (projectFile is null)
            {
                //Undo changes, and return to the previous state.
                projectFile = "";
            }
            else
            {
                SaveSettings(projectFile);
                main.Status = "Settings saved.";
                window.Close();
            }
        }

        public void SaveSettings(string path)
        {
            settings.ReadUnicode = ReadUnicode;
            settings.SplitWhitespace = SplitWhitespace;
            if (newRecList)
            {
                settings.RecListFile = RecListFile;
                settings.RecList.Clear();
                foreach (var item in recList)
                    settings.RecList.Add(item);
                main.SelectedLineIndex = 0;
            }
            if (newFolder)
            {
                settings.DestinationFolder = DestinationFolder;
                foreach(RecListItem item in settings.RecList)
                {
                    item.Audio.Read();
                    item.RaisePropertyChanged("Audio");
                }
                main.SelectedLineIndex = main.SelectedLineIndex;
            }

            settings.AudioInputDevice = AudioInputDevice;
            settings.AudioInputLevel = AudioInputLevel;
            settings.AudioOutputDevice = AudioOutputDevice;
            settings.AudioOutputLevel = AudioOutputLevel;

            settings.FontSize = FontSize;
            main.FontSize = FontSize;
            settings.WaveformEnabled = WaveformEnabled;
            settings.WaveformColor = WaveformColor;
            main.WaveformColor = Color.FromName(WaveformColor);

            settings.ProjectFile = projectFile;

            settings.SaveSettings(path);
        }

        public void CloseSettings()
        {
            window.Close();
        }
    }
}