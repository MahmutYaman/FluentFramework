using FluentNHibernate.Mapping;

namespace FluentFramework.Types
{
    public abstract class Entity<ConnectionConfigurer> : FluentNHibernate.Data.Entity where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        /// <summary>
        /// You can do something before you insert. Return value will decide is it persist.
        /// </summary>
        /// <returns>Is persistent</returns>
        public virtual bool OnPreInsert()
            => true;

        /// <summary>
        /// You can do something before you update. Return value will decide is it persist.
        /// </summary>
        /// <returns>Is persistent</returns>
        public virtual bool OnPreUpdate()
            => true;

        /// <summary>
        /// You can do something before you delete. Return value will decide is it persist.
        /// </summary>
        /// <returns>Is persistent</returns>
        public virtual bool OnPreDelete()
            => true;
    }

    public abstract class EntityMap<T, ConnectionConfigurer> : ClassMap<T> where T : FluentNHibernate.Data.Entity where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        public EntityMap()
            => Id(x => x.Id).GeneratedBy.Native();
    }
}