﻿using FluentFramework.Types;
using NHibernate;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework
{
    public class Repository<ConnectionConfigurer> : IDisposable where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        private readonly ISession _session;

        internal Repository(ISession session)
            => _session = session;

        public Repository()
        {
            var sessionFactory = ConnectionDescriptors.GetSessionFactory<ConnectionConfigurer>();
            if (sessionFactory is null)
                throw new ArgumentException("Settings for this connection is not defined. Use ConnectionDescriptors.Add().", "ConnectionConfigurer");

            _session = sessionFactory.OpenSession();
            _session.FlushMode = FlushMode.Manual;
        }

        private Transaction _transaction;
        public Transaction Transaction
            => _transaction ?? (_transaction = new Transaction(_session));

        public IQueryable<T> Query<T>() where T : Entity<ConnectionConfigurer>
            => _session.Query<T>();

        public T Get<T>(long id) where T : Entity<ConnectionConfigurer>
            => _session.Get<T>(id);

        public async Task<T> GetAsync<T>(long id, CancellationToken cancellationToken = default) where T : Entity<ConnectionConfigurer>
            => await _session.GetAsync<T>(id, cancellationToken);

        public long Add(Entity<ConnectionConfigurer> entity)
            => (long)_session.Save(entity);

        public void Update(Entity<ConnectionConfigurer> entity)
            => _session.Update(entity);

        public void Delete(Entity<ConnectionConfigurer> entity)
            => _session.Delete(entity);

        public void SaveChanges()
            => _session.Flush();

        #region Disposing
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_session != null)
                {
                    if (_session.IsOpen)
                        _session.Close();

                    _session.Dispose();
                }
            }
        }

        ~Repository() => Dispose(false);
        #endregion
    }
}