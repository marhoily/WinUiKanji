using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Devices;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using Windows.Media.Streaming.Adaptive;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace WinUiKanji
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Random _rnd = new();
        private readonly SpeechSynthesizer _synthesizer = new()
        {
            Voice = SpeechSynthesizer.AllVoices
                .First(v => v.Language == "ja-JP")
        };

        public MainViewModel()
        {
            BackgroundMediaPlayer.Current.AutoPlay = true;
        }

        [ICommand]
        private void MoveNext()
        {
            if (SourceSet == null) return;
            var nextVal = (CurrentTermIndex + 1) % SourceSet.Length;
            if (nextVal == 0)
                _rnd.Shuffle(SourceSet);
            CurrentTermIndex = nextVal;
            Pronounce().GetAwaiter();
        }
        private async Task Pronounce()
        {
            var stream = await _synthesizer.SynthesizeTextToStreamAsync(
                CurrentTerm.Kanji);
            BackgroundMediaPlayer.Current.Source = MediaSource
                .CreateFromStream(stream, stream.ContentType);
        }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CurrentTerm))]
        private int _currentTermIndex = -1;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CurrentTerm))]
        private Card[] _sourceSet;

        public Card CurrentTerm => _sourceSet[CurrentTermIndex];
    }
}