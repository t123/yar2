using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Yar.Data;

namespace Yar.BLL
{
    public interface IParserService
    {
        ParserResult Parse(Text text, Word[] words, bool asParallel);
    }

    public class ParserService : IParserService
    {
        public const string DefaultRegex = @"([a-zA-ZÀ-ÖØ-öø-ÿĀ-ſƀ-ɏ\’\'-]+)|(\s+)|(\d+)|(__\d+__)|(<\/?[a-z][A-Z0-9]*[^>]*>)|(.)";

        private readonly ParserResult _output;
        private readonly IParserHelper _parserHelper;

        private Text _text;
        private Word[] _words;
        private Word[] _singleWords;
        private Word[] _fragmentWords;
        private Dictionary<string, Word> _lookup;
        private bool _asParallel;
        private Regex _termRegex;
        private Dictionary<string, int> _frequency = new Dictionary<string, int>();

        public ParserService(IParserHelper parserHelper)
        {
            _output = new ParserResult();
            _parserHelper = parserHelper;
        }

        public ParserResult Parse(Text text, Word[] words, bool asParallel)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
            _words = words ?? throw new ArgumentNullException(nameof(words));

            _words = _words.Where(w => w != null).Where(w => !string.IsNullOrWhiteSpace(w.Phrase)).ToArray();

            var maxFragmentLength = text.Language.GetOption<int>(LanguageOptions.MaxFragmentParseLength);

            if (maxFragmentLength <= 1)
            {
                maxFragmentLength = int.MaxValue;
            }

            _singleWords = _words.Where(x => !x.IsFragment).ToArray();
            _fragmentWords = _words.Where(x => x.IsFragment && x.FragmentLength <= maxFragmentLength).ToArray();
            _lookup = _singleWords.ToDictionary(word => word.Phrase.ToLowerInvariant(), word => word);
            _asParallel = text.IsParallel ? asParallel : false;

            try
            {
                _termRegex = string.IsNullOrWhiteSpace(text.Language.RegEx)
                    ? new Regex(DefaultRegex, RegexOptions.Compiled)
                    : new Regex(text.Language.RegEx, RegexOptions.Compiled);
            }
            catch
            {
                _termRegex = new Regex(DefaultRegex, RegexOptions.Compiled);
            }

            Parse();

            return _output;
        }

        private void Parse()
        {
            var fragmentResult = ParseFragments();
            var l1Content = _text.Title + "\n\n" + fragmentResult.Item1;
            var fragments = fragmentResult.Item2;

            var l1Paragraphs = _parserHelper.SplitIntoParagraphs(l1Content);
            var l2Paragraphs = _parserHelper.SplitIntoParagraphs($"{_text.Title}\n\n{_text.L2Text}");

            XDocument document = new XDocument();
            var rootNode = new XElement("root");

            var contentNode = new XElement("content");
            AddDataToContentNode(contentNode);

            for (int i = 0; i < l1Paragraphs.Length; i++)
            {
                var l1Paragraph = l1Paragraphs[i];

                if (string.IsNullOrWhiteSpace(l1Paragraph))
                {
                    continue;
                }

                var l2Paragraph = _asParallel && l2Paragraphs != null && i < l2Paragraphs.Length ? l2Paragraphs[i] : string.Empty;

                var joinNode = new XElement("join");
                var l1ParagraphNode = new XElement("paragraph");
                var l2ParagraphNode = new XElement("translation");
                l2ParagraphNode.Value = l2Paragraph;

                l1ParagraphNode.SetAttributeValue("direction", _text.Language.GetOption(LanguageOptions.LeftToRight, true) ? "ltr" : "rtl");
                l2ParagraphNode.SetAttributeValue("direction", _text.Language2?.GetOption(LanguageOptions.LeftToRight, true) ?? true ? "ltr" : "rtl");

                var terms = _parserHelper.SplitIntoTerms(l1Paragraph, _termRegex);

                foreach (Match term in terms)
                {
                    var node = CreateTermNode(term, fragments);

                    if (node != null)
                    {
                        l1ParagraphNode.Add(node);
                    }
                }

                joinNode.Add(l1ParagraphNode);
                joinNode.Add(l2ParagraphNode);
                contentNode.Add(joinNode);
            }

            rootNode.Add(contentNode);
            document.Add(rootNode);

            AddFrequencyData(document);

            // Newlines in XSLT transform cause extra spaces around punctuation. If anyone wants to try fix that... 
            _output.AsParallel = _asParallel;
            _output.Xml = document.ToString();
            _output.Html = Regex.Replace(_parserHelper.ApplyTransform(document), @"span>\s+<span", "span><span");
        }

        protected XElement CreateTermNode(Match term, Dictionary<string, XElement> fragments = null)
        {
            if (term.Groups[1].Success) //Term
            {
                var termNode = new XElement("term");
                var termLower = term.Value.ToLowerInvariant();

                _output.Total++;
                termNode.Value = term.Value;
                termNode.SetAttributeValue("phrase", termLower);
                termNode.SetAttributeValue("phraseClass", termLower.Replace("'", "_").Replace("\"", "_"));

                if (_frequency.ContainsKey(termLower))
                {
                    _frequency[termLower]++;
                }
                else
                {
                    _frequency[termLower] = 1;
                }

                if (_lookup.ContainsKey(termLower))
                {
                    var existing = _lookup[termLower];
                    termNode.SetAttributeValue("state", existing.State.ToString().ToLower());
                    termNode.SetAttributeValue("definition", existing.Translation);

                    switch (existing.State)
                    {
                        case WordState.Known:
                            _output.Known++;
                            break;

                        case WordState.NotKnown:
                            _output.NotKnown++;
                            break;

                        case WordState.Ignored:
                            _output.Ignored++;
                            break;
                    }
                }
                else
                {
                    _output.NotSeen++;
                    termNode.SetAttributeValue("state", WordState.NotSeen.ToString().ToLower());
                }

                return termNode;
            }
            else if (term.Groups[2].Success) //Whitespace 
            {
                var termNode = new XElement("whitespace");
                termNode.Value = term.Value;
                return termNode;
            }
            else if (term.Groups[3].Success) //Number
            {
                var termNode = new XElement("number");
                termNode.Value = term.Value;
                return termNode;
            }
            else if (term.Groups[4].Success) //Fragment 
            {
                if (fragments == null || !fragments.ContainsKey(term.Groups[4].Value))
                {
                    return null;
                }

                return fragments[term.Groups[4].Value];
            }
            else if (term.Groups[5].Success) //Tag 
            {
                return null;
            }
            else if (term.Groups[6].Success) //Punctuation
            {
                var termNode = new XElement("punctuation");
                termNode.Value = term.Value;
                return termNode;
            }

            return null;
        }

        private Tuple<string, Dictionary<string, XElement>> ParseFragments()
        {
            var counter = 0;
            var matchCache = new Dictionary<string, XElement>();
            var rDict = new Dictionary<string, XElement>();
            string content = _text.L1Text;

            foreach (var fragment in _fragmentWords)
            {
                var matches = Regex.Matches(content, fragment.Phrase, RegexOptions.IgnoreCase);

                if (!matches.Any())
                {
                    continue;
                }

                foreach (Match match in matches)
                {
                    if (matchCache.ContainsKey(match.Value))
                    {
                        continue;
                    }

                    var fragmentNode = new XElement("fragment");
                    fragmentNode.SetAttributeValue("termId", fragment.Id);
                    fragmentNode.SetAttributeValue("lower", fragment.PhraseLower);
                    fragmentNode.SetAttributeValue("phrase", match.Value);
                    fragmentNode.SetAttributeValue("state", fragment.State.ToString().ToLower());
                    fragmentNode.SetAttributeValue("definition", fragment.Translation);

                    var terms = _parserHelper.SplitIntoTerms(match.Value, _termRegex);

                    foreach (Match term in terms)
                    {
                        fragmentNode.Add(CreateTermNode(term));
                    }

                    matchCache.Add(match.Value, fragmentNode);
                    rDict.Add($"__{counter}__", fragmentNode);

                    content = content.Replace(match.Value, $"__{counter}__");
                    counter++;
                }
            }

            return new Tuple<string, Dictionary<string, XElement>>(content, rDict);
        }

        private void AddDataToContentNode(XElement contentNode)
        {
            contentNode.SetAttributeValue("isParallel", _asParallel.ToString().ToLower());
        }

        private void AddFrequencyData(XDocument document)
        {
            foreach (var term in document.Descendants("term"))
            {
                var occurrences = _frequency[term.Attribute("phrase").Value];
                var frequencyTotal = Math.Round((decimal)occurrences / (decimal)_output.Total * 100, 2);
                var frequencyNotSeen = Math.Round((decimal)occurrences / (decimal)_output.NotSeen * 100, 2);

                term.SetAttributeValue("occurrences", occurrences);
                term.SetAttributeValue("frequency-total", frequencyTotal);
                term.SetAttributeValue("frequency-notseen", frequencyNotSeen);
            }

            var mostCommon = _text.Language.GetOption<int>(LanguageOptions.MostCommonTerms);

            if (mostCommon > 0)
            {
                var terms = document
                    .Descendants("term")
                    .Where(x => x.Attribute("state").Value == WordState.NotSeen.ToString().ToLower())
                    .DistinctBy(x => x.Attribute("phrase").Value)
                    .OrderByDescending(x => (decimal)x.Attribute("frequency-notseen"))
                    .Take(mostCommon)
                    .Select(x => x.Attribute("phrase").Value);

                int counter = 1;

                foreach (var term in terms)
                {
                    document
                    .Descendants("term")
                    .Where(x => x.Attribute("phrase").Value == term)
                    .ToList()
                    .ForEach(t => t.SetAttributeValue("frequency-commonness", counter));

                    counter++;
                }
            }
        }
    }

    public static class LinqExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            GeneralPropertyComparer<T, TKey> comparer = new GeneralPropertyComparer<T, TKey>(property);
            return items.Distinct(comparer);
        }
    }

    public class GeneralPropertyComparer<T, TKey> : IEqualityComparer<T>
    {
        private Func<T, TKey> expr { get; set; }

        public GeneralPropertyComparer(Func<T, TKey> expr)
        {
            this.expr = expr;
        }

        public bool Equals(T left, T right)
        {
            var leftProp = expr.Invoke(left);
            var rightProp = expr.Invoke(right);

            if (leftProp == null && rightProp == null)
            {
                return true;
            }
            else if (leftProp == null ^ rightProp == null)
            {
                return false;
            }
            else
            {
                return leftProp.Equals(rightProp);
            }
        }

        public int GetHashCode(T obj)
        {
            var prop = expr.Invoke(obj);
            return (prop == null) ? 0 : prop.GetHashCode();
        }
    }
}
