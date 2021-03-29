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
            AudioInputDevice = Bass.CurrentDevice;
            AudioInputLevel = 100;
            AudioOutputDevice = Bass.CurrentRecordingDevice;
            AudioOutputLevel = 100;
        }

        public string RecListFile { get; set; }
        public bool ReadUnicode { get; set; }
        public bool SplitWhitespace { get; set; }

        public ObservableCollection<RecListItem> RecList => throw new NotImplementedException();
        public string DestinationFolder { get; set; }
        public string AudioDriver { get; set; }
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

        public int FontSize { get; set; }

        List<string> ISettings.AudioInputDeviceList => throw new NotImplementedException();

        List<string> ISettings.AudioOutputDeviceList => throw new NotImplementedException();
    }
}
