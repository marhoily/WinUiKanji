namespace Tests;

public sealed class MainViewModelTests : MainViewModelTestBase, IDisposable
{
    public MainViewModelTests()
    {
        Sut = new(_player, _studySet);
        // Ignore initial reshuffle
        _studySet.EvictWorkLog().Should().Equal("reshuffled");
    }
    [Fact]
    public void CurrentIndexStr()
    {
        Sut.CurrentIndexStr.Should().Be("1");
    }
    [Fact]
    public void CurrentCard()
    {
        Sut.CurrentCard.Should().Be(C1);
    }
    [Fact]
    public void WorkingSetLength()
    {
        Sut.WorkingSetLength.Should().Be("2");
    }
    [Fact]
    public async Task GoBack_When_At0_Should_Do_NothingAsync()
    {
        await Sut.GoBack();
        Sut.CurrentTermIndex.Should().Be(0);
        await Sut.GoAhead();
        Sut.CurrentTermIndex.Should().Be(0);
        _player.EvictPlaylist();
    }
    [Fact]
    public async Task GoBack_When_At0R_Should_Reset_AnswerIsRead()
    {
        await Sut.GoAhead();
        await Sut.GoBack();
        Sut.CurrentTermIndex.Should().Be(0);
        await Sut.GoAhead();
        Sut.CurrentTermIndex.Should().Be(0);
        _player.EvictPlaylist();
    }
    [Fact]
    public async Task CorrectGoNext_Should_Read_Answer()
    {
        await Sut.GoAhead();
        Sut.CurrentTermIndex.Should().Be(0);
        _player.EvictPlaylist().Should().Equal("en-US: m1");
        await Sut.GoAhead();
        Sut.CurrentTermIndex.Should().Be(1);
        _player.EvictPlaylist().Should().Equal("ja-JP: p2");
        await Sut.GoAhead();
        Sut.CurrentTermIndex.Should().Be(1);
        _player.EvictPlaylist().Should().Equal("en-US: m2");
    }
    [Fact]
    public async Task CorrectGoNext_When_WrapAround_Should_Reshuffle()
    {
        await Sut.GoAhead(); // 1R
        await Sut.GoAhead(); // 2
        await Sut.GoAhead(); // 2R
        _studySet.EvictWorkLog().Should().BeEmpty();
        await Sut.GoAhead(); // Reshuffle
        _studySet.EvictWorkLog().Should().Equal("reshuffled");
        IgnorePlayActions();
    }


    [Fact]
    public async Task GoBack_When_At_TheFirstCard_Should_Not_Reshuffle()
    {
        _studySet.EvictWorkLog().Should().BeEmpty();
        await Sut.GoBack(); // 1R
        _studySet.EvictWorkLog().Should().BeEmpty();
        Sut.CurrentTermIndex.Should().Be(0);
    }

    void IDisposable.Dispose() => AndNoOtherEffects();
}