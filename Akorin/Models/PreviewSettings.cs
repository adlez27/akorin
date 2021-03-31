using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akorin.Models
{
    class PreviewSettings : ISettings
    {
        public string RecListFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ReadUnicode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool SplitWhitespace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ObservableCollection<RecListItem> RecList => throw new NotImplementedException();
        public string NotesFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DestinationFolder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AudioDriver { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int AudioInputDevice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int AudioInputLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int AudioOutputDevice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int AudioOutputLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int FontSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        List<string> ISettings.AudioInputDeviceList => throw new NotImplementedException();

        List<string> ISettings.AudioOutputDeviceList => throw new NotImplementedException();

        ObservableCollection<RecListItem> ISettings.RecList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string ISettings.ProjectFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void ISettings.LoadDefault()
        {
            throw new NotImplementedException();
        }

        void ISettings.LoadSettings(string path)
        {
            throw new NotImplementedException();
        }

        void ISettings.SaveSettings(string path)
        {
            throw new NotImplementedException();
        }
    }
}
