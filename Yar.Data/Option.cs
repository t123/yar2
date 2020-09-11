namespace Yar.Data
{
    public class Option
    {
        public virtual int Id { get; set; }
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }
        public virtual string ExpectedType { get; set; }
        public virtual Language Language { get; set; }
    }
}
