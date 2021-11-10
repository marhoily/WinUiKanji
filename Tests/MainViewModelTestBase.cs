using Shared;

namespace Tests;

public abstract class MainViewModelTestBase
{
    protected static readonly Card C1 = C(1);
    protected static readonly Card C2 = C(2);
    protected static readonly Card C3 = C(3);
    protected readonly FakePlayer _player = new();
    protected readonly FakeStudySet _studySet = new();
    protected MainViewModel Sut { get; set; } = null!; // to be set in derived classes
    protected MainViewModelTestBase()
    {
        _studySet.Cards.Add(C1);
        _studySet.Cards.Add(C2);
    }
    private static Card C(int number) => new Card { Meaning = "m" + number, ToPronounce = "p" + number };
    protected void AndNoOtherEffects()
    {
        _player.EvictPlaylist().Should().BeEmpty("playlist is not empty");
        _studySet.EvictWorkLog().Should().BeEmpty();
    }
    protected void IgnorePlayActions() => _player.EvictPlaylist();

}
