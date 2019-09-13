using FluentFramework.Database.Conventions;
using FluentFramework.Listeners;
using FluentFramework.Types;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentFramework
{
    public static class ConnectionDescriptors
    {
        private static readonly Dictionary<string, ISessionFactory> _sessionFactories = new Dictionary<string, ISessionFactory>();

        internal static ISessionFactory GetSessionFactory<ConnectionConfigurer>()
            => _sessionFactories.Where(x => x.Key == typeof(ConnectionConfigurer).FullName).Select(x => x.Value).SingleOrDefault();

        public static bool Exists<ConnectionConfigurer>() where ConnectionConfigurer : IConnectionConfigurer, new()
            => _sessionFactories.ContainsKey(typeof(ConnectionConfigurer).FullName);

        public static void Add<ConnectionConfigurer>(bool autoCreateDatabase, bool useSecondLevelCache, bool useQueryCache) where ConnectionConfigurer : IConnectionConfigurer, new()
        {
            if (Exists<ConnectionConfigurer>())
                throw new ArgumentException("Already exists.", "ConnectionConfigurer");

            var connection = Activator.CreateInstance<ConnectionConfigurer>();

            var sessionFactory = Fluently.Configure()
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
                    cfg.AppendListeners(ListenerType.PreInsert, new[] { new PreInsertListener<ConnectionConfigurer>() });
                    cfg.AppendListeners(ListenerType.PreUpdate, new[] { new PreUpdateListener<ConnectionConfigurer>() });
                    cfg.AppendListeners(ListenerType.PreDelete, new[] { new PreDeleteListener<ConnectionConfigurer>() });

                    if (autoCreateDatabase)
                    {
                        try
                        {
                            new SchemaValidator(cfg).Validate();
                        }
                        catch
                        {
                            new SchemaUpdate(cfg).Execute(false, true);
                        }
                    }
                })
                .Cache(caching =>
                {
                    if (useSecondLevelCache)
                        caching.UseSecondLevelCache().UseMinimalPuts();
                    if (useQueryCache)
                        caching.UseQueryCache().QueryCacheFactory<StandardQueryCacheFactory>();
                }).Database(connection.Configuration).BuildSessionFactory();

            _sessionFactories.Add(connection.GetType().FullName, sessionFactory);
        }

        public static void Remove<ConnectionConfigurer>() where ConnectionConfigurer : IConnectionConfigurer, new()
        {
            var connectionTypeName = typeof(ConnectionConfigurer).FullName;
            if (_sessionFactories.ContainsKey(connectionTypeName))
                _sessionFactories.Remove(connectionTypeName);
        }
    }
}
