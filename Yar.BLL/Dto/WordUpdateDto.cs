using Yar.Data;

namespace Yar.BLL.Dto
{
    public class WordUpdateDto
    {
        public int Id { get; set; }
        public string Sentence { get; set; }
        public string Notes { get; set; }
        public string Phrase { get; set; }
        public string PhraseBase { get; set; }
        public string Translation { get; set; }
        public WordState State { get; set; }
        public int LanguageId { get; set; }
        public int TextId { get; set; }
    }
}
