using System;

namespace Yar.Api.Models
{
    public class ChangePhraseStateRequestModel
    {
        public int LanguageId { get; set; }
        public string Phrase { get; set; }
    }
}
