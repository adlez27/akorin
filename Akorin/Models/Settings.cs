using System;
using ManagedBass;

namespace Akorin.Models
{
    public class Settings
    {
        public Settings()
        {
            Bass.Init();
            Bass.RecordInit();
            DestinationFolder = "voicebank";
            AudioInputLevel = 100;
            AudioOutputLevel = 100;
        }

        public string DestinationFolder { get; set; }

        public string AudioDriver { get; set; }

        public string AudioInputDevice { get; set; }

        private int _audioInputLevel;
        public int AudioInputLevel
        {
            get
            {
                return _audioInputLevel;
            }
            set
            {
                if (value >= 0 && value <= 100)
                    _audioInputLevel = value;
                else
                    throw new ArgumentOutOfRangeException("Audio input level");
            }
        }

        public string AudioOutputDevice { get; set; }

        private int _audioOutputLevel;
        public int AudioOutputLevel 
        { 
            get
            {
                return _audioOutputLevel;
            } 
            set
            {
                if (value >= 0 && value <= 100)
                    _audioOutputLevel = value;
                else
                    throw new ArgumentOutOfRangeException("Audio output level");
            } 
        }
    }
}
