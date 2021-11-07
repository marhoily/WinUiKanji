using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using CsvHelper;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace WinUiKanji
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Random _rnd = new();
        private readonly SpeechSynthesizer _synthesizer = new()
        {
            Voice = SpeechSynthesizer.AllVoices.First(v => v.Language == "ja-JP")
        };

        public MainViewModel()
        {
            BackgroundMediaPlayer.Current.AutoPlay = true;
            using var reader = new StreamReader(@"C:\git\Kanji\WordsToStudy4.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            _sourceSet = csv.GetRecords<Card>().ToArray();
            _rnd.Shuffle(SourceSet);
        }

        [ICommand]
        private void NextCard()
        {
            var nextVal = (CurrentTermIndex + 1) % SourceSet.Length;
            if (nextVal == 0)
                _rnd.Shuffle(SourceSet);
            CurrentTermIndex = nextVal;
            Pronounce().GetAwaiter();
        }
        private async Task Pronounce()
        {
            var stream = await _synthesizer.SynthesizeTextToStreamAsync(CurrentCard.ToPronounce);
            BackgroundMediaPlayer.Current.Source =
                MediaSource.CreateFromStream(stream, stream.ContentType);
        }

        [ObservableProperty, AlsoNotifyChangeFor(nameof(CurrentCard))]
        private int _currentTermIndex;

        [ObservableProperty, AlsoNotifyChangeFor(nameof(CurrentCard))]
        private Card[] _sourceSet;

        public Card CurrentCard => _sourceSet[CurrentTermIndex];
    }
}