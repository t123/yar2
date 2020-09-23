using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Yar.BLL.Dto;
using Yar.Data;

namespace Yar.BLL
{
    public class TextService
    {
        private readonly GenericRepository<Text> _repository;
        private readonly GenericRepository<User> _userRepository;
        private readonly GenericRepository<Language> _languageRepository;
        private readonly GenericRepository<Word> _wordRepository;

        public TextService(ISession session)
        {
            _repository = new GenericRepository<Text>(session);
            _userRepository = new GenericRepository<User>(session);
            _languageRepository = new GenericRepository<Language>(session);
            _wordRepository = new GenericRepository<Word>(session);
        }

        public IEnumerable<Text> Get(int userId)
        {
            return _repository.Get().Where(x => x.User.Id == userId);
        }

        public Text Get(int userId, int textId)
        {
            return _repository
                .Get()
                .Where(x => x.User.Id == userId && x.Id == textId).SingleOrDefault();
        }

        public (Text Previous, Text Next) GetPreviousAndNext(Text text)
        {
            if (!string.IsNullOrEmpty(text.Collection) && text.CollectionNo.HasValue)
            {
                var prevText = _repository
                    .Get()
                    .Where(x =>
                        x.User.Id == text.User.Id &&
                        x.Language.Id == text.Language.Id &&
                        x.Collection == text.Collection &&
                        x.CollectionNo < text.CollectionNo
                    )
                    .OrderByDescending(x => x.CollectionNo)
                    .FirstOrDefault();

                var nextText = _repository
                    .Get()
                    .Where(x =>
                        x.User.Id == text.User.Id &&
                        x.Language.Id == text.Language.Id &&
                        x.Collection == text.Collection &&
                        x.CollectionNo > text.CollectionNo
                    )
                    .OrderBy(x => x.CollectionNo)
                    .FirstOrDefault();

                return (prevText, nextText);
            }

            return (null, null);
        }

        public IEnumerable<string> GetCollections(int userId, int languageId)
        {
            var collections = _repository.Get().Where(x => x.User.Id == userId);

            if (languageId > 0)
            {
                collections = collections.Where(x => x.Language.Id == languageId);
            }

            return collections
                .Select(x => x.Collection)
                .Distinct()
                .AsEnumerable().Where(x => !string.IsNullOrEmpty(x))
                .OrderBy(x => x);
        }

        public void Delete(int userId, int textId)
        {
            var text = Get(userId, textId);

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _repository.Delete(text);
        }

        public void SetLastRead(int userId, int textId)
        {
            var text = Get(userId, textId);

            if (text == null)
            {
                return;
            }

            text.LastRead = DateTime.UtcNow;
            _repository.Save(text);
        }

        public Text Save(int userId, PostTextDto text)
        {
            var user = _userRepository.GetById(userId);

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Text obj;

            if (text.Id == 0)
            {
                obj = new Text()
                {
                    L1Text = text.L1Text ?? "",
                    L2Text = text.L2Text ?? "",
                    Collection = text.Collection ?? "",
                    CollectionNo = text.CollectionNo,
                    Language = _languageRepository.GetById(text.LanguageId),
                    Language2 = text.Language2Id.HasValue ? _languageRepository.GetById(text.Language2Id) : null,
                    Title = text.Title ?? "",
                    User = user,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    LastRead = null,
                    IsParallel = !string.IsNullOrWhiteSpace(text.L2Text)
                };

                user.Texts.Add(obj);
                _userRepository.Save(user);
            }
            else
            {
                obj = Get(userId, text.Id);

                if (obj == null)
                {
                    throw new Exception("Text not found");
                }

                obj.Title = text.Title ?? "";
                obj.L1Text = text.L1Text ?? "";
                obj.L2Text = text.L2Text ?? "";
                obj.Collection = text.Collection ?? "";
                obj.CollectionNo = text.CollectionNo;
                obj.Updated = DateTime.UtcNow;
                obj.IsParallel = !string.IsNullOrWhiteSpace(text.L2Text);
                obj.Language = _languageRepository.GetById(text.LanguageId);
                obj.Language2 = text.Language2Id.HasValue ? _languageRepository.GetById(text.Language2Id) : null;

                _repository.Save(obj);
            }

            var parser = new ParserService(new TextParserHelper());
            var output = parser.Parse(obj, _wordRepository.Get().Where(x => x.User.Id == userId && x.Language.Id == obj.Language.Id).ToArray(), false);

            obj.Total = output.Total;
            obj.Known = output.Known;
            obj.NotKnown = output.NotKnown;
            obj.Ignored = output.Ignored;
            obj.NotSeen = output.NotSeen;

            _repository.Save(obj);

            return obj;
        }

        public void Parse(int userId, int textId)
        {
            var text = Get(userId, textId);

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var parser = new ParserService(new TextParserHelper());
            var output = parser.Parse(text, _wordRepository.Get().Where(x => x.User.Id == userId && x.Language.Id == text.Language.Id).ToArray(), false);

            Parse(userId, textId, output);
        }

        public void Parse(int userId, int textId, ParserResult output)
        {
            var text = Get(userId, textId);

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            text.Total = output.Total;
            text.Known = output.Known;
            text.NotKnown = output.NotKnown;
            text.Ignored = output.Ignored;
            text.NotSeen = output.NotSeen;

            _repository.Save(text);
        }

        public void Archive(int userId, int textId, bool state)
        {
            var text = Get(userId, textId);

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            text.IsArchived = state;

            _repository.Save(text);
        }
    }
}
