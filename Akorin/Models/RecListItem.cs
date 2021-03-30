﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Akorin.Models
{
    public class RecListItem: ReactiveObject
    {
        public AudioFile Audio { get; }
        public string Text { get; }
        
        public RecListItem (ISettings s, string t)
        {
            Text = t;
            Audio = new AudioFile(s, t);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
