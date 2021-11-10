using FluentAssertions;
using Shared;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public sealed class MainViewModel_When_AnswerIsPronounciation_Tests : MainViewModelTestBase
    {
        private MainViewModel _vm;

        public MainViewModel_When_AnswerIsPronounciation_Tests()
        {
            _vm = new(_player, new FakeStudySet(C1, C2));
            _vm.AnswerIsMeaning = false;
        }
        
        [Fact]
        public async Task CorrectGoNext_Should_Read_Answer()
        {
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(0);
            _player.EvictPlaylist().Should().Equal("ja-JP: p1");
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(1);
            _player.EvictPlaylist().Should().Equal("en-US: m2");
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(1);
            _player.EvictPlaylist().Should().Equal("ja-JP: p2");
        }
    }
}