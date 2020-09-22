using System;

namespace Yar.Data
{
    public class Text
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Language Language { get; set; }
        public virtual Language Language2 { get; set; }
        public virtual string L1Text { get; set; }
        public virtual string L2Text { get; set; }
        public virtual string Title { get; set; }
        public virtual string Collection { get; set; }
        public virtual int? CollectionNo { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        public virtual DateTime? LastRead { get; set; }
        public virtual bool IsParallel { get; set; }
        public virtual bool IsArchived { get; set; }

        public virtual int NotSeen { get; set; }
        public virtual int Total { get; set; }
        public virtual int Known { get; set; }
        public virtual int NotKnown { get; set; }
        public virtual int Ignored { get; set; }
    }
}
