using System;

namespace Yar.Data
{
    public class WordLog
    {
        public virtual int Id { get; set; }
        public virtual Word Word { get; set; }
        public virtual Language Language { get; set; }
        public virtual User User { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual string Action { get; set; }
        public virtual string State { get; set; }
    }
}
