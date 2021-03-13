using ManagedBass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akorin.Models
{
    public class AudioFile
    {
        private Settings settings;
        public string FullName { get; }
        private int stream;

        public AudioFile(Settings s, string fileName)
        {
            settings = s;
            FullName = Path.Combine(s.DestinationFolder, fileName + ".wav");
            stream = 0;
        }

        public bool Read ()
        {
            if (File.Exists(FullName))
            {
                stream = Bass.CreateStream(FullName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Play()
        {
            StopPlayback();
            if (stream != 0)
            {
                Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, (double)settings.AudioOutputLevel / 100.0);
                Bass.ChannelPlay(stream, true);
            }
        }

        public void StopPlayback()
        {
            Bass.ChannelStop(stream);
        }

        public void Record() { }
        public void StopRecording() { }

        public void Write (string data)
        {
            Directory.CreateDirectory(settings.DestinationFolder);
            using (StreamWriter sw = File.CreateText(FullName))
            {
                sw.WriteLine(data);
            }
        }
    }
}
