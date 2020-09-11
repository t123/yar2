using Yar.Data;

namespace Yar.Api.Models
{
    public class LanguageIndexModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static LanguageIndexModel From(Language language)
        {
            return new LanguageIndexModel
            {
                Id = language.Id,
                Name = language.Name
            };
        }
    }
}
