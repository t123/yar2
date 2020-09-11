using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Yar.Data
{
    public class GenericRepository<T> where T : class
    {
        private readonly ISession _session;

        public GenericRepository(ISession session)
        {
            _session = session;
        }

        public T GetById(object id)
        {
            return (T)_session.Get<T>(id);
        }
        public IQueryable<T> Get()
        {
            return _session.Query<T>();
        }

        public void Delete(T obj)
        {
            _session.Delete(obj);
        }

        public void Save(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _session.SaveOrUpdate(obj);
        }
    }
}
