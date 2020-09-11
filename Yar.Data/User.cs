using System;
using System.Collections.Generic;

namespace Yar.Data
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual IList<Language> Languages { get; set; } = new List<Language>();
        public virtual IList<Word> Words { get; set; } = new List<Word>();
        public virtual IList<Text> Texts { get; set; } = new List<Text>();
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        public virtual DateTime? LastLogin { get; set; }
    }
}
