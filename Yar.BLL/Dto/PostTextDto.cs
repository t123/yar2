namespace Yar.BLL.Dto
{
    public class PostTextDto
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int? Language2Id { get; set; }
        public string L1Text { get; set; }
        public string L2Text { get; set; }
        public string Title { get; set; }
        public string Collection { get; set; }
        public int? CollectionNo { get; set; }
    }
}
