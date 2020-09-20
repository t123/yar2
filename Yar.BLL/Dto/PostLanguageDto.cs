namespace Yar.BLL.Dto
{
    public class PostLanguageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegEx { get; set; }
        public string TranslationMethod { get; set; }
        public string BingTranslationUrl { get; set; }
        public string BingTranslationKey { get; set; }
        public string GoogleTranslationKey { get; set; }
        public string GoogleTranslationSource { get; set; }
        public string GoogleTranslationTarget { get; set; }
        public string GoogleTranslateUrl { get; set; }
        public string DeepLUrl { get; set; }
        public string ForvoLanguageCode { get; set; }
        public bool LeftToRight { get; set; }
        public bool Paged { get; set; }
        public bool Spaced { get; set; }
        public bool MultipleSentences { get; set; }
        public string PopupModal { get; set; }
        public bool CopyClipboard { get; set; }
        public bool ShowDefinitions { get; set; }
        public int MaxFragmentParseLength { get; set; }
        public int MostCommonTerms { get; set; }
        public bool ShowTermStatistics { get; set; }
        public bool CentreModal { get; set; }
        public string CustomDictionaryUrl { get; set; }
        public bool CustomDictionaryAuto { get; set; }
        public bool EnableLeitner { get; set; }
        public decimal LeitnerMultiplier { get; set; }
        public string StateOnOpen { get; set; }
        public decimal SingleViewPercentage { get; set; }
        public decimal SingleLineHeight { get; set; }
        public decimal SingleFontSize { get; set; }
        public decimal ParallelLineHeight { get; set; }
        public decimal ParallelFontSize { get; set; }
        public string FontColor { get; set; }
        public string BackgroundColor { get; set; }
        public string FontFamily { get; set; }
        public string HighlightLines { get; set; }
        public string HighlightLinesColour { get; set; }
    }
}
