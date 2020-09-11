using NHibernate;
using System;
using Yar.Data;
using Microsoft.Extensions.Logging;

namespace Yar.BLL
{
    public interface IUnitOfWork : IDisposable
    {
        TextService TextService { get; }
        LanguageService LanguageService { get; }
        WordService WordService { get; }
        UserService UserService { get; }
        OptionService OptionService { get; }
        IParserService ParserService { get; }
        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory _sessionFactory;

        static UnitOfWork()
        {
            _sessionFactory = SessionFactoryBuilder.BuildSessionFactory(AppSettings.Current.YarDatabase, false, true);
        }

        protected bool _disposed = false;
        protected bool _autoSaveChanges;
        protected ISession _session;
        protected ITransaction _transaction;

        private Lazy<TextService> _textService;
        private Lazy<LanguageService> _languageService;
        private Lazy<WordService> _wordService;
        private Lazy<UserService> _userService;
        private Lazy<OptionService> _optionService;
        private Lazy<IParserService> _parserService;

        public TextService TextService => _textService.Value;
        public LanguageService LanguageService => _languageService.Value;
        public WordService WordService => _wordService.Value;
        public UserService UserService => _userService.Value;
        public OptionService OptionService => _optionService.Value;
        public IParserService ParserService => _parserService.Value;

        public UnitOfWork(ILogger<UnitOfWork> logger, bool autoSaveChanges = true)
        {
            _autoSaveChanges = autoSaveChanges;
            _session = _sessionFactory.OpenSession();
            _transaction = _session.BeginTransaction(System.Data.IsolationLevel.Serializable);

            _textService = new Lazy<TextService>(() => new TextService(_session));
            _languageService = new Lazy<LanguageService>(() => new LanguageService(_session));
            _wordService = new Lazy<WordService>(() => new WordService(_session));
            _userService = new Lazy<UserService>(() => new UserService(_session));
            _optionService = new Lazy<OptionService>(() => new OptionService(_session));
            _parserService = new Lazy<IParserService>(() => new ParserService(new TextParserHelper()));
        }

        public void SaveChanges()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction = _session.BeginTransaction();
                }
            }
            catch (Exception e)
            {
                _transaction.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_autoSaveChanges)
                {
                    SaveChanges();
                }

                if (_session != null)
                {
                    _session.Close();
                }
            }

            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
