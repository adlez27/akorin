using Akorin.Models;
using System.Threading.Tasks;
using Xunit;

namespace AkorinTests
{
    public class AudioFileTests
    {
        AudioFile af = new AudioFile(new Settings { DestinationFolder = @"C:\Users\Mark\Desktop\test" }, "scale");

        [Fact]
        public void ReadAudioTest()
        {
            Assert.True(af.Read());
        }

        //[Fact]
        //public async void PlayAudioTest()
        //{
        //    af.Play();

        //    await Task.Delay(4000);
        //}

        [Fact]
        public async void PlayAndStopAudioTest()
        {
            af.Read();
            af.Play();
            await Task.Delay(1000);
            af.StopPlayback();
            await Task.Delay(3000);
        }

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
