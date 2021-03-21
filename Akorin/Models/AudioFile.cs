using ManagedBass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Akorin.Models
{
    public class AudioFile
    {
        private ISettings settings;
        private string fullName;
        private int stream;
        private bool recorded;

        private List<byte> data;
        public byte[] Data
        {
            get
            {
                return data.ToArray();
            }
        }

        public AudioFile(ISettings s, string fileName)
        {
            settings = s;
            fullName = Path.Combine(s.DestinationFolder, fileName + ".wav");
            stream = 0;
            data = new List<byte>();
            recorded = false;
            Read();
        }

        public void Read ()
        {
            if (File.Exists(fullName))
            {
                stream = Bass.CreateStream(fullName);

                byte[] temp = new byte[40000];
                Bass.ChannelGetData(stream, temp, temp.Length);
                data.AddRange(temp);
            }
        }

        public void Play()
        {
            Stop();
            if (stream == 0)
            {
                Read();
            }
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
            Bass.Free();
            stream = Bass.RecordStart(44100, 1, BassFlags.Default, RecordProcedure);
            recorded = true;
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
            Bass.Free();
            Directory.CreateDirectory(settings.DestinationFolder);
            var format = new WaveFormat(44100,16,1);

            using (FileStream fs = File.Create(fullName))
            using (WaveFileWriter wfw = new WaveFileWriter(fs, format))
            {
                wfw.Write(data,data.Length);
            }
        }

        public void Write()
        {
            if (recorded)
            {
                Write(Data);
            }
           
        }
    }
}
