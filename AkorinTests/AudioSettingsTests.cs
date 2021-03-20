using System;
using Xunit;
using Akorin.Models;

namespace AkorinTests
{
    public class AudioSettingsTests
    {
        Settings settings = new Settings();

        //[Fact]
        public void SetAudioInputLevelTest()
        {
            settings.AudioInputLevel = 50;
            var expected = 50;
            Assert.Equal(expected, settings.AudioInputLevel);
        }

        //[Fact]
        public void SetAudioOutputLevelTest()
        {
            settings.AudioOutputLevel = 50;
            var expected = 50;
            Assert.Equal(expected, settings.AudioOutputLevel);
        }
    }
}
