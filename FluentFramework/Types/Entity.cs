using FluentNHibernate.Mapping;

namespace FluentFramework.Types
{
    public abstract class Entity<ConnectionConfigurer> : FluentNHibernate.Data.Entity where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        public virtual void OnPreInsert(Repository<ConnectionConfigurer> repository, out bool vetoed)
            => vetoed = false;

        public virtual void OnPreUpdate(Repository<ConnectionConfigurer> repository, out bool vetoed)
            => vetoed = false;

        public virtual void OnPreDelete(Repository<ConnectionConfigurer> repository, out bool vetoed)
            => vetoed = false;
    }

    public abstract class EntityMap<T, ConnectionConfigurer> : ClassMap<T> where T : FluentNHibernate.Data.Entity where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        public EntityMap()
            => Id(x => x.Id).GeneratedBy.Native();
    }
}