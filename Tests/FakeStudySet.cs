using Shared;
namespace Tests;

public class FakeStudySet : IStudySet
{
    public List<string> WorkLog { get; } = new();
    public List<Card> Cards { get; } = new();
    public List<Card> GetShuffle()
    {
        WorkLog.Add("reshuffled");
        return Cards.ToList();
    }
    public List<string> EvictWorkLog()
    {
        var copy = WorkLog.ToList();
        WorkLog.Clear();
        return copy;
    }

    public void Load(string fileName) { }
    public Task SaveAsync() => Task.CompletedTask;
}
