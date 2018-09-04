using FluentNHibernate.Data;
using NHibernate.Proxy;

namespace FluentFramework.Extensions
{
    public static class TypeExtenions
    {
        public static T Unproxy<T>(this T entity) where T : Entity
            => entity.IsProxy()
                ? (T)((INHibernateProxy)entity).HibernateLazyInitializer.GetImplementation()
                : entity;

        public static bool IsProxy(this Entity entity)
            => entity is INHibernateProxy;
    }
}