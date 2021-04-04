using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akorin.Models
{
    public class WCTReclist
    {
        public Phonemes Phonemes { get; set; }
        public List<Line> Files { get; set; }
    }
    public class Phonemes
    {
        public List<string> Vowels { get; set; }
        public List<string> Consonants { get; set; }
    }
    public class Line
    {
        public string Filename { get; set; }
        public List<string> Phonemes { get; set; }
        public string Description { get; set; }
    }
}
