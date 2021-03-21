using System;
using Xunit;
using Akorin.Models;

namespace AkorinTests
{
    public class SettingsTests
    {
        Settings settings = new Settings();

        [Fact]
        public void DefaultRecList()
        {
            Assert.Equal(@"reclists\default_reclist.txt", settings.RecListFile);
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
            Assert.Equal(@"voicebank\default_notes.json", settings.NotesFile);
        }

        [Fact]
        public void LoadNotes()
        {
            Assert.Equal("a", settings.Notes["‚ "]);
        }

        [Fact]
        public void DefaultFolder()
        {
            Assert.Equal("voicebank", settings.DestinationFolder);
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
