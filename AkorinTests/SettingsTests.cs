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
            Assert.Equal(@"C:\Users\Mark\source\repos\akorin\Akorin\reclists\default_reclist.txt", settings.RecListFile);
        }

        [Fact]
        public void DefaultFolder()
        {
            Assert.Equal("voicebank", settings.DestinationFolder);
        }

        [Fact]
        public void SplitByWhitespace()
        {
            Assert.Equal("a", settings.RecList[0].Text);
        }

        [Fact]
        public void SplitByNewline()
        {
            settings.SplitWhitespace = false;
            Assert.Equal("a i u e o", settings.RecList[0].Text);
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
