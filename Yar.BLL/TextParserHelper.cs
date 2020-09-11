using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Yar.BLL
{
    public class TextParserHelper : IParserHelper
    {
        static TextParserHelper()
        {
        }

        public string[] SplitIntoParagraphs(string content)
        {
            if (string.IsNullOrEmpty(content))
                return new string[0];

            return content.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
        }

        public MatchCollection SplitIntoTerms(string sentence, Regex regex)
        {
            return regex.Matches(sentence);
        }

        public string ApplyTransform(XDocument document)
        {
            string xsl = File.ReadAllText(@"D:\gitrepository\personal\yar3\Yar\Yar.BLL\XSLT\Text.xslt");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.Indent = true;

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (XmlWriter writer = XmlWriter.Create(sw, settings))
                {
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(XmlReader.Create(new StringReader(xsl)));
                    xslt.Transform(document.CreateReader(), writer);
                }
            }

            return sb.ToString();
        }
    }

}
