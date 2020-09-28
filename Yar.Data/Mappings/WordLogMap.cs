using FluentNHibernate.Mapping;

namespace Yar.Data
{
    public class WordLogMap : ClassMap<WordLog>
    {
        public WordLogMap()
        {
            Id(m => m.Id);
            References(m => m.Word);
            References(m => m.Language);
            References(m => m.User);
            Map(m => m.Created);
            Map(m => m.Action);
            Map(m => m.State);
        }
    }
}
