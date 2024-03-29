﻿using ManagedBass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ScottPlot.Plottable;

namespace Akorin.Models
{
    public class AudioFile
    {
        private ISettings settings;
        private int stream;
        private bool recorded;

        private List<byte> data;
        public short[] Data
        {
            get
            {
                short[] sdata = new short[data.Count / 2];
                System.Buffer.BlockCopy(data.ToArray(), 0, sdata, 0, data.Count);
                return sdata;
            }
        }

        public AudioFile(ISettings s, string fileName)
        {
            settings = s;
            this.fileName = fileName;
            stream = 0;
            data = new List<byte>();
            recorded = false;
            Read();
        }

        private string fileName;
        private string FullName
        {
            get { return Path.Combine(settings.DestinationFolder, fileName + ".wav"); }
        }

        public void Read ()
        {
            if (File.Exists(FullName) && !recorded)
            {
                byte[] rawBytes = File.ReadAllBytes(FullName);
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
                    wfw.Write(Data, Data.Length * 2);
                    reRead = ms.GetBuffer();
                }
                stream = Bass.CreateStream(reRead, 0, reRead.Length, BassFlags.Mono);
            }
        }

        public void Unload()
        {
            data.Clear();
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

            //Modify recording according to Input Volume
            short[] sdata = new short[temp.Length/2];
            System.Buffer.BlockCopy(temp, 0, sdata, 0, temp.Length);
            for (var i = 0; i < sdata.Length; i++)
            {
                sdata[i] = (short)(sdata[i] * ((double)settings.AudioInputLevel / 100.0));
            }
            System.Buffer.BlockCopy(sdata, 0, temp, 0, temp.Length);

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

                Write(temp, FullName);
            }
        }
    }
}
