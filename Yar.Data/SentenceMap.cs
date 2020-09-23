using FluentNHibernate.Mapping;

namespace Yar.Data
{
    public class SentenceMap : ClassMap<Sentence>
    {
        public SentenceMap()
        {
            Id(m => m.Id);
            Map(m => m.Created);
            Map(m => m.Sntnce);
            References(m => m.Word);
            References(m => m.Text);
        }
    }
}
