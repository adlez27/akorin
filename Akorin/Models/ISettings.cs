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
        public ObservableCollection<RecListItem> RecList { get; }
        public string NotesFile { get; set; }
        public string DestinationFolder { get; set; }

        public string AudioDriver { get; set; }
        public string AudioInputDevice { get; set; }
        public int AudioInputLevel { get; set; }
        public string AudioOutputDevice { get; set; }
        public int AudioOutputLevel { get; set; }

        public int FontSize { get; set; }
    }
}
