using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Yar.BLL.Dto;
using Yar.Data;

namespace Yar.BLL
{
    public class OptionService
    {
        private readonly GenericRepository<Option> _repository;

        public OptionService(ISession session)
        {
            _repository = new GenericRepository<Option>(session);
        }

        public Option Get(string key)
        {
            key = key?.Trim()?.ToLowerInvariant();

            return _repository.Get().SingleOrDefault(x => x.Key.ToLowerInvariant() == key);
        }

        public IEnumerable<Option> Get()
        {
            return _repository.Get().Where(x => x.Language == null);
        }

        public void Save(PostOptionDto option)
        {
            if (option == null)
            {
                throw new ArgumentException(nameof(option));
            }

            var o = Get(option.Key);

            if (o == null)
            {
                o = new Option
                {
                    Key = option.Key
                };
            }

            o.Value = option.Value;
            o.ExpectedType = option.ExpectedType;

            _repository.Save(o);
        }
    }
}
