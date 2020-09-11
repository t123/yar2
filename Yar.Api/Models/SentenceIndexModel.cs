using System;
using Yar.Data;

namespace Yar.Api.Models
{
    public class SentenceIndexModel
    {
        public int Id { get; private set; }
        public string Sentence { get; private set; }
        public DateTime Created { get; private set; }

        public static SentenceIndexModel From(Sentence sentence)
        {
            return new SentenceIndexModel
            {
                Id = sentence.Id,
                Sentence = sentence.Sntnce,
                Created = sentence.Created
            };
        }
    }
}
