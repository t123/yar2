using FluentNHibernate.Mapping;

namespace Yar.Data
{
    public class TextMap : ClassMap<Text>
    {
        public TextMap()
        {
            Id(m => m.Id);
            References(m => m.User).Not.Nullable();
            References(m => m.Language).Not.Nullable();
            References(m => m.Language2);
            Map(m => m.L1Text);
            Map(m => m.L2Text);
            Map(m => m.Title);
            Map(m => m.Collection);
            Map(m => m.CollectionNo);
            Map(m => m.Created);
            Map(m => m.Updated);
            Map(m => m.LastRead);
            Map(m => m.IsParallel);
            Map(m => m.NotSeen);
            Map(m => m.Total);
            Map(m => m.Known);
            Map(m => m.NotKnown);
            Map(m => m.Ignored);
            Map(m => m.IsArchived);
        }
    }
}
