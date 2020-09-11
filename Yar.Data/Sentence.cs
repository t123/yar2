using System;

namespace Yar.Data
{
    public class Sentence
    {
        public virtual int Id { get; set; }
        public virtual string Sntnce { get; set; }
        public virtual Word Word { get; set; }
        public virtual DateTime Created { get; set; }
    }
}
