namespace Yar.Api.Models
{
    public class ReadIndexModel
    {
        public int TextId { get;  }
        public bool AsParallel { get; }
        public int LanguageId { get; }
        public bool IsParallel { get; }

        public ReadIndexModel(int textId, bool asParallel, int languageId, bool isParallel)
        {
            TextId = textId;
            AsParallel = asParallel;
            LanguageId = languageId;
            IsParallel = isParallel;
        }
    }
}
