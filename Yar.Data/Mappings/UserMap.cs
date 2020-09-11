using FluentNHibernate.Mapping;

namespace Yar.Data
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(m => m.Id);
            Map(m => m.Username);
            HasMany(m => m.Languages).Cascade.AllDeleteOrphan();
            HasMany(m => m.Words).Cascade.AllDeleteOrphan();
            HasMany(m => m.Texts).Cascade.AllDeleteOrphan();
            Map(m => m.Created);
            Map(m => m.Updated);
            Map(m => m.LastLogin).Nullable(); 
        }
    }
}
