using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Yar.Data;

namespace Yar.Api.Models
{
    public class TextViewModel
    {
        public int Id { get; set; }
        public string L1Text { get; set; }
        public string L2Text { get; set; }
        public int UserId { get; set; }
        public int LanguageId { get; set; }
        public int? Language2Id { get; set; }
        public string LanguageName { get; set; }
        public string Title { get; set; }
        public string Collection { get; set; }
        public int? CollectionNo { get; set; }
        public bool IsParallel { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? LastRead { get; set; }

        public string[] AvailableCollections { get; set; }
        public Dictionary<int, string> AvailableLanguages { get; set; }

        public static TextViewModel From(Text text, IEnumerable<string> collections, IEnumerable<Language> languages)
        {
            if (text == null)
            {
                return new TextViewModel()
                {
                    AvailableCollections = collections?.ToArray() ?? new string[0],
                    AvailableLanguages = languages?.ToDictionary(x => x.Id, x => x.Name) ?? new Dictionary<int, string>()
                };
            }

            return new TextViewModel
            {
                Id = text.Id,
                L1Text = text.L1Text,
                L2Text = text.L2Text,
                UserId = text.User.Id,
                LanguageId = text.Language.Id,
                Language2Id = text.Language2?.Id,
                LanguageName = text.Language.Name,
                Title = text.Title,
                Collection = text.Collection,
                CollectionNo = text.CollectionNo,
                IsParallel = text.IsParallel,
                Created = text.Created,
                Updated = text.Updated,
                LastRead = text.LastRead,
                AvailableCollections = collections?.ToArray() ?? new string[0],
                AvailableLanguages = languages?.ToDictionary(x => x.Id, x => x.Name) ?? new Dictionary<int, string>()
            };
        }
    }
}
