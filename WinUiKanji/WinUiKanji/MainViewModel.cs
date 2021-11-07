using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using CommunityToolkit.WinUI.UI.Controls.TextToolbarSymbols;
using CsvHelper;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace WinUiKanji
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Random _rnd = new();
        private readonly SpeechSynthesizer _synthesizer = new();

        private const bool ReadAnswerEnabled = true;
        private bool _answerIsRead = false;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CurrentCard))]
        [AlsoNotifyChangeFor(nameof(CurrentIndexStr))]
        private int _currentTermIndex;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CurrentCard))]
        [AlsoNotifyChangeFor(nameof(SourceSetLength))]
        private List<Card> _sourceSet;
        private Card[] _originalSet;

        public Card CurrentCard => _sourceSet[CurrentTermIndex];
        public string CurrentIndexStr => (CurrentTermIndex + 1).ToString();
        public string SourceSetLength => SourceSet.Count.ToString();

        public MainViewModel()
        {
            BackgroundMediaPlayer.Current.AutoPlay = true;
            using var reader = new StreamReader(@"C:\git\Kanji\nov-21.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            _originalSet = csv.GetRecords<Card>().ToArray();
            _sourceSet = _originalSet.ToList();
            _rnd.Shuffle(SourceSet);
        }

        [ICommand] private Task PrevCard() => Go(-1);
        [ICommand]
        private Task CorrectGoNext()
        {
            return Go(1);
        }

        [ICommand]
        private Task WrongTaskGoNext()
        {
            _sourceSet.Insert(CurrentTermIndex + 2, CurrentCard);
            OnPropertyChanged(nameof(SourceSetLength));
            _answerIsRead = true;
            return Go(1);
        }

        private async Task Go(int i)
        {
            if (ReadAnswerEnabled && !_answerIsRead)
            {
                _synthesizer.Voice = SpeechSynthesizer.AllVoices.First(v => v.Language == "ja-JP");
                var answer = await _synthesizer.SynthesizeTextToStreamAsync(CurrentCard.ToPronounce);
                BackgroundMediaPlayer.Current.Source =
                    MediaSource.CreateFromStream(answer, answer.ContentType);
                _answerIsRead = true;
                return;
            }
            _synthesizer.Voice = SpeechSynthesizer.DefaultVoice;
            var nextVal = (CurrentTermIndex + i) % SourceSet.Count;
            if (nextVal == -1) return;
            if (nextVal == 0 && i == 1)
            {
                _originalSet = SourceSet.ToArray();
                _rnd.Shuffle(SourceSet);
                OnPropertyChanged(nameof(SourceSetLength));
                BackgroundMediaPlayer.Current.SetUriSource(
                    new Uri("ms-winsoundevent:Notification.SMS"));
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            CurrentTermIndex = nextVal;
            var stream = await _synthesizer.SynthesizeTextToStreamAsync(CurrentCard.Meaning);
            BackgroundMediaPlayer.Current.Source =
                MediaSource.CreateFromStream(stream, stream.ContentType);
            _answerIsRead = false;
        }
    }
}