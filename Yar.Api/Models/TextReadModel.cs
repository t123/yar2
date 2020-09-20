using Newtonsoft.Json;
using Yar.Data;

namespace Yar.Api.Models
{
    public class TextReadModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string Title { get; set; }
        public string Collection { get; set; }
        public int? CollectionNo { get; set; }
        public bool IsParallel { get; set; }
        public bool AsParallel { get; set; }
        public string Html { get; set; }
        public LanguageOptionsModel Options { get; set; }

        public static TextReadModel From(Text text, ParserResult parserResult)
        {
            var model = new TextReadModel
            {
                Id = text.Id,
                UserId = text.User.Id,
                LanguageId = text.Language.Id,
                LanguageName = text.Language.Name,
                Title = text.Title,
                Collection = text.Collection,
                CollectionNo = text.CollectionNo,
                IsParallel = text.IsParallel,
                Html = parserResult.Html,
                AsParallel = parserResult.AsParallel,
                Options = new LanguageOptionsModel
                {
                    ShowDefinitions = text.Language?.GetOption<bool>(LanguageOptions.ShowDefinitions) ?? true,
                    ShowTermStatistics = text.Language?.GetOption<bool>(LanguageOptions.ShowTermStatistics) ?? true,
                    CopyClipboard = text.Language?.GetOption<bool>(LanguageOptions.CopyClipboard) ?? false,
                    LeftToRight = text.Language?.GetOption<bool>(LanguageOptions.LeftToRight) ?? true,
                    MultipleSentences = text.Language?.GetOption<bool>(LanguageOptions.MultipleSentences) ?? true,
                    Paged = text.Language?.GetOption<bool>(LanguageOptions.Paged) ?? true,
                    PopupModal = text.Language?.GetOption<string>(LanguageOptions.PopupModal) ?? "popup",
                    Spaced = text.Language?.GetOption<bool>(LanguageOptions.Spaced) ?? true,
                    L2LeftToRight = text.Language2?.GetOption<bool>(LanguageOptions.LeftToRight) ?? true,
                    L2Spaced = text.Language2?.GetOption<bool>(LanguageOptions.Spaced) ?? true,
                    HasBing = !string.IsNullOrEmpty(text.Language?.GetOption<string>(LanguageOptions.BingTranslationKey, "")),
                    HasGoogle = !string.IsNullOrEmpty(text.Language?.GetOption<string>(LanguageOptions.GoogleTranslationKey, "")),
                    ForvoLanguageCode = text.Language?.GetOption<string>(LanguageOptions.ForvoLanguageCode, ""),
                    GoogleTranslateUrl = text.Language?.GetOption<string>(LanguageOptions.GoogleTranslateUrl, ""),
                    DeepLUrl = text.Language?.GetOption<string>(LanguageOptions.DeepLUrl, ""),
                    MostCommonTerms = text.Language?.GetOption<int>(LanguageOptions.MostCommonTerms, 20) ?? 20,
                    CustomDictionaryAuto = text.Language?.GetOption<bool>(LanguageOptions.CustomDictionaryAuto, false) ?? false,
                    CentreModal = text.Language?.GetOption<bool>(LanguageOptions.CentreModal, false) ?? false,
                    StateOnOpen = text.Language?.GetOption<string>(LanguageOptions.StateOnOpen, "Unknown") ?? "Unknown",
                    SingleViewPercentage = text.Language?.GetOption<decimal>(LanguageOptions.SingleViewPercentage, 45m) ?? 45m,
                    SingleLineHeight = text.Language?.GetOption<decimal>(LanguageOptions.SingleLineHeight, 1.2m) ?? 1.2m,
                    SingleFontSize = text.Language?.GetOption<decimal>(LanguageOptions.SingleFontSize, 1.4m) ?? 1.4m,
                    ParallelLineHeight = text.Language?.GetOption<decimal>(LanguageOptions.ParallelLineHeight, 1.2m) ?? 1.2m,
                    ParallelFontSize = text.Language?.GetOption<decimal>(LanguageOptions.ParallelFontSize, 1.4m) ?? 1.4m,
                    FontColor = text.Language?.GetOption<string>(LanguageOptions.FontColor, "#000"),
                    BackgroundColor = text.Language?.GetOption<string>(LanguageOptions.BackgroundColor, "#f4f4eb"),
                    FontFamily = text.Language?.GetOption<string>(LanguageOptions.FontFamily, @"""Times New Roman"", Times, serif"),
                    HighlightLines = text.Language?.GetOption<string>(LanguageOptions.HighlightLines, LanguageOptions.HighlightLinesDefault),
                    HighlightLinesColour = text.Language?.GetOption<string>(LanguageOptions.HighlightLinesColour, LanguageOptions.HighlightLinesColourDefault),
                }
            };

            var dictionaries = text.Language?.GetOption<string>(LanguageOptions.CustomDictionaryUrl, null);

            if (dictionaries == null)
            {
                model.Options.CustomDictionaryUrl = new string[0];
            }
            else
            {
                model.Options.CustomDictionaryUrl = JsonConvert.DeserializeObject<string[]>(dictionaries);
            }

            return model;
        }
    }

    public class LanguageOptionsModel
    {
        public bool LeftToRight { get; set; }
        public bool Paged { get; set; }
        public bool Spaced { get; set; }
        public bool MultipleSentences { get; set; }
        public string PopupModal { get; set; }
        public bool CopyClipboard { get; set; }
        public bool ShowDefinitions { get; set; }
        public bool ShowTermStatistics { get; set; }
        public bool HasGoogle { get; set; }
        public bool HasBing { get; set; }
        public bool HasGoogleTranslate => !string.IsNullOrWhiteSpace(GoogleTranslateUrl);
        public bool HasDeepL => !string.IsNullOrWhiteSpace(DeepLUrl);
        public bool HasForvo => !string.IsNullOrWhiteSpace(ForvoLanguageCode);
        public bool HasCustomDictionary => CustomDictionaryUrl.Length > 0;
        public string GoogleTranslateUrl { get; set; }
        public string DeepLUrl { get; set; }
        public string ForvoLanguageCode { get; set; }
        public int MostCommonTerms { get; set; }
        public string[] CustomDictionaryUrl { get; set; }
        public bool CustomDictionaryAuto { get; set; }
        public bool CentreModal { get; set; }

        public bool L2LeftToRight { get; set; }
        public bool L2Spaced { get; set; }

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
