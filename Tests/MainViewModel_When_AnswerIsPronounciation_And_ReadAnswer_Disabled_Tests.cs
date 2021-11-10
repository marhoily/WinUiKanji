using FluentAssertions;
using Shared;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public sealed class MainViewModel_When_AnswerIsPronounciation_And_ReadAnswer_Disabled_Tests : MainViewModelTestBase
    {
        private MainViewModel _vm;

        public MainViewModel_When_AnswerIsPronounciation_And_ReadAnswer_Disabled_Tests()
        {
            _vm = new(_player, new FakeStudySet(C1, C2, C3));
            _vm.AnswerIsMeaning = false;
            _vm.ReadAnswerEnabled = false;
        }
        
        [Fact]
        public async Task CorrectGoNext_Should_Read_Answer()
        {
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(1);
            _player.EvictPlaylist().Should().Equal("en-US: m2");
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(2);
            _player.EvictPlaylist().Should().Equal("en-US: m3");
        }
    }
}