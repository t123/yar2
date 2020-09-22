using Yar.Data;

namespace Yar.Api.Models
{
    public class TextSearchModel
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string Title { get; set; }
        public string Collection { get; set; }
        public int? CollectionNo { get; set; }
        public bool IsParallel { get; set; }
        public bool IsArchived { get; set; }
        public string Created { get; set; }
        public string LastRead { get; set; }
        public string EditUrl { get; set; }
        public string ReadUrl { get; set; }

        public static TextSearchModel From(Text text, string editUrl, string readUrl)
        {
            return new TextSearchModel
            {
                Id = text.Id,
                LanguageName = text.Language.Name,
                Title = text.Title,
                Collection = text.Collection,
                CollectionNo = text.CollectionNo,
                IsParallel = text.IsParallel,
                IsArchived = text.IsArchived,
                Created = text.Created.ToString(),
                LastRead = text.LastRead?.ToString(),
                EditUrl = $"{editUrl}/{text.Id}",
                ReadUrl = $"{readUrl}/{text.Id}",
            };
        }
    }

    public class TextSearchDtoModel
    {
        public string Filter { get; set; }
    }
}
