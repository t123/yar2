using Yar.Data;

namespace Yar.BLL.Dto
{
    public class WordCreateDto
    {
        public int LanguageId { get; set; }
        public string Notes { get; set; }
        public string Phrase { get; set; }
        public string PhraseBase { get; set; }
        public string Sentence { get; set; }
        public string Translation { get; set; }
        public WordState State { get; set; }
        public int TextId { get; set; }
    }

    public class WordTranslateCreateDto
    {
        public int LanguageId { get; set; }
        public string Phrase { get; set; }
        public string Sentence { get; set; }
        public string Translation { get; set; }
        public int TextId { get; set; }
    }

    public class WordTranslateUpdateDto
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Sentence { get; set; }
        public int TextId { get; set; }
    }
}
