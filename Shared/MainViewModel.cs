using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPlayer _player;
        private readonly IStudySet _studySet;
        public bool ReadAnswerEnabled { get; set; } = true;
        public bool AnswerIsMeaning { get; set; } = true;
        private bool _answerIsRead = false;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CurrentCard))]
        [AlsoNotifyChangeFor(nameof(CurrentIndexStr))]
        private int _currentTermIndex;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(CurrentCard))]
        [AlsoNotifyChangeFor(nameof(WorkingSetLength))]
        private List<Card> _workingSet;

        public Card CurrentCard => _workingSet[CurrentTermIndex];
        public string CurrentIndexStr => (CurrentTermIndex + 1).ToString();
        public string WorkingSetLength => WorkingSet.Count.ToString();

        public MainViewModel(IPlayer player, IStudySet studySet)
        {
            studySet.Load(@"C:\git\WinUiKanji\StudySets\2021-oct-7.csv");
            _workingSet = studySet.GetShuffle();
            _player = player;
            _studySet = studySet;
        }

        [ICommand]
        public Task GoBack()
        {
            _answerIsRead = false;
            return Go(-1);
        }

        [ICommand]
        public async Task ItIsGettingRepetitive()
        {
            _answerIsRead = true;
            await ChangeWellKnownMarker(+1);
            await Go(1);
        }

        private async Task ChangeWellKnownMarker(int increment)
        {
            CurrentCard.WellKnown += increment;
            await _studySet.SaveAsync();
        }

        [ICommand] public Task GoAhead() => Go(1);

        [ICommand]
        public async Task DoNotKnow()
        {
            await ChangeWellKnownMarker(-1);
            if (!_answerIsRead)
            {
                await Go(1);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            _workingSet.Insert(CurrentTermIndex + 2, CurrentCard);
            _workingSet.Add(CurrentCard);
            OnPropertyChanged(nameof(WorkingSetLength));
            await Go(1);
        }
        [ICommand]
        public Task ReadQuestion() => AnswerIsMeaning ? ReadPronounciation() : ReadMeaning();
        [ICommand]
        public Task ReadAnswer() => AnswerIsMeaning ? ReadMeaning() : ReadPronounciation();

        private async Task Go(int i)
        {
            if (ReadAnswerEnabled && !_answerIsRead && i >= 0)
            {
                await ReadAnswer();
                if (i == 1)
                    _answerIsRead = true;
                return;
            }
            var nextVal = (CurrentTermIndex + i) % WorkingSet.Count;
            if (nextVal == -1) 
                return;
            if (nextVal == 0 && i == 1)
            {
                _workingSet = _studySet.GetShuffle();
                OnPropertyChanged(nameof(WorkingSetLength));
                await _player.Blimp();
            }
            CurrentTermIndex = nextVal;
            await ReadQuestion();
            if (i == 1)
                _answerIsRead = false;
        }

        private async Task ReadMeaning() => await _player.Say("en-US", CurrentCard.Meaning);
        private async Task ReadPronounciation() => await _player.Say("ja-JP", CurrentCard.ToPronounce);
    }
}