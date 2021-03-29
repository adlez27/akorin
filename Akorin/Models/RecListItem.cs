using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Akorin.Models
{
    public class RecListItem
    {
        [YamlIgnore]
        public AudioFile Audio { get; }

        public string Text { get; }

        public string Note { get; set; }
        
        public RecListItem (ISettings s, string t)
        {
            Text = t;
            Audio = new AudioFile(s, t);
            Note = "test";
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
