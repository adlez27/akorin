using System;
using Xunit;
using Akorin.Models;

namespace AkorinTests
{
    public class AudioSettingsTests
    {
        Settings settings = new Settings();

        [Fact]
        public void SetAudioInputLevelTest()
        {
            settings.AudioInputLevel = 100;
        }

        [Fact]
        public void SetAudioOutputLevelTest()
        {
            settings.AudioOutputLevel = 100;
        }
    }
}
