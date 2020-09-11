using FluentNHibernate.Mapping;

namespace Yar.Data
{
    public class LanguageMap : ClassMap<Language>
    {
        public LanguageMap()
        {
            Id(m => m.Id);
            Map(m => m.Name);
            References(m => m.User);
            HasMany(m => m.Texts).Cascade.AllDeleteOrphan();
            Map(m => m.Created);
            Map(m => m.Updated);
            Map(m => m.RegEx);
            Map(m => m.IsDeleted);
            HasMany(m => m.Options).Cascade.AllDeleteOrphan();
        }
    }
}
