
using JetBrains.Annotations;

namespace WinUiKanji
{
    [UsedImplicitly]
    public sealed record Card(string Kanji, string Meaning, string ToPronounce);
}