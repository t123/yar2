namespace Yar.Data
{
    public class ParserResult
    {
        public string Xml { get; set; }
        public string Html { get; set; }
        public bool AsParallel { get; set; }

        public int NotSeen { get; set; }
        public int Total { get; set; }
        public int Known { get; set; }
        public int NotKnown { get; set; }
        public int Ignored { get; set; }
    }
}
