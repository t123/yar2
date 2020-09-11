namespace Yar.Api.Models
{
    public class ReadWordResponseModel
    {
        public string Phrase { get; set; }
        public string PhraseBase { get; set; }
        public string PhraseLower { get; set; }
        public string Translation { get; set; }
        public string State { get; set; }
        public string Notes { get; set; }
        public bool CanUndo { get; set; }
        public string Uuid { get; set; }
    }
}
