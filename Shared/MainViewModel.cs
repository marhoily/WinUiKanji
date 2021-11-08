using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Random _rnd = new();
        private readonly IPlayer _player;
        private const bool ReadAnswerEnabled = true;
        private const bool AnswerIsMeaning = true;
        private const string Words = @"C:\git\WinUiKanji\StudySets\2021-oct-7.csv";
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

        public MainViewModel(IPlayer player)
        {
            using var reader = new StreamReader(Words);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            _originalSet = csv.GetRecords<Card>().ToArray();
            _sourceSet = Reshuffle();
            _player = player;
        }

        [ICommand]
        private Task PrevCard()
        {
            _answerIsRead = false;
            return Go(-1);
        }

        [ICommand]
        private async Task WellKnownGoNext()
        {
            _answerIsRead = true;
            await ChangeWellKnownMarker(+1);
            await Go(1);
        }

        private async Task ChangeWellKnownMarker(int increment)
        {
            CurrentCard.WellKnown += increment;
            using var writer = new StreamWriter(Words);
            foreach (var b in Encoding.UTF8.GetPreamble())
                writer.BaseStream.WriteByte(b);
            using var csv = new CsvWriter(writer,
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = Encoding.UTF8,
                });
            await csv.WriteRecordsAsync(_originalSet);
        }

        [ICommand] private Task CorrectGoNext() => Go(1);

        [ICommand]
        private async Task WrongGoNext()
        {
            await ChangeWellKnownMarker(-1);
            if (!_answerIsRead)
            {
                await Go(1);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            _sourceSet.Insert(CurrentTermIndex + 2, CurrentCard);
            _sourceSet.Add(CurrentCard);
            OnPropertyChanged(nameof(SourceSetLength));
            await Go(1);
        }
        [ICommand]
        private Task ReadQuestion() => AnswerIsMeaning ? ReadPronounciation() : ReadMeaning();
        [ICommand]
        private Task ReadAnswer() => AnswerIsMeaning ? ReadMeaning() : ReadPronounciation();

        private async Task Go(int i)
        {
            if (ReadAnswerEnabled && !_answerIsRead)
            {
                await ReadAnswer();
                if (i == 1)
                    _answerIsRead = true;
                return;
            }
            var nextVal = (CurrentTermIndex + i) % SourceSet.Count;
            if (nextVal == -1) return;
            if (nextVal == 0 && i == 1)
            {
                _sourceSet = Reshuffle();
                OnPropertyChanged(nameof(SourceSetLength));
                await _player.Blimp();
            }
            CurrentTermIndex = nextVal;
            await ReadQuestion();
            if (i == 1)
                _answerIsRead = false;
        }

        private List<Card> Reshuffle() => _originalSet
                .Where(card => card.WellKnown < 2)
                .Shuffle();

        private async Task ReadMeaning() => await _player.Say("us-EN", CurrentCard.Meaning);
        private async Task ReadPronounciation() => await _player.Say("ja-JP", CurrentCard.ToPronounce);
    }
}