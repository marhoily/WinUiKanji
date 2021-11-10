using Shared;
using System.IO;

namespace Tests;

public sealed class StudySetTests
{
    [Fact]
    public async Task FullCycle()
    {
        string fileName = PrepareTempStudySetFile();
        var sut = new StudySet();
        sut.Load(fileName);
        sut.GetShuffle()[0].WellKnown++;
        await sut.SaveAsync();
        var sut2 = new StudySet();
        sut2.Load(fileName);
        sut2.GetShuffle()[0].WellKnown.Should().Be(1);
    }

    private static string PrepareTempStudySetFile()
    {
        var fileName = Path.Combine(Path.GetTempPath(), "tempSet.csv");
        File.WriteAllText(fileName, @"Kanji,Meaning,ToPronounce,WellKnown
火を付ける,to turn on the lights,ひをつける,0");
        return fileName;
    }
}
