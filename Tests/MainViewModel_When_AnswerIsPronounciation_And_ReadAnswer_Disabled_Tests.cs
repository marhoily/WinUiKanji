namespace Tests;

public sealed class MainViewModel_When_AnswerIsPronounciation_And_ReadAnswer_Disabled_Tests : MainViewModelTestBase
{
  
    public MainViewModel_When_AnswerIsPronounciation_And_ReadAnswer_Disabled_Tests()
    {
        _studySet.Cards.Add(C3);
        Sut = new(_player, _studySet);
        // Ignore initial reshuffle
        _studySet.EvictWorkLog().Should().Equal("reshuffled");
        Sut.AnswerIsMeaning = false;
        Sut.NeedToReadAnswer = false;
    }

    [Fact]
    public async Task CorrectGoNext_Should_Read_Answer()
    {
        await Sut.GoAhead();
        Sut.CurrentTermIndex.Should().Be(1);
        _player.EvictPlaylist().Should().Equal("en-US: m2");
        await Sut.GoAhead();
        Sut.CurrentTermIndex.Should().Be(2);
        _player.EvictPlaylist().Should().Equal("en-US: m3");
    }
}
