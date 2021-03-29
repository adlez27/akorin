﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ManagedBass;
using YamlDotNet.Serialization;

namespace Akorin.Models
{
    public class Settings : ISettings
    {
        private bool init = false;
        public Settings()
        {
            ReadUnicode = false;
            SplitWhitespace = true;
            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            RecListFile = Path.Combine(currentDirectory,"reclists", "default_reclist.arl");
            DestinationFolder = Path.Combine(currentDirectory, "voicebank");

            Bass.Init();
            Bass.RecordInit();
            AudioDriver = Bass.GetDeviceInfo(Bass.CurrentDevice).Driver;
            AudioInputDevice = Bass.CurrentRecordingDevice;
            AudioInputLevel = 100;
            AudioOutputDevice = Bass.CurrentDevice;
            AudioOutputLevel = 100;

            FontSize = 24;

            init = true;
            LoadRecList();
            Save(Path.Combine(currentDirectory, "settings.arp"));
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
                recList = new ObservableCollection<RecListItem>();
                HashSet<string> uniqueStrings = new HashSet<string>();

                Encoding e;
                if (ReadUnicode)
                {
                    e = Encoding.UTF8;
                }
                else
                {
                    e = CodePagesEncodingProvider.Instance.GetEncoding(932);
                }

                var ext = Path.GetExtension(RecListFile);
                if (ext == ".txt")
                {
                    string[] textArr;
                    if (SplitWhitespace)
                    {
                        var rawText = File.ReadAllText(RecListFile, e);
                        rawText = Regex.Replace(rawText, @"\s{2,}", " ");
                        textArr = Regex.Split(rawText, @"\s");
                    }
                    else
                    {
                        textArr = File.ReadAllLines(RecListFile, e);
                    }

                    foreach (string line in textArr)
                    {
                        if (!uniqueStrings.Contains(line))
                        {
                            recList.Add(new RecListItem(this, line));
                            uniqueStrings.Add(line);
                        }
                    }
                }
                else if (ext == ".arl")
                {
                    var rawText = File.ReadAllText(RecListFile);
                    var deserializer = new YamlDotNet.Serialization.Deserializer();
                    var tempDict = deserializer.Deserialize<Dictionary<string, string>>(rawText);
                    foreach (var item in tempDict)
                    {
                        recList.Add(new RecListItem(this, item.Key, item.Value));
                    }
                }
            }
        }

        public string DestinationFolder { get; set; }

        public string AudioDriver { get; set; }

        [YamlIgnore]
        public List<string> AudioInputDeviceList
        {
            get
            {
                var temp = new List<string>();
                if (init)
                {
                    for (var i = 0; i < Bass.RecordingDeviceCount; i++)
                    {
                        temp.Add(Bass.RecordGetDeviceInfo(i).Name);
                    }
                }
                return temp;
            }
        }

        public int AudioInputDevice { get; set; }

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

        [YamlIgnore]
        public List<string> AudioOutputDeviceList
        {
            get
            {
                var temp = new List<string>();
                if (init)
                {
                    for (var i = 0; i < Bass.DeviceCount; i++)
                    {
                        temp.Add(Bass.GetDeviceInfo(i).Name);
                    }
                }
                return temp;
            }
        }

        public int AudioOutputDevice { get; set; }

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

        public void Load()
        {
            // load settings from file
        }

        public void Save(string path)
        {
            using (StreamWriter file = new(path))
            {
                var serializer = new YamlDotNet.Serialization.Serializer();
                serializer.Serialize(file, this);
            }
        }
    }
}
