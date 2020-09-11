using System;

namespace Yar.Api.Models
{
    public class UndoRequestModel
    {
        public string Uuid { get; set; }
    }

    public class UndoResponseModel
    {
        public string Phrase { get; set; }
        public string PhraseLower { get; set; }
        public bool Success { get; set; }
    }
}
