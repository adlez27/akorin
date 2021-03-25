using ManagedBass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public short[] Data
        {
            get
            {
                short[] sdata = new short[data.Count / 2];
                System.Buffer.BlockCopy(data.ToArray(), 0, sdata, 0, data.Count);
                for (var i = 0; i < sdata.Length; i++)
                {
                    sdata[i] = (short)(sdata[i] * ((double)settings.AudioInputLevel / 100.0));
                }
                return sdata;
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
            if (File.Exists(fullName) && !recorded)
            {
                byte[] rawBytes = File.ReadAllBytes(fullName);
                data = new ArraySegment<byte>(rawBytes, 46, rawBytes.Length - 46).ToList();
                stream = Bass.CreateStream(rawBytes, 0, rawBytes.Length, BassFlags.Mono);
            }
            else if (recorded)
            {
                byte[] reRead;
                var format = new WaveFormat(44100, 16, 1);
                using (MemoryStream ms = new MemoryStream())
                using (WaveFileWriter wfw = new WaveFileWriter(ms, format))
                {
                    wfw.Write(Data, Data.Length);
                    reRead = ms.GetBuffer();
                }
                stream = Bass.CreateStream(reRead, 0, reRead.Length, BassFlags.Mono);
            }
        }

        public void Play()
        {
            Stop();
            Read();
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
            data = new List<byte>();
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

        public void Write (byte[] data, string destination)
        {
            var format = new WaveFormat(44100,16,1);
            using (FileStream fs = File.Create(destination))
            using (WaveFileWriter wfw = new WaveFileWriter(fs, format))
            {
                wfw.Write(data,data.Length);
            }
        }

        public void Write()
        {
            Directory.CreateDirectory(settings.DestinationFolder);
            if (recorded)
            {
                byte[] temp = new byte[Data.Length * 2];
                System.Buffer.BlockCopy(Data, 0, temp, 0, Data.Length * 2);

                Write(temp, fullName);
            }
        }
    }
}
