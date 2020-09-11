using Newtonsoft.Json;
using Yar.Data;

namespace Yar.Api.Models
{
    public class LanguageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegEx { get; set; }

        public string TranslationMethod { get; set; }
        public string BingTranslationUrl { get; set; }
        public string BingTranslationKey { get; set; }
        public string GoogleTranslationSource { get; set; }
        public string GoogleTranslationTarget { get; set; }
        public string GoogleTranslationKey { get; set; }
        public string GoogleTranslateUrl { get; set; }
        public string ForvoLanguageCode { get; set; }
        public bool LeftToRight { get; set; }
        public bool Paged { get; set; }
        public bool Spaced { get; set; }
        public bool MultipleSentences { get; set; }
        public string PopupModal { get; set; }
        public bool CopyClipboard { get; set; }
        public bool ShowDefinitions { get; set; }
        public bool ShowTermStatistics { get; set; }
        public int MaxFragmentParseLength { get; set; }
        public int MostCommonTerms { get; set; }
        public string CustomDictionaryUrl { get; set; }
        public bool CustomDictionaryAuto { get; set; }
        public bool CentreModal { get; set; }
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

        public static LanguageViewModel From(Language language)
        {
            if (language == null)
            {
                return new LanguageViewModel
                {
                    Id = 0,
                    Name = "",
                    RegEx = "",
                    TranslationMethod = LanguageOptions.TranslationMethodDefault,
                    BingTranslationUrl = LanguageOptions.BingTranslationUrlDefault,
                    BingTranslationKey = LanguageOptions.BingTranslationKeyDefault,
                    GoogleTranslationSource = LanguageOptions.GoogleTranslationSourceDefault,
                    GoogleTranslationTarget = LanguageOptions.GoogleTranslationTargetDefault,
                    GoogleTranslationKey = LanguageOptions.GoogleTranslationKeyDefault,
                    LeftToRight = LanguageOptions.LeftToRightDefault,
                    Paged = LanguageOptions.PagedDefault,
                    Spaced = LanguageOptions.SpacedDefault,
                    MultipleSentences = LanguageOptions.MultipleSentencesDefault,
                    PopupModal = LanguageOptions.PopupModalDefault,
                    CopyClipboard = LanguageOptions.CopyClipboardDefault,
                    ShowDefinitions = LanguageOptions.ShowDefinitionsDefault,
                    ForvoLanguageCode = LanguageOptions.ForvoLanguageCodeDefault,
                    GoogleTranslateUrl = LanguageOptions.GoogleTranslateUrlDefault,
                    MaxFragmentParseLength = LanguageOptions.MaxFragmentParseLengthDefault,
                    ShowTermStatistics = LanguageOptions.ShowTermStatisticsDefault,
                    MostCommonTerms = LanguageOptions.MostCommonTermsDefault,
                    CustomDictionaryUrl = LanguageOptions.CustomDictionaryUrlDefault,
                    CustomDictionaryAuto = LanguageOptions.CustomDictionaryAutoDefault,
                    CentreModal = LanguageOptions.CentreModalDefault,
                    EnableLeitner = LanguageOptions.EnableLeitnerDefault,
                    LeitnerMultiplier = LanguageOptions.LeitnerMultiplierDefault,
                    StateOnOpen = LanguageOptions.StateOnOpenDefault,
                    ParallelFontSize = LanguageOptions.ParallelFontSizeDefault,
                    ParallelLineHeight = LanguageOptions.ParallelLineHeightDefault,
                    SingleFontSize = LanguageOptions.SingleFontSizeDefault,
                    SingleLineHeight = LanguageOptions.SingleLineHeightDefault,
                    SingleViewPercentage = LanguageOptions.SingleViewPercentageDefault,
                    FontColor = LanguageOptions.FontColor,
                    BackgroundColor = LanguageOptions.BackgroundColor,
                    FontFamily = LanguageOptions.FontFamily,
                };
            }

            var model = new LanguageViewModel
            {
                Id = language.Id,
                Name = language.Name,
                RegEx = language.RegEx,
                TranslationMethod = language.GetOption<string>(LanguageOptions.TranslationMethod, LanguageOptions.TranslationMethodDefault),
                BingTranslationUrl = language.GetOption<string>(LanguageOptions.BingTranslationUrl, LanguageOptions.BingTranslationUrlDefault) ?? "",
                BingTranslationKey = language.GetOption<string>(LanguageOptions.BingTranslationKey, "") ?? "",
                GoogleTranslationSource = language.GetOption<string>(LanguageOptions.GoogleTranslationSource, "") ?? "",
                GoogleTranslationTarget = language.GetOption<string>(LanguageOptions.GoogleTranslationTarget, "") ?? "",
                GoogleTranslationKey = language.GetOption<string>(LanguageOptions.GoogleTranslationKey, "") ?? "",
                LeftToRight = language.GetOption<bool>(LanguageOptions.LeftToRight, LanguageOptions.LeftToRightDefault),
                Paged = language.GetOption<bool>(LanguageOptions.Paged, LanguageOptions.PagedDefault),
                Spaced = language.GetOption<bool>(LanguageOptions.Spaced, LanguageOptions.SpacedDefault),
                MultipleSentences = language.GetOption<bool>(LanguageOptions.MultipleSentences, LanguageOptions.MultipleSentencesDefault),
                PopupModal = language.GetOption<string>(LanguageOptions.PopupModal, LanguageOptions.PopupModalDefault),
                CopyClipboard = language.GetOption<bool>(LanguageOptions.CopyClipboard, LanguageOptions.CopyClipboardDefault),
                ShowDefinitions = language.GetOption<bool>(LanguageOptions.ShowDefinitions, LanguageOptions.ShowDefinitionsDefault),
                ForvoLanguageCode = language.GetOption<string>(LanguageOptions.ForvoLanguageCode, LanguageOptions.ForvoLanguageCodeDefault),
                GoogleTranslateUrl = language.GetOption<string>(LanguageOptions.GoogleTranslateUrl, LanguageOptions.GoogleTranslateUrlDefault),
                MaxFragmentParseLength = language.GetOption<int>(LanguageOptions.MaxFragmentParseLength, LanguageOptions.MaxFragmentParseLengthDefault),
                ShowTermStatistics = language.GetOption<bool>(LanguageOptions.ShowTermStatistics, LanguageOptions.ShowTermStatisticsDefault),
                MostCommonTerms = language.GetOption<int>(LanguageOptions.MostCommonTerms, LanguageOptions.MostCommonTermsDefault),
                CustomDictionaryAuto = language.GetOption<bool>(LanguageOptions.CustomDictionaryAuto, LanguageOptions.CustomDictionaryAutoDefault),
                CustomDictionaryUrl = "",
                CentreModal = language.GetOption<bool>(LanguageOptions.CentreModal, LanguageOptions.CentreModalDefault),
                EnableLeitner = language.GetOption<bool>(LanguageOptions.EnableLeitner, false),
                LeitnerMultiplier = language.GetOption<decimal>(LanguageOptions.LeitnerMultiplier, 2.05m),
                StateOnOpen = language.GetOption<string>(LanguageOptions.StateOnOpen, "Unknown") ?? "Unknown",
                SingleViewPercentage = language.GetOption<decimal>(LanguageOptions.SingleViewPercentage, 45m),
                SingleLineHeight = language.GetOption<decimal>(LanguageOptions.SingleLineHeight, 1.2m),
                SingleFontSize = language.GetOption<decimal>(LanguageOptions.SingleFontSize, 1.4m),
                ParallelLineHeight = language.GetOption<decimal>(LanguageOptions.ParallelLineHeight, 1.2m),
                ParallelFontSize = language.GetOption<decimal>(LanguageOptions.ParallelFontSize, 1.4m),
                FontColor = language.GetOption<string>(LanguageOptions.FontColor, "#000"),
                BackgroundColor = language.GetOption<string>(LanguageOptions.BackgroundColor, "#f4f4eb"),
                FontFamily = language.GetOption<string>(LanguageOptions.FontFamily, @"""Times New Roman"", Times, serif"),
            };

            var dictionaries = language.GetOption<string>(LanguageOptions.CustomDictionaryUrl, null);

            if (dictionaries != null)
            {
                model.CustomDictionaryUrl = string.Join('\n', JsonConvert.DeserializeObject<string[]>(dictionaries));
            }

            return model;
        }
    }
}
