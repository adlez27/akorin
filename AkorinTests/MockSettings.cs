using Akorin.Models;
using ManagedBass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkorinTests
{
    class MockSettings : ISettings
    {
        public MockSettings()
        {
            Bass.Init();
            Bass.RecordInit();
            AudioDriver = Bass.GetDeviceInfo(Bass.CurrentDevice).Driver;
            AudioInputDevice = Bass.GetDeviceInfo(Bass.CurrentDevice).Name;
            AudioInputLevel = 100;
            AudioOutputDevice = Bass.GetDeviceInfo(Bass.CurrentRecordingDevice).Name;
            AudioOutputLevel = 100;
        }

        public string RecListFile { get; set; }
        public bool ReadUnicode { get; set; }
        public bool SplitWhitespace { get; set; }

        public ObservableCollection<RecListItem> RecList => throw new NotImplementedException();

        public string NotesFile { get; set; }
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

        public int FontSize { get; set; }
        public bool WaveformEnabled { get; set; }

        Dictionary<string, string> ISettings.Notes => throw new NotImplementedException();
    }
}
