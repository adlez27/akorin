using Akorin.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AkorinTests
{
    public class AudioFileTests
    {
        AudioFile af;
        Settings settings; 

        public AudioFileTests()
        {
            settings = new Settings();
            settings.DestinationFolder = @"C:\Users\Mark\Desktop\test";
            af = new AudioFile(settings, "b");
        }

        //[Fact]
        //public async void WriteAudioTest()
        //{
        //    Random rnd = new Random();
        //    Byte[] b = new Byte[40000];
        //    rnd.NextBytes(b);
        //    af.Write(b);
        //}

        //[Fact]
        //public async void ReadAndWriteAudioTest()
        //{
        //    af.Play();
        //    await Task.Delay(4000);
        //    af.Stop();
        //    af.Write();
        //}

        //[Fact]
        //public async void OverwriteTest()
        //{
        //    af.Play();
        //    await Task.Delay(4000);
        //    af.Stop();
        //    af.Record();
        //    await Task.Delay(3000);
        //    af.Stop();
        //    af.Write();
        //}

        [Fact]
        public async void RecordAndSaveAudioTest()
        {
            af.Record();
            await Task.Delay(3000);
            af.Stop();
            af.Write();
        }

        //[Fact]
        //public async void PlayAudioTest()
        //{
        //    af.Play();
        //    await Task.Delay(4000);
        //}

        //[Fact]
        //public async void OutputVolumeTest()
        //{
        //    settings.AudioOutputLevel = 100;
        //    af.Play();
        //    await Task.Delay(4000);
        //    settings.AudioOutputLevel = 20;
        //    af.Play();
        //    await Task.Delay(4000);
        //}

        //[Fact]
        //public async void PlayAndStopAudioTest()
        //{
        //    af.Play();
        //    await Task.Delay(1000);
        //    af.Stop();
        //    await Task.Delay(3000);
        //}

        //[Fact]
        //public async void PlayRepeatedlyTest()
        //{
        //    af.Play();
        //    await Task.Delay(1000);
        //    af.Play();
        //    await Task.Delay(4000);
        //}
    }
}
