using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ManagedBass;
using Newtonsoft.Json;

namespace Akorin.Models
{
    public class Settings : ISettings
    {
        private bool init = false;
        public Settings()
        {
            ReadUnicode = true;
            SplitWhitespace = true;
            RecListFile = Path.Combine("reclists", "default_reclist.txt");
            NotesFile = Path.Combine("voicebank", "default_notes.json");
            DestinationFolder = "voicebank";

            Bass.Init();
            Bass.RecordInit();
            AudioDriver = Bass.GetDeviceInfo(Bass.CurrentDevice).Driver;
            AudioInputDevice = Bass.GetDeviceInfo(Bass.CurrentDevice).Name;
            AudioInputLevel = 100;
            AudioOutputDevice = Bass.GetDeviceInfo(Bass.CurrentRecordingDevice).Name;
            AudioOutputLevel = 100;

            FontSize = 24;

            init = true;
            LoadRecList();
            LoadNotes();
        }

        private string recListFile;
        public string RecListFile
        {
            get
            {
                return recListFile;
            }
            set
            {
                recListFile = value;
                LoadRecList();
            }
        }

        private bool readUnicode;
        public bool ReadUnicode
        {
            get { return readUnicode; }
            set
            {
                readUnicode = value;
                LoadRecList();
            }
        }

        private bool splitWhitespace;
        public bool SplitWhitespace
        {
            get { return splitWhitespace; }
            set
            {
                splitWhitespace = value;
                LoadRecList();
            }
        }

        private ObservableCollection<RecListItem> recList;
        public ObservableCollection<RecListItem> RecList
        {
            get { return recList; }
        }
        public void LoadRecList()
        {
            if (init)
            {
                string[] textArr;
                recList = new ObservableCollection<RecListItem>();

                if (SplitWhitespace)
                {
                    var rawText = File.ReadAllText(RecListFile);
                    rawText = Regex.Replace(rawText, @"\s{2,}", " ");
                    textArr = Regex.Split(rawText, @"\s");
                }
                else
                {
                    textArr = File.ReadAllLines(RecListFile);
                }

                foreach (string line in textArr)
                {
                    recList.Add(new RecListItem(this, line));
                }
            }
        }

        private string notesFile;
        public string NotesFile
        {
            get
            {
                return notesFile;
            }
            set
            {
                notesFile = value;
                LoadNotes();
            }
        }

        private Dictionary<string, string> notes;
        public Dictionary<string, string> Notes
        {
            get { return notes; }
        }
        public void LoadNotes()
        {
            if (init)
            {
                var rawText = File.ReadAllText(NotesFile);
                notes = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawText);
                foreach (var x in RecList)
                {
                    if (!notes.ContainsKey(x.Text))
                    {
                        notes.Add(x.Text, "");
                    }
                }
            }
        }

        public string DestinationFolder { get; set; }

        public string AudioDriver { get; set; }

        public string AudioInputDevice { get; set; }

        private int _audioInputLevel;
        public int AudioInputLevel
        {
            get { return _audioInputLevel; }
            set
            {
                if (value >= 0 && value <= 100)
                    _audioInputLevel = value;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string AudioOutputDevice { get; set; }

        private int _audioOutputLevel;
        public int AudioOutputLevel 
        { 
            get { return _audioOutputLevel; } 
            set
            {
                if (value >= 0 && value <= 100)
                    _audioOutputLevel = value;
                else
                    throw new ArgumentOutOfRangeException();
            } 
        }

        private int fontSize;
        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if (value >= 8 && value <= 200)
                    fontSize = value;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
