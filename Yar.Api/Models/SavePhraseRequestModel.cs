namespace Yar.Api.Models
{
    public class SavePhraseRequestModel
    {
        public string Phrase { get; set; }
        public string Sentence { get; set; }
        public string Translation { get; set; }
        public int UserId { get; set; }
        public int LanguageId { get; set; }
        public string State { get; set; }
        public string PhraseBase { get; set; }
        public string Notes { get; set; }
        public bool HasMore { get; set; }
    }
}
