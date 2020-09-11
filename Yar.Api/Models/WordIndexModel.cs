using System.Linq;
using Yar.Data;

namespace Yar.Api.Models
{
    public class WordIndexModel
    {
        public int Id { get; private set; }
        public string LanguageName { get; private set; }
        public string Phrase { get; private set; }
        public string Translation { get; private set; }
        public int SentenceCount => Sentences.Length;
        public SentenceIndexModel[] Sentences { get; private set; }

        public static WordIndexModel From(Word word)
        {
            return new WordIndexModel
            {
                Id = word.Id,
                LanguageName = word.Language.Name,
                Phrase = word.Phrase,
                Translation = word.Translation,
                Sentences = word.Sentences.Select(s => SentenceIndexModel.From(s)).ToArray()
            };
        }
    }
}
