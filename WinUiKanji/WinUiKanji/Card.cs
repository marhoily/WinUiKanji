namespace WinUiKanji
{
    public sealed record Card(string Kanji, string Meaning, string ToPronounce);
    // public sealed class Card
    // {
    //     public string Kanji { get; set; }
    //     public string Meaning { get; set; }
    //     public string ToPronounce { get; set; }
    //     public override string ToString()
    //     {
    //         return $"{Kanji}: {ToPronounce}";
    //     }
    // }
}