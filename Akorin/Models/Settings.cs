using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akorin.Models
{
    class Settings
    {
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
