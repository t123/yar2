using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Yar.BLL.Dto;
using Yar.Data;

namespace Yar.BLL
{
    public class WordService
    {
        private readonly GenericRepository<Word> _repository;
        private readonly GenericRepository<User> _userRepository;
        private readonly GenericRepository<Language> _languageRepository;
        private readonly GenericRepository<Text> _textRepository;

        public WordService(ISession session)
        {
            _repository = new GenericRepository<Word>(session);
            _userRepository = new GenericRepository<User>(session);
            _languageRepository = new GenericRepository<Language>(session);
            _textRepository = new GenericRepository<Text>(session);
        }

        public Word GetById(int userId, int wordId)
        {
            return _repository.Get().SingleOrDefault(x => x.User.Id == userId && x.Id == wordId);
        }

        public Word Get(int userId, Guid uuid)
        {
            return _repository.Get().SingleOrDefault(x => x.User.Id == userId && x.Uuid == uuid);
        }

        public IEnumerable<Word> Get(int userId)
        {
            return _repository.Get().Where(x => x.User.Id == userId);
        }

        public IEnumerable<Word> Get(int userId, int languageId)
        {
            return _repository.Get().Where(x => x.User.Id == userId && x.Language.Id == languageId);
        }

        public Word Get(int userId, int languageId, string phrase)
        {
            return _repository.Get().SingleOrDefault(x => x.User.Id == userId && x.Language.Id == languageId && x.PhraseLower == phrase.ToLowerInvariant());
        }

        public void Delete(int userId, int wordId)
        {
            var word = GetById(userId, wordId);

            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }

            _repository.Delete(word);
        }

        private bool IsPhraseFragment(string phrase)
        {
            return phrase?.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Count() > 1;
        }

        private int PhraseFragmentLength(string phrase)
        {
            return phrase?.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Count() ?? 1;
        }

        private Word CreateWord(int userId, Word word)
        {
            var user = _userRepository.GetById(userId);

            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            word.User = user;
            word.Created = DateTime.UtcNow;
            word.Updated = DateTime.UtcNow;
            word.Uuid = Guid.NewGuid();

            if (word.IsFragment)
            {
                word.FragmentLength = PhraseFragmentLength(word.Phrase);
            }
            else
            {
                word.FragmentLength = 1;
            }

            user.Words.Add(word);
            _userRepository.Save(user);

            return word;
        }

        private Word UpdateWord(int userId, Word word)
        {
            var user = _userRepository.GetById(userId);

            if (word == null)
            {
                throw new ArgumentNullException(nameof(word));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var obj = GetById(userId, word.Id);

            if (obj == null)
            {
                throw new NullReferenceException();
            }

            obj.Updated = DateTime.UtcNow;

            _repository.Save(word);

            return word;
        }

        public Word Save(int userId, WordTranslateCreateDto word)
        {
            var language = _languageRepository.GetById(word.LanguageId);
            WordState initialState;

            if (!Enum.TryParse(language.GetOption<string>(LanguageOptions.StateOnOpen), true, out initialState))
            {
                initialState = WordState.NotKnown;
            }

            var obj = new Word()
            {
                Language = language,
                Phrase = word.Phrase ?? "",
                PhraseLower = (word.Phrase ?? "").ToLowerInvariant(),
                PhraseBase = "",
                IsFragment = IsPhraseFragment(word.Phrase),
                State = initialState,
                Translation = word.Translation ?? "",
                Notes = "",
            };

            AddSentence(obj, language, word.Sentence, word.TextId);

            return CreateWord(userId, obj);
        }

        public Word CycleState(int userId, WordStateUpdateDto word)
        {
            var obj = GetById(userId, word.Id);

            if (obj == null)
            {
                throw new NullReferenceException();
            }

            switch (obj.State)
            {
                //case WordState.Known: obj.State = WordState.NotSeen; break;
                //case WordState.NotSeen: obj.State = WordState.NotKnown; break;
                //case WordState.NotKnown: obj.State = WordState.Known; break;

                case WordState.Known: obj.State = WordState.NotKnown; break;
                case WordState.NotKnown: obj.State = WordState.Known; break;
                default: break;
            }

            return UpdateWord(userId, obj);
        }

        public Word Save(int userId, WordCreateDto word)
        {
            var language = _languageRepository.GetById(word.LanguageId);

            var obj = new Word()
            {
                Language = language,
                Phrase = word.Phrase ?? "",
                PhraseLower = (word.Phrase ?? "").ToLowerInvariant(),
                PhraseBase = word.PhraseBase ?? "",
                IsFragment = IsPhraseFragment(word.Phrase),
                State = word.State,
                Translation = word.Translation ?? "",
                Notes = word.Notes ?? "",
            };

            AddSentence(obj, language, word.Sentence, word.TextId);

            return CreateWord(userId, obj);
        }

        public Word Save(int userId, WordUpdateDto word)
        {
            var obj = GetById(userId, word.Id);

            if (obj == null)
            {
                throw new NullReferenceException();
            }

            var language = _languageRepository.GetById(word.LanguageId);

            obj.Phrase = word.Phrase;
            obj.PhraseBase = word.PhraseBase;
            obj.Translation = word.Translation;
            obj.State = word.State;
            obj.Notes = word.Notes;

            AddSentence(obj, language, word.Sentence, word.TextId);

            return UpdateWord(userId, obj);
        }

        public Word Save(int userId, WordTranslateUpdateDto word)
        {
            var obj = GetById(userId, word.Id);

            if (obj == null)
            {
                throw new NullReferenceException();
            }

            var language = _languageRepository.GetById(word.LanguageId);

            AddSentence(obj, language, word.Sentence, word.TextId);

            return UpdateWord(userId, obj);
        }

        private void AddSentence(Word word, Language language, string sentence, int textId)
        {
            if (word.Sentences == null)
            {
                word.Sentences = new List<Sentence>();
            }

            if (word.State != WordState.NotKnown)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(sentence))
            {
                return;
            }

            var allowMultitple = language.GetOption(LanguageOptions.MultipleSentences, true);

            if (!allowMultitple)
            {
                word.Sentences.Clear();
            }

            var text = _textRepository.GetById(textId);
            sentence = sentence.Trim();

            if (!word.Sentences.Any(w => w.Sntnce == sentence))
            {
                word.Sentences.Add(
                    new Sentence
                    {
                        Created = DateTime.UtcNow,
                        Sntnce = sentence ?? "",
                        Word = word,
                        Text = text
                    }
                );
            }
        }
    }
}
