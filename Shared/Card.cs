using JetBrains.Annotations;

namespace Shared
{
    [UsedImplicitly]
    public sealed class Card
    {
        public string Kanji { get; set; } = null!;
        public string Meaning { get; set; } = null!;
        public string ToPronounce { get; set; } = null!;
        public int WellKnown { get; set; }
    }
}