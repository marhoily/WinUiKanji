using Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;

namespace WinUiKanji
{
    internal class PlayerImpl : IPlayer
    {
        private readonly SpeechSynthesizer _synthesizer = new();

        public PlayerImpl()
        {
            BackgroundMediaPlayer.Current.AutoPlay = true;
        }
        public async Task Blimp()
        {
            BackgroundMediaPlayer.Current.SetUriSource(
                new Uri("ms-winsoundevent:Notification.SMS"));
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        public async Task Say(string language, string text)
        {
            _synthesizer.Voice = SpeechSynthesizer.AllVoices.First(v => v.Language == language);
            var stream = await _synthesizer.SynthesizeTextToStreamAsync(text);
            BackgroundMediaPlayer.Current.Source = 
                MediaSource.CreateFromStream(stream, stream.ContentType);            
        }

        public Task WaitABit() => Task.Delay(TimeSpan.FromSeconds(1));
    }
}