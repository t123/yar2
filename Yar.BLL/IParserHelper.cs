using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Yar.BLL
{
    public interface IParserHelper
    {
        string[] SplitIntoParagraphs(string content);
        MatchCollection SplitIntoTerms(string sentence, Regex regex);
        string ApplyTransform(XDocument document);
    }
}
