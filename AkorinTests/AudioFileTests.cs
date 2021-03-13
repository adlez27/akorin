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
            af = new AudioFile(settings, "a");
        }

        //[Fact]
        //public void ReadAudioTest()
        //{
        //    Assert.True(af.Read());
        //}

        //[Fact]
        //public async void WriteAudioTest()
        //{
        //    Random rnd = new Random();
        //    Byte[] b = new Byte[40000];
        //    rnd.NextBytes(b);
        //    af.Write(b);
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
        //    af.Read();
        //    af.Play();
        //    await Task.Delay(4000);
        //}

        //[Fact]
        //public async void OutputVolumeTest()
        //{
        //    af.Read();
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
        //    af.Read();
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
