using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akorin.Models
{
    public interface ISettings
    {
        public string RecListFile { get; set; }
        public bool ReadUnicode { get; set; }
        public bool SplitWhitespace { get; set; }
        public ObservableCollection<RecListItem> RecList { get; set; }
        public string DestinationFolder { get; set; }

        public string AudioDriver { get; set; }
        public List<string> AudioInputDeviceList { get; }
        public int AudioInputDevice { get; set; }
        public int AudioInputLevel { get; set; }
        public List<string> AudioOutputDeviceList { get; }
        public int AudioOutputDevice { get; set; }
        public int AudioOutputLevel { get; set; }

        public int FontSize { get; set; }

        public void LoadSettings(string path);
        public void SaveSettings(string path);
    }
}
