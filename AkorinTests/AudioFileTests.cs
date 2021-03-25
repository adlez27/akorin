using Akorin.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AkorinTests
{
    public class AudioFileTests
    {
        AudioFile af;
        ISettings settings;
        string fileName = "b";

        public AudioFileTests()
        {
            settings = new MockSettings();
            settings.DestinationFolder = @"C:\Users\Mark\Desktop\test";
            af = new AudioFile(settings, fileName);
        }

        [Fact]
        public void Read()
        {
            byte[] b = new byte[40000];
            Array.Fill(b, (byte)127);
            af.Write(b,Path.Combine(settings.DestinationFolder, fileName + ".wav"));
            af.Read();
            Assert.NotEmpty(af.Data);
            Assert.Equal((short)32639, af.Data[0]);
        }

        [Fact]
        public async void RecordPlay()
        {
            af.Record();
            await Task.Delay(4000);
            af.Play();
            await Task.Delay(4000);
            af.Stop();
        }

        [Fact]
        public void WriteRandom()
        {
            Random rnd = new Random();
            byte[] b = new byte[40000];
            rnd.NextBytes(b);
            af.Write(b,Path.Combine(settings.DestinationFolder, fileName + ".wav"));
        }

        [Fact]
        public async void PlayWrite()
        {
            af.Play();
            await Task.Delay(4000);
            af.Stop();
            af.Write();
        }

        [Fact]
        public async void PlayRecord()
        {
            af.Play();
            await Task.Delay(4000);
            af.Stop();
            af.Record();
            await Task.Delay(3000);
            af.Stop();
            af.Write();
        }

        [Fact]
        public async void RecordWrite()
        {
            af.Record();
            await Task.Delay(3000);
            af.Stop();
            af.Write();
        }

        [Fact]
        public async void Play()
        {
            af.Play();
            await Task.Delay(4000);
        }

        [Fact]
        public async void OutputVolume()
        {
            settings.AudioOutputLevel = 100;
            af.Play();
            await Task.Delay(4000);
            settings.AudioOutputLevel = 20;
            af.Play();
            await Task.Delay(4000);
        }

        [Fact]
        public async void InputVolume()
        {
            settings.AudioInputLevel = 20;
            af.Record();
            await Task.Delay(4000);
            af.Stop();
            af.Write();
        }

        [Fact]
        public async void PlayStop()
        {
            af.Play();
            await Task.Delay(1000);
            af.Stop();
            await Task.Delay(3000);
        }

        [Fact]
        public async void PlayPlay()
        {
            af.Play();
            await Task.Delay(1000);
            af.Play();
            await Task.Delay(4000);
        }
    }
}
