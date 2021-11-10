namespace Tests;

public sealed class MainViewModel_When_ReadAnswer_Disabled_Tests : MainViewModelTestBase
{
    public MainViewModel_When_ReadAnswer_Disabled_Tests()
    {
        _studySet.Cards.Add(C3);
        Sut = new(_player, _studySet);
        // Ignore initial reshuffle
        _studySet.EvictWorkLog().Should().Equal("reshuffled");
        Sut.ReadAnswerEnabled = false;
    }

    [Fact]
    public async Task CorrectGoNext_Should_Read_Answer()
    {
        await Sut.CorrectGoNext();
        Sut.CurrentTermIndex.Should().Be(1);
        _player.EvictPlaylist().Should().Equal("ja-JP: p2");
        await Sut.CorrectGoNext();
        Sut.CurrentTermIndex.Should().Be(2);
        _player.EvictPlaylist().Should().Equal("ja-JP: p3");
    }
}
