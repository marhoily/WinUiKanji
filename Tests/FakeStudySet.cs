using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    internal class FakeStudySet : IStudySet
    {
        private readonly List<Card> _cards;
        public FakeStudySet(params Card[] cards) => _cards = cards.ToList();
        public List<Card> GetShuffle() => _cards;
        public void Load(string fileName) { }
        public Task SaveAsync() => Task.CompletedTask;
    }
}