namespace Yar.Api.Models
{
    public class ReadIndexModel
    {
        public int TextId { get;  }
        public bool AsParallel { get; }
        public int LanguageId { get; }
        public bool IsParallel { get; }
        public int? PrevTextId { get; }
        public int? NextTextId { get; }
        public string Title { get; }

        public ReadIndexModel(int textId, bool asParallel, int languageId, bool isParallel, int? prevTextId, int? nextTextId, string title)
        {
            TextId = textId;
            AsParallel = asParallel;
            LanguageId = languageId;
            IsParallel = isParallel;
            PrevTextId = prevTextId;
            NextTextId = nextTextId;
            Title = title;
        }
    }
}
