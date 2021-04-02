using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using ReactiveUI;
using System.ComponentModel.DataAnnotations;

namespace Akorin.Models
{
    //[MetadataType(typeof(RecListItemMetadata))]
    public class RecListItem : ReactiveObject
    {
        [YamlIgnore]
        public AudioFile Audio { get; set; }

        public string Text { get; set; }

        public string Note { get; set; }
        
        public RecListItem (ISettings s, string t)
        {
            Text = t;
            Audio = new AudioFile(s, t);
            Note = "";
        }

        public RecListItem (ISettings s, string t, string n)
        {
            Text = t;
            Audio = new AudioFile(s, t);
            Note = n;
        }

        public RecListItem() { }

        public void CreateAudio(ISettings s)
        {
            Audio = new AudioFile(s, Text);
        }

        public override string ToString()
        {
            return Text;
        }

        [YamlIgnore]
        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing { get { return base.Changing; } }
        [YamlIgnore]
        public IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed { get { return base.Changed; } }
        [YamlIgnore]
        public IObservable<Exception> ThrownExceptions { get { return base.ThrownExceptions; } }
    }
}
