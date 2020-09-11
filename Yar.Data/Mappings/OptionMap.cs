using FluentNHibernate.Mapping;

namespace Yar.Data
{
    public class OptionMap : ClassMap<Option>
    {
        public OptionMap()
        {
            Id(m => m.Id);
            References(m => m.Language);
            Map(m => m.Key);
            Map(m => m.Value);
            Map(m => m.ExpectedType);
        }
    }
}
