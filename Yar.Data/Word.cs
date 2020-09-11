using System;
using System.Collections.Generic;
using System.Linq;

namespace Yar.Data
{
    public class Word
    {
        public virtual int Id { get; set; }
        public virtual Language Language { get; set; }
        public virtual User User { get; set; }
        public virtual string Phrase { get; set; }
        public virtual string PhraseLower { get; set; }
        public virtual string PhraseBase { get; set; }
        public virtual string Translation { get; set; }
        public virtual string Notes { get; set; }
        public virtual string Sentence => Sentences?.LastOrDefault()?.Sntnce ?? "";

        public virtual IList<Sentence> Sentences { get; set; }

        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }

        public virtual WordState State { get; set; }
        public virtual int? Box { get; set; }
        public virtual DateTime? NextReviewDate { get; set; }

        public virtual bool IsFragment { get; set; }
        public virtual int FragmentLength { get; set; }
        public virtual bool HasSentence => !string.IsNullOrWhiteSpace(Sentence);
        public virtual Guid Uuid { get; set; }
    }
}
