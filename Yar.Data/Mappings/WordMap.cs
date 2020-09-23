using FluentNHibernate.Mapping;

namespace Yar.Data
{
    public class WordMap : ClassMap<Word>
    {
        public WordMap()
        {
            Id(m => m.Id);
            References(m => m.User);
            References(m => m.Language);
            Map(m => m.Phrase);
            Map(m => m.PhraseLower);
            Map(m => m.PhraseBase);
            Map(m => m.Translation);
            Map(m => m.Notes);
            Map(m => m.Created);
            Map(m => m.Updated);
            Map(m => m.State).CustomType<WordState>();
            Map(m => m.IsFragment);
            Map(m => m.Uuid);
            Map(m => m.FragmentLength);
            HasMany(m => m.Sentences).Cascade.AllDeleteOrphan();
        }
    }
}
