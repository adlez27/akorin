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
            Assert.Equal(Path.Combine(currentDirectory, "reclists", "default_reclist.txt"), settings.RecListFile);
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
            Assert.Equal("‚  ‚¢ ‚¤ ‚¦ ‚¨", settings.RecList[0].Text);
        }

        [Fact]
        public void DefaultNotes()
        {
            Assert.Equal(Path.Combine(currentDirectory, "voicebank", "default_notes.yaml"), settings.NotesFile);
        }

        [Fact]
        public void LoadNotes()
        {
            Assert.Equal("a", settings.Notes["‚ "]);
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
    }
}
