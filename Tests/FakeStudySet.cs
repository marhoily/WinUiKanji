using Shared;
namespace Tests;

public class FakeStudySet : IStudySet
{
    public List<string> WorkLog { get; } = new();
    public List<Card> Cards { get; } = new();
    public List<Card> GetShuffle() => Cards.ToList();
    public void Load(string fileName) { }
    public Task SaveAsync() => Task.CompletedTask;
}
