using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ManagedBass;
using Microsoft.VisualBasic.FileIO;
using ReactiveUI;
using YamlDotNet.Serialization;

namespace Akorin.Models
{
    public class Settings : ISettings
    {
        private bool init = false;
        private string defaultRecListRaw = @"- Text: あ
  Note: a
- Text: い
  Note: i
- Text: う
  Note: u
- Text: え
  Note: e
- Text: お
  Note: o
- Text: ん
  Note: n
- Text: か
  Note: ka
- Text: き
  Note: ki
- Text: く
  Note: ku
- Text: け
  Note: ke
- Text: こ
  Note: ko
- Text: きゃ
  Note: kya
- Text: きゅ
  Note: kyu
- Text: きょ
  Note: kyo
- Text: が
  Note: ga
- Text: ぎ
  Note: gi
- Text: ぐ
  Note: gu
- Text: げ
  Note: ge
- Text: ご
  Note: go
- Text: ぎゃ
  Note: gya
- Text: ぎゅ
  Note: gyu
- Text: ぎょ
  Note: gyo
- Text: さ
  Note: sa
- Text: し
  Note: shi
- Text: す
  Note: su
- Text: せ
  Note: se
- Text: そ
  Note: so
- Text: しゃ
  Note: sha
- Text: しゅ
  Note: shu
- Text: しょ
  Note: sho
- Text: ざ
  Note: za
- Text: じ
  Note: ji
- Text: ず
  Note: zu
- Text: ぜ
  Note: ze
- Text: ぞ
  Note: zo
- Text: じゃ
  Note: ja
- Text: じゅ
  Note: ju
- Text: じょ
  Note: jo
- Text: た
  Note: ta
- Text: ち
  Note: chi
- Text: つ
  Note: tsu
- Text: て
  Note: te
- Text: と
  Note: to
- Text: ちゃ
  Note: cha
- Text: ちゅ
  Note: chu
- Text: ちょ
  Note: cho
- Text: だ
  Note: da
- Text: で
  Note: de
- Text: ど
  Note: do
- Text: な
  Note: na
- Text: に
  Note: ni
- Text: ぬ
  Note: nu
- Text: ね
  Note: ne
- Text: の
  Note: no
- Text: にゃ
  Note: nya
- Text: にゅ
  Note: nyu
- Text: にょ
  Note: nyo
- Text: は
  Note: ha
- Text: ひ
  Note: hi
- Text: ふ
  Note: fu
- Text: へ
  Note: he
- Text: ほ
  Note: ho
- Text: ひゃ
  Note: hya
- Text: ひゅ
  Note: hyu
- Text: ひょ
  Note: hyo
- Text: ば
  Note: ba
- Text: び
  Note: bi
- Text: ぶ
  Note: bu
- Text: べ
  Note: be
- Text: ぼ
  Note: bo
- Text: びゃ
  Note: bya
- Text: びゅ
  Note: byu
- Text: びょ
  Note: byo
- Text: ぱ
  Note: pa
- Text: ぴ
  Note: pi
- Text: ぷ
  Note: pu
- Text: ぺ
  Note: pe
- Text: ぽ
  Note: po
- Text: ぴゃ
  Note: pya
- Text: ぴゅ
  Note: pyu
- Text: ぴょ
  Note: pyo
- Text: ま
  Note: ma
- Text: み
  Note: mi
- Text: む
  Note: mu
- Text: め
  Note: me
- Text: も
  Note: mo
- Text: みゃ
  Note: mya
- Text: みゅ
  Note: myu
- Text: みょ
  Note: myo
- Text: や
  Note: ya
- Text: ゆ
  Note: yu
- Text: よ
  Note: yo
- Text: ら
  Note: ra
- Text: り
  Note: ri
- Text: る
  Note: ru
- Text: れ
  Note: re
- Text: ろ
  Note: ro
- Text: りゃ
  Note: rya
- Text: りゅ
  Note: ryu
- Text: りょ
  Note: ryo
- Text: わ
  Note: wa
- Text: を
  Note: wo";

        public Settings() { }

        public Settings(bool startup)
        {
            Bass.Init();
            Bass.RecordInit();
            recList = new ObservableCollection<RecListItem>();
            LoadDefault();
            init = true;
        }

        public void LoadDefault()
        {
            var currentDirectory = AppContext.BaseDirectory;
            var defaultSettings = Path.Combine(currentDirectory, "default.arp");

            if (File.Exists(defaultSettings))
            {
                LoadSettings(defaultSettings);
            }
            else
            {
                ReadUnicode = false;
                SplitWhitespace = true;
                DestinationFolder = Path.Combine(currentDirectory, "voicebank");

                AudioInputLevel = 100;
                AudioOutputLevel = 100;

                FontSize = 24;
                WaveformEnabled = true;
                WaveformColor = "Blue";

                var deserializer = new Deserializer();
                var defaultRecList = deserializer.Deserialize<ObservableCollection<RecListItem>>(defaultRecListRaw);
                foreach (RecListItem item in defaultRecList)
                {
                    item.CreateAudio(this);
                    RecList.Add(item);
                }

                SaveSettings(defaultSettings);
            }

            ProjectFile = "";
            recListFile = "List loaded from default.";
        }

        private string recListFile;
        [YamlIgnore]
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
        [YamlIgnore]
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
        [YamlIgnore]
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
            set { recList = value; }
        }
        public void LoadRecList()
        {
            if (init)
            {
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
                    if (Path.GetFileName(RecListFile) == "OREMO-comment.txt")
                    {
                        var rawText = File.ReadAllLines(RecListFile, e);
                        foreach(string rawLine in rawText)
                        {
                            var line = rawLine.Split("\t");
                            if (!uniqueStrings.Contains(line[0]))
                            {
                                RecList.Add(new RecListItem(this, line[0], line[1]));
                                uniqueStrings.Add(line[0]);
                            }
                        }
                    }
                    else
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
                                RecList.Add(new RecListItem(this, line));
                                uniqueStrings.Add(line);
                            }
                        }
                    }
                }
                else if (ext == ".arl")
                {
                    var rawText = File.ReadAllText(RecListFile, e);
                    var deserializer = new YamlDotNet.Serialization.Deserializer();
                    var tempDict = deserializer.Deserialize<Dictionary<string, string>>(rawText);
                    foreach (var item in tempDict)
                    {
                        RecList.Add(new RecListItem(this, item.Key, item.Value));
                    }
                }
                else if (ext == ".csv")
                {
                    using (TextFieldParser parser = new TextFieldParser(RecListFile))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        while (!parser.EndOfData)
                        {
                            string[] line = parser.ReadFields();
                            var text = line[0].Substring(0,line[0].Length - 4);
                            if (!uniqueStrings.Contains(text))
                            {
                                RecList.Add(new RecListItem(this, text, line[1]));
                                uniqueStrings.Add(text);
                            }
                        }
                    }
                    CopyIndex();
                }
            }
        }

        public void CopyIndex()
        {
            var vbIndex = Path.Combine(DestinationFolder, "index.csv");
            if (Path.GetFileName(RecListFile) == "index.csv" && !File.Exists(vbIndex))
            {
                File.Copy(RecListFile, vbIndex);
            }
        }

        private string destinationFolder;
        public string DestinationFolder
        {
            get => destinationFolder;
            set
            {
                destinationFolder = value;
                if (init)
                {
                    foreach(RecListItem item in RecList)
                    {
                        item.Audio.Unload();
                    }
                }
                CopyIndex();
            }
        }

        public string AudioDriver
        {
            get => Bass.GetDeviceInfo(Bass.CurrentDevice).Driver;
            set { }
        }

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

        public int AudioInputDevice
        {
            get => Bass.CurrentRecordingDevice;
            set
            {
                Bass.RecordInit(value);
                Bass.CurrentRecordingDevice = value;
            }
        }

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

        public int AudioOutputDevice
        {
            get => Bass.CurrentDevice;
            set
            {
                Bass.Init(value);
                Bass.CurrentDevice = value;
            }
        }

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

        private bool waveformEnabled;
        public bool WaveformEnabled
        {
            get { return waveformEnabled; }
            set
            {
                waveformEnabled = value;
            }
        }

        public string WaveformColor { get; set; }

        [YamlIgnore]
        public string ProjectFile { get; set; }

        public void LoadSettings(string path)
        {
            ProjectFile = path;

            var raw = File.ReadAllText(path);
            var deserializer = new Deserializer();
            var newSettings = deserializer.Deserialize<Settings>(raw);

            recListFile = "List loaded from project file.";
            DestinationFolder = newSettings.DestinationFolder;

            AudioDriver = newSettings.AudioDriver;
            AudioInputDevice = newSettings.AudioInputDevice;
            AudioInputLevel = newSettings.AudioInputLevel;
            AudioOutputDevice = newSettings.AudioOutputDevice;
            AudioOutputLevel = newSettings.AudioOutputLevel;

            FontSize = newSettings.FontSize;
            WaveformEnabled = newSettings.WaveformEnabled;
            WaveformColor = newSettings.WaveformColor;

            recList.Clear();
            foreach (RecListItem item in newSettings.RecList)
            {
                item.CreateAudio(this);
                RecList.Add(item);
            }
        }

        public void SaveSettings(string path)
        {
            using (StreamWriter file = new(path))
            {
                var serializer = new YamlDotNet.Serialization.Serializer();
                serializer.Serialize(file, this);
            }
        }
    }
}
