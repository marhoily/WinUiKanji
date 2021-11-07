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

        [ICommand] private Task PrevCard() => Go(-1);
        [ICommand] private Task NextCard() => Go(1);

        private async Task Go(int i)
        {
            var nextVal = (CurrentTermIndex + i) % SourceSet.Length;
            if (nextVal == -1) return;
            if (nextVal == 0 && i == 1)
            {
                _rnd.Shuffle(SourceSet);
                BackgroundMediaPlayer.Current.SetUriSource(
                    new Uri("ms-winsoundevent:Notification.SMS"));
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            CurrentTermIndex = nextVal;
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