using System;
using Yar.Data;

namespace Yar.Api.Models
{
    public class TextIndexModel
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string Title { get; set; }
        public string Collection { get; set; }
        public int? CollectionNo { get; set; }
        public bool IsParallel { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastRead { get; set; }

        public static TextIndexModel From(Text text)
        {
            return new TextIndexModel
            {
                Id = text.Id,
                LanguageName = text.Language.Name,
                Title = text.Title,
                Collection = text.Collection,
                CollectionNo = text.CollectionNo,
                IsParallel = text.IsParallel,
                Created = text.Created,
                LastRead = text.LastRead
            };
        }
    }
}
