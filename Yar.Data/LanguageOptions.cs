namespace Yar.Data
{
    public class LanguageOptions
    {
        public const string TranslationMethod = "TranslationMethod";
        public const string TranslationMethodDefault = "Google";

        public const string BingTranslationUrl = "BingTranslationUrl";
        public const string BingTranslationUrlDefault = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&from=en&to=en";

        public const string BingTranslationKey = "BingTranslationKey";
        public const string BingTranslationKeyDefault = "";

        public const string GoogleTranslationSource = "GoogleTranslationSource";
        public const string GoogleTranslationSourceDefault = "";

        public const string GoogleTranslationTarget = "GoogleTranslationTarget";
        public const string GoogleTranslationTargetDefault = "";

        public const string GoogleTranslationKey = "GoogleTranslationKey";
        public const string GoogleTranslationKeyDefault = "";

        public const string LeftToRight = "LeftToRight";
        public const bool LeftToRightDefault = true;

        public const string Paged = "Paged";
        public const bool PagedDefault = true;

        public const string Spaced = "Spaced";
        public const bool SpacedDefault = true;

        public const string MultipleSentences = "MultipleSentences";
        public const bool MultipleSentencesDefault = true;

        public const string PopupModal = "PopupModal";
        public const string PopupModalDefault = "popup";

        public const string CopyClipboard = "CopyClipboard";
        public const bool CopyClipboardDefault = false;

        public const string ShowDefinitions = "ShowDefinitions";
        public const bool ShowDefinitionsDefault = true;

        public const string ForvoLanguageCode = "ForvoLanguageCode";
        public const string ForvoLanguageCodeDefault = "";

        public const string GoogleTranslateUrl = "GoogleTranslateUrl";
        public const string GoogleTranslateUrlDefault = "";

        public const string MaxFragmentParseLength = "MaxFragmentParseLength";
        public const int MaxFragmentParseLengthDefault = 3;

        public const string ShowTermStatistics = "ShowTermStatistics";
        public const bool ShowTermStatisticsDefault = true;

        public const string MostCommonTerms = "MostCommonTerms";
        public const int MostCommonTermsDefault = 20;

        public const string CentreModal = "CentreModal";
        public const bool CentreModalDefault = false;

        public const string CustomDictionaryUrl = "DictionaryUrl";
        public const string CustomDictionaryUrlDefault = "";

        public const string ForvoUrl = "ForvoUrl";
        public const string ForvoUrlDefault = "";

        public const string CustomDictionaryAuto = "CustomDictionaryAuto";
        public const bool CustomDictionaryAutoDefault = false;

        public const string EnableLeitner = "EnableLeitner";
        public const bool EnableLeitnerDefault = false;

        public const string LeitnerMultiplier = "LeitnerMultiplier";
        public const decimal LeitnerMultiplierDefault = 2.05m;

        public const string StateOnOpen = "StateOnOpen";
        public const string StateOnOpenDefault = "";

        public const string SingleViewPercentage = "SingleViewPercentage";
        public const int SingleViewPercentageDefault = 45;

        public const string SingleLineHeight= "SingleLineHeight";
        public const decimal SingleLineHeightDefault = 1.3m;

        public const string SingleFontSize = "SingleFontSize";
        public const decimal SingleFontSizeDefault = 1.6m;

        public const string ParallelLineHeight = "ParallelLineHeight";
        public const decimal ParallelLineHeightDefault = 1.3m;

        public const string ParallelFontSize = "ParallelFontSize";
        public const decimal ParallelFontSizeDefault = 1.4m;

        public const string FontColor = "FontColor";
        public const string FontColorDefault = "#000";

        public const string BackgroundColor = "BackgroundColor";
        public const string BackgroundColorDefault = "#f4f4eb";

        public const string FontFamily = "FontFamily";
        public const string FontFamilyDefault = @"""Times New Roman"", Times, serif";
    }
}
