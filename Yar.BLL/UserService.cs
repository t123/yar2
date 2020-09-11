using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Yar.Data;

namespace Yar.BLL
{
    public class UserService
    {
        private readonly GenericRepository<User> _repository;

        public UserService(ISession session)
        {
            _repository = new GenericRepository<User>(session);
        }

        public IEnumerable<User> Get()
        {
            return _repository.Get();
        }

        public User Get(int userId)
        {
            return _repository.Get().SingleOrDefault(x => x.Id == userId);
        }

        public User Get(string username)
        {
            username = username?.Trim()?.ToLowerInvariant();

            return _repository.Get().SingleOrDefault(x => x.Username.ToLowerInvariant() == username);
        }

        public User Login(string username)
        {
            return Get(username);
        }

        public void Save(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User obj;

            if (user.Id == 0)
            {
                if (Get(user.Username) != null)
                {
                    throw new Exception("Username taken");
                }

                obj = new User()
                {
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    LastLogin = null,
                    Username = user.Username.Trim()
                };

                _repository.Save(obj);
            }
            else
            {
                obj = Get(user.Id);

                if (obj == null)
                {
                    throw new Exception("User not found");
                }

                obj.Updated = DateTime.UtcNow;

                _repository.Save(obj);
            }
        }
    }
}