using System;
using Xunit;
using Akorin.Models;
using System.IO;
using System.Reflection;

namespace AkorinTests
{
    public class SettingsTests
    {
        Settings settings = new Settings();
        string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        [Fact]
        public void DefaultRecList()
        {
            Assert.Equal(Path.Combine(currentDirectory, "reclists", "default_reclist.arl"), settings.RecListFile);
        }

        [Fact]
        public void SplitByWhitespace()
        {
            Assert.Equal("‚ ", settings.RecList[0].Text);
        }

        [Fact]
        public void SplitByNewline()
        {
            settings.SplitWhitespace = false;
            settings.RecListFile = Path.Combine(currentDirectory, "reclists", "default_reclist.txt");
            Assert.Equal("‚  ‚¢ ‚¤ ‚¦ ‚¨", settings.RecList[0].Text);
        }

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
            settings.LoadSettings(@"C:\Users\Mark\Desktop\project.arp");
            Assert.Equal("foo", settings.RecList[0].Text);
            Assert.Equal("bar", settings.RecList[0].Note);
            Assert.NotNull(settings.RecList[0].Audio);
        }
    }
}
