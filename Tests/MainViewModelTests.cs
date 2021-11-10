using FluentAssertions;
using Shared;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public sealed class MainViewModelTests : MainViewModelTestBase
    {
        private MainViewModel _vm;

        public MainViewModelTests()
        {
            _vm = new(_player, new FakeStudySet(C1, C2));
        }

        [Fact]
        public void CurrentIndexStr()
        {
            _vm.CurrentIndexStr.Should().Be("1");
        }
        [Fact]
        public void CurrentCard()
        {
            _vm.CurrentCard.Should().Be(C1);
        }
        [Fact]
        public void WorkingSetLength()
        {
            _vm.WorkingSetLength.Should().Be("2");
        }
        [Fact]
        public async Task PrevCard_When_At0_Should_Do_NothingAsync()
        {
            await _vm.PrevCard();
            _vm.CurrentTermIndex.Should().Be(0);
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(0);
        }
        [Fact]
        public async Task PrevCard_When_At0R_Should_Reset_AnswerIsRead()
        {
            await _vm.CorrectGoNext();
            await _vm.PrevCard();
            _vm.CurrentTermIndex.Should().Be(0);
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(0);
        }
        [Fact]
        public async Task CorrectGoNext_Should_Read_Answer()
        {
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(0);
            _player.EvictPlaylist().Should().Equal("en-US: m1");
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(1);
            _player.EvictPlaylist().Should().Equal("ja-JP: p2");
            await _vm.CorrectGoNext();
            _vm.CurrentTermIndex.Should().Be(1);
            _player.EvictPlaylist().Should().Equal("en-US: m2");
        }

    }
}