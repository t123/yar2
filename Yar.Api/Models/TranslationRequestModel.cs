namespace Yar.Api.Models
{
    public class TranslationRequestModel
    {
        public string Phrase { get; set; }
        public string Sentence { get; set; }
        public int UserId { get; set; }
        public int LanguageId { get; set; }
        public string Method { get; set; }
        public int TextId { get; set; }
    }

    public class RetranslateResponseModel
    {
        public string Phrase { get; set; }
        public string PhraseLower { get; set; }
        public string Translation { get; set; }
    }
}
