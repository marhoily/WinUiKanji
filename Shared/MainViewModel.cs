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
        public bool NeedToReadAnswer { get; set; } = true;
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
        public async Task GoBack()
        {
            _answerIsRead = false;
            if (CurrentTermIndex == 0)
                return;
            CurrentTermIndex--;
            await ReadQuestion();
        }

        [ICommand]
        public async Task ItIsGettingRepetitive()
        {
            _answerIsRead = true;
            await ChangeWellKnownMarker(+1);
            await GoAhead();
        }

        private async Task ChangeWellKnownMarker(int increment)
        {
            CurrentCard.WellKnown += increment;
            await _studySet.SaveAsync();
        }

        [ICommand]
        public async Task GoAhead()
        {
            if (_answerIsRead || !NeedToReadAnswer)
            {
                await ReadQuestionAndStuff();
                _answerIsRead = false;
            }
            else
            {
                await ReadAnswer();
                _answerIsRead = true;
            }


            async Task ReadQuestionAndStuff()
            {
                CurrentTermIndex = (CurrentTermIndex + 1) % WorkingSet.Count;
                if (CurrentTermIndex == 0) await Reshuffle();
                await ReadQuestion();
            }

            async Task Reshuffle()
            {
                _workingSet = _studySet.GetShuffle();
                OnPropertyChanged(nameof(WorkingSetLength));
                await _player.Blimp();
            }
        }

        [ICommand]
        public async Task DoNotKnow()
        {
            await ChangeWellKnownMarker(-1);
            if (!_answerIsRead)
            {
                await GoAhead();
                await _player.WaitABit();
            }
            _workingSet.Insert(CurrentTermIndex + 2, CurrentCard);
            _workingSet.Add(CurrentCard);
            OnPropertyChanged(nameof(WorkingSetLength));
            await GoAhead();
        }
        [ICommand]
        public Task ReadQuestion() => AnswerIsMeaning ? ReadPronounciation() : ReadMeaning();
        [ICommand]
        public Task ReadAnswer() => AnswerIsMeaning ? ReadMeaning() : ReadPronounciation();


        private async Task ReadMeaning() => await _player.Say("en-US", CurrentCard.Meaning);
        private async Task ReadPronounciation() => await _player.Say("ja-JP", CurrentCard.ToPronounce);
    }
}