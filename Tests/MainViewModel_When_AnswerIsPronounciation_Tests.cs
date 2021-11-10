namespace Tests;

public sealed class MainViewModel_When_AnswerIsPronounciation_Tests : MainViewModelTestBase
{

    public MainViewModel_When_AnswerIsPronounciation_Tests()
    {
        Sut = new(_player, _studySet);
        // Ignore initial reshuffle
        _studySet.EvictWorkLog().Should().Equal("reshuffled");
        Sut.AnswerIsMeaning = false;
    }

    [Fact]
    public async Task CorrectGoNext_Should_Read_Answer()
    {
        await Sut.CorrectGoNext();
        Sut.CurrentTermIndex.Should().Be(0);
        _player.EvictPlaylist().Should().Equal("ja-JP: p1");
        await Sut.CorrectGoNext();
        Sut.CurrentTermIndex.Should().Be(1);
        _player.EvictPlaylist().Should().Equal("en-US: m2");
        await Sut.CorrectGoNext();
        Sut.CurrentTermIndex.Should().Be(1);
        _player.EvictPlaylist().Should().Equal("ja-JP: p2");
    }
}
