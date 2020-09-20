using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Yar.BLL.Dto;
using Yar.Data;

namespace Yar.BLL
{
    public class LanguageService
    {
        private readonly GenericRepository<Language> _repository;
        private readonly GenericRepository<User> _userRepository;

        public LanguageService(ISession session)
        {
            _repository = new GenericRepository<Language>(session);
            _userRepository = new GenericRepository<User>(session);
        }

        public IEnumerable<Language> Get(int userId)
        {
            return _repository.Get().Where(x => x.User.Id == userId && !x.IsDeleted);
        }

        public Language Get(int userId, int languageId)
        {
            return _repository.Get().SingleOrDefault(x => x.User.Id == userId && x.Id == languageId && !x.IsDeleted);
        }

        public void Delete(int userId, int languageId)
        {
            var language = Get(userId, languageId);

            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            language.IsDeleted = true;
            _repository.Save(language);
        }

        public void Save(int userId, PostLanguageDto language)
        {
            var user = _userRepository.GetById(userId);

            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Language obj;

            if (language.Id == 0)
            {
                obj = new Language()
                {
                    User = user,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    IsDeleted = false,
                    Name = language.Name ?? "",
                    RegEx = language.RegEx ?? ""
                };

                user.Languages.Add(obj);
                _userRepository.Save(user);
            }
            else
            {
                obj = Get(userId, language.Id);

                if (obj == null)
                {
                    throw new Exception("Language not found");
                }

                obj.Name = language.Name ?? "";
                obj.RegEx = language.RegEx ?? "";
                obj.Updated = DateTime.UtcNow;

                _repository.Save(obj);
            }

            obj.SetOption(LanguageOptions.TranslationMethod, language.TranslationMethod);
            obj.SetOption(LanguageOptions.BingTranslationUrl, language.BingTranslationUrl);
            obj.SetOption(LanguageOptions.BingTranslationKey, language.BingTranslationKey);
            obj.SetOption(LanguageOptions.GoogleTranslationKey, language.GoogleTranslationKey);
            obj.SetOption(LanguageOptions.GoogleTranslationSource, language.GoogleTranslationSource);
            obj.SetOption(LanguageOptions.GoogleTranslationTarget, language.GoogleTranslationTarget);
            obj.SetOption(LanguageOptions.LeftToRight, language.LeftToRight);
            obj.SetOption(LanguageOptions.Paged, language.Paged);
            obj.SetOption(LanguageOptions.Spaced, language.Spaced);
            obj.SetOption(LanguageOptions.MultipleSentences, language.MultipleSentences);
            obj.SetOption(LanguageOptions.PopupModal, language.PopupModal);
            obj.SetOption(LanguageOptions.CopyClipboard, language.CopyClipboard);
            obj.SetOption(LanguageOptions.ShowDefinitions, language.ShowDefinitions);
            obj.SetOption(LanguageOptions.GoogleTranslateUrl, language.GoogleTranslateUrl);
            obj.SetOption(LanguageOptions.DeepLUrl, language.DeepLUrl);
            obj.SetOption(LanguageOptions.ForvoLanguageCode, language.ForvoLanguageCode);
            obj.SetOption(LanguageOptions.MaxFragmentParseLength, language.MaxFragmentParseLength);
            obj.SetOption(LanguageOptions.ShowTermStatistics, language.ShowTermStatistics);
            obj.SetOption(LanguageOptions.MostCommonTerms, language.MostCommonTerms);
            obj.SetOption(LanguageOptions.CustomDictionaryAuto, language.CustomDictionaryAuto);
            obj.SetOption(LanguageOptions.CustomDictionaryUrl, JsonConvert.SerializeObject(language.CustomDictionaryUrl.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)));
            obj.SetOption(LanguageOptions.StateOnOpen, language.StateOnOpen);
            obj.SetOption(LanguageOptions.SingleViewPercentage, language.SingleViewPercentage);
            obj.SetOption(LanguageOptions.SingleLineHeight, language.SingleLineHeight);
            obj.SetOption(LanguageOptions.SingleFontSize, language.SingleFontSize);
            obj.SetOption(LanguageOptions.ParallelLineHeight, language.ParallelLineHeight);
            obj.SetOption(LanguageOptions.ParallelFontSize, language.ParallelFontSize);
            obj.SetOption(LanguageOptions.FontColor, language.FontColor);
            obj.SetOption(LanguageOptions.BackgroundColor, language.BackgroundColor);
            obj.SetOption(LanguageOptions.FontFamily, language.FontFamily);
            obj.SetOption(LanguageOptions.HighlightLines, language.HighlightLines);
            obj.SetOption(LanguageOptions.HighlightLinesColour, language.HighlightLinesColour);

            _repository.Save(obj);
        }
    }
}
