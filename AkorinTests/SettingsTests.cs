using System;
using Xunit;
using Akorin.Models;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AkorinTests
{
    public class SettingsTests
    {
        Settings settings = new Settings(true);
        string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        [Fact]
        public void ReadNotes()
        {
            Assert.Equal("a", settings.RecList[0].Note);
        }

        [Fact]
        public void DefaultFolder()
        {
            Assert.Equal(Path.Combine(currentDirectory, "voicebank"), settings.DestinationFolder);
        }

        [Fact]
        public void SetAudioInputLevelTest()
        {
            settings.AudioInputLevel = 50;
            var expected = 50;
            Assert.Equal(expected, settings.AudioInputLevel);
        }

        [Fact]
        public void SetAudioOutputLevelTest()
        {
            settings.AudioOutputLevel = 50;
            var expected = 50;
            Assert.Equal(expected, settings.AudioOutputLevel);
        }

        [Fact]
        public void LoadSettings()
        {
            settings.LoadSettings(Path.Combine(currentDirectory, "test.arp"));
            Assert.Equal("foo", settings.RecList[0].Text);
            Assert.Equal("bar", settings.RecList[0].Note);
            Assert.NotNull(settings.RecList[0].Audio);
        }

        [Fact]
        public async void SwitchOutput()
        {
            settings.RecList[0].Audio.Play();
            await Task.Delay(1000);
            settings.AudioOutputDevice = 3;
            settings.RecList[0].Audio.Play();
            await Task.Delay(1000);
        }
    }
}
