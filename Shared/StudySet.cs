using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public sealed class StudySet : IStudySet
    {
        private Card[]? _cards;
        private string? _fileName;
        public void Load(string fileName)
        {
            using var reader = new StreamReader(fileName);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            _cards = csv.GetRecords<Card>().ToArray();
        }

        public async Task SaveAsync()
        {
            if (_fileName == null) throw new InvalidOperationException("Call Load() first");
            using var writer = new StreamWriter(_fileName);
            foreach (var b in Encoding.UTF8.GetPreamble())
                writer.BaseStream.WriteByte(b);
            using var csv = new CsvWriter(writer,
                new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = Encoding.UTF8,
                });
            await csv.WriteRecordsAsync(_cards);
        }
        public List<Card> GetShuffle()
        {
            return _cards == null
                ? throw new InvalidOperationException("Call Load() first")
                : _cards.Where(card => card.WellKnown < 2).Shuffle();
        }
    }
}