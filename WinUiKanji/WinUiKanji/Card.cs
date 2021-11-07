
using JetBrains.Annotations;

namespace WinUiKanji
{
    [UsedImplicitly]
    public sealed class Card
    {
        public string Kanji { get; set; }
        public string Meaning { get; set; }
        public string ToPronounce { get; set; }
        public int WellKnown { get; set; }
    }
}