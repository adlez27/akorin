using ManagedBass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Akorin.Models
{
    public class AudioFile
    {
        private Settings settings;
        public string FullName { get; }
        private List<byte> data;
        private int stream;

        public AudioFile(Settings s, string fileName)
        {
            settings = s;
            FullName = Path.Combine(s.DestinationFolder, fileName + ".wav");
            stream = 0;
            data = new List<byte>();
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
            Stop();
            if (stream != 0)
            {
                Bass.ChannelSetAttribute(stream, ChannelAttribute.Volume, (double)settings.AudioOutputLevel / 100.0);
                Bass.ChannelPlay(stream, true);
            }
        }

        public void Stop()
        {
            Bass.ChannelStop(stream);
        }

        public void Record()
        {
            stream = Bass.RecordStart(44100, 1, BassFlags.Default, RecordProcedure);
        }

        public bool RecordProcedure(int Handle, IntPtr Buffer, int Length, IntPtr User)
        {
            byte[] temp = new byte[Length];
            Marshal.Copy(Buffer, temp, 0, Length);
            data.AddRange(temp);
            return true;
        }

        public void Write (byte[] data)
        {
            Directory.CreateDirectory(settings.DestinationFolder);
            var format = new WaveFormat(44100,16,1);

            using (FileStream fs = File.Create(FullName))
            using (WaveFileWriter wfw = new WaveFileWriter(fs, format))
            {
                wfw.Write(data,data.Length);
            }
        }

        public void Write()
        {
            byte[] dataArr = data.ToArray();
            Write(dataArr);
        }
    }
}
