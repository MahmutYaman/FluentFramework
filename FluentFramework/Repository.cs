using FluentFramework.Database.Conventions;
using FluentFramework.Listeners;
using FluentFramework.Types;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Context;
using NHibernate.Event;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework
{
    public class Repository<ConnectionDescriptive> where ConnectionDescriptive : IConnectionDescriptive
    {
        private static readonly Dictionary<string, ISessionFactory> _sessionFactories = new Dictionary<string, ISessionFactory>();
        private readonly ISession _session;
        internal Repository(ISession session)
            => _session = session;


        /// <summary>
        /// Creates a repository which is connects to the specified connection for an entity group.
        /// </summary>
        /// <typeparam name="ConnectionDescriptive">Database connection descriptive for an entity group.</typeparam>
        /// <param name="useSecondLevelCache">The second level cache is responsible for caching objects across sessions. When this is turned on, objects will first be searched in the cache and if they are not found, a database query will be fired.</param>
        /// <param name="useQueryCache">Query Cache is used to cache the results of a query. When the query cache is turned on, the results of the query are stored against the combination query and parameters. Every time the query is fired the cache manager  checks for the combination of parameters and query. If the results are found in the cache, they are returned, otherwise a database transaction is initiated.  As you can see, it is not a good idea to cache a query if it has a number of parameters, because then a single parameter can take a number of values. For each of these combinations the results are stored in the memory. This  can lead to extensive memory usage.</param>
        public static Repository<ConnectionDescriptive> CreateRepository(bool useSecondLevelCache = false, bool useQueryCache = false)
        {
            var connection = Activator.CreateInstance<ConnectionDescriptive>();
            var connectionString = connection.GetConnectionString();
            var provider = connection.GetProvider();

            _sessionFactories.TryGetValue(connectionString + provider + useSecondLevelCache + useQueryCache, out ISessionFactory sessionFactory);
            if (sessionFactory is null)
            {
                sessionFactory = Fluently.Configure().Database(() =>
                {
                    switch (provider)
                    {
                        case ConnectionProvider.MSSQL2012:
                            return MsSqlConfiguration.MsSql2012.ConnectionString(connectionString);
                        case ConnectionProvider.MSSQL2008:
                            return MsSqlConfiguration.MsSql2008.ConnectionString(connectionString);
                        case ConnectionProvider.SQLCE:
                            return MsSqlCeConfiguration.Standard.ConnectionString(connectionString);
                        case ConnectionProvider.SQLITE:
                            return SQLiteConfiguration.Standard.ConnectionString(connectionString);
                        case ConnectionProvider.MYSQL:
                            return MySQLConfiguration.Standard.ConnectionString(connectionString);
                        case ConnectionProvider.POSTGRESQL:
                            return PostgreSQLConfiguration.Standard.ConnectionString(connectionString);
                        default:
                            throw new ArgumentNullException(nameof(provider));
                    }
                })
                .Mappings(mapping =>
                {
                    mapping.FluentMappings.Conventions.AddFromAssemblyOf<TableNameConvention>();

                    var entityMapType = typeof(EntityMap<,>);
                    var connectionType = connection.GetType();

                    foreach (var entityMap in AppDomain.CurrentDomain.GetAssemblies()
                                                .Where(a => !a.GlobalAssemblyCache)
                                                .SelectMany(a => a.GetTypes()
                                                    .Where(t => t.BaseType?.IsGenericType == true && 
                                                                t.BaseType.GetGenericTypeDefinition() == entityMapType && 
                                                                t.BaseType.GenericTypeArguments.Any(arg => connectionType.IsAssignableFrom(arg)))
                                                    .Distinct()))
                        mapping.FluentMappings.Add(entityMap);
                })
                .ExposeConfiguration(cfg =>
                {
                    cfg.AppendListeners(ListenerType.PreInsert, new[] { new PreInsertListener<ConnectionDescriptive>() });
                    cfg.AppendListeners(ListenerType.PreUpdate, new[] { new PreUpdateListener<ConnectionDescriptive>() });
                    cfg.AppendListeners(ListenerType.PreDelete, new[] { new PreDeleteListener<ConnectionDescriptive>() });

                    try
                    {
                        new SchemaValidator(cfg).Validate();
                    }
                    catch
                    {
                        new SchemaUpdate(cfg).Execute(false, true);
                    }
                })
                .Cache(caching =>
                {
                    if (useSecondLevelCache)
                        caching.UseSecondLevelCache().UseMinimalPuts();
                    if (useQueryCache)
                        caching.UseQueryCache().QueryCacheFactory<StandardQueryCacheFactory>();
                })
                .CurrentSessionContext<ThreadLocalSessionContext>()
                .BuildSessionFactory();
                _sessionFactories.Add(connectionString + provider + useSecondLevelCache + useQueryCache, sessionFactory);
            }

            var session = sessionFactory.OpenSession();
            session.FlushMode = FlushMode.Manual;
            return new Repository<ConnectionDescriptive>(session);
        }


        private Transaction _transaction;
        public Transaction Transaction
            => _transaction ?? (_transaction = new Transaction(_session));


        public IQueryable<T> Query<T>() where T : Entity<ConnectionDescriptive>
            => _session.Query<T>();


        public T Get<T>(long id) where T : Entity<ConnectionDescriptive>
            => _session.Query<T>().SingleOrDefault(x => x.Id == id);

        public async Task<T> GetAsync<T>(long id, CancellationToken cancellationToken = default(CancellationToken)) where T : Entity<ConnectionDescriptive>
            => await _session.Query<T>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);


        public T Single<T>() where T : Entity<ConnectionDescriptive>
            => _session.Query<T>().SingleOrDefault();

        public async Task<T> SingleAsync<T>(CancellationToken cancellationToken = default(CancellationToken)) where T : Entity<ConnectionDescriptive>
            => await _session.Query<T>().SingleOrDefaultAsync(cancellationToken);


        public long Add(Entity<ConnectionDescriptive> entity)
            => (long)_session.Save(entity);

        public async Task<long> AddAsync(Entity<ConnectionDescriptive> entity, CancellationToken cancellationToken = default(CancellationToken))
            => (long)await _session.SaveAsync(entity, cancellationToken);


        public void Update(Entity<ConnectionDescriptive> entity)
            => _session.Update(entity);

        public async Task UpdateAsync(Entity<ConnectionDescriptive> entity, CancellationToken cancellationToken = default(CancellationToken))
            => await _session.UpdateAsync(entity, cancellationToken);


        public void Delete(Entity<ConnectionDescriptive> entity)
            => _session.Delete(entity);

        public async Task DeleteAsync(Entity<ConnectionDescriptive> entity, CancellationToken cancellationToken = default(CancellationToken))
            => await _session.DeleteAsync(entity, cancellationToken);


        public void SaveChanges()
            => _session.Flush();

        public async Task SaveChangesAsync()
            => await _session.FlushAsync();
    }
}
