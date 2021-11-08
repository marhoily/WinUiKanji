using Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    internal class FakePlayer : IPlayer
    {
        public List<string> Playlist { get; } = new();
        public Task Blimp()
        {
            Playlist.Add("blimp");
            return Task.CompletedTask;
        }

        public Task Say(string culture, string text)
        {
            Playlist.Add(culture + ": " + text);
            return Task.CompletedTask;
        }
    }
}