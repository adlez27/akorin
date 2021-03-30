using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ManagedBass;
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
  Note: byo
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
  Note: wo
- Text: ん
  Note: n";

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
            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var defaultSettings = Path.Combine(currentDirectory, "default.arp");

            if (File.Exists(defaultSettings))
            {
                LoadSettings(defaultSettings);
            }
            else
            {
                DestinationFolder = Path.Combine(currentDirectory, "voicebank");

                AudioDriver = Bass.GetDeviceInfo(Bass.CurrentDevice).Driver;
                AudioInputDevice = Bass.CurrentRecordingDevice;
                AudioInputLevel = 100;
                AudioOutputDevice = Bass.CurrentDevice;
                AudioOutputLevel = 100;

                FontSize = 24;

                var deserializer = new Deserializer();
                var defaultRecList = deserializer.Deserialize<ObservableCollection<RecListItem>>(defaultRecListRaw);
                foreach (RecListItem item in defaultRecList)
                {
                    item.CreateAudio(this);
                    recList.Add(item);
                }

                SaveSettings(defaultSettings);
            }

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

        public void LoadSettings(string path)
        {
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

            recList.Clear();
            foreach (RecListItem item in newSettings.RecList)
            {
                item.CreateAudio(this);
                recList.Add(item);
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
