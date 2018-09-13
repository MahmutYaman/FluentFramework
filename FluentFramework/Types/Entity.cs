using FluentNHibernate.Mapping;

namespace FluentFramework.Types
{
    public abstract class Entity<ConnectionDescriptive> : FluentNHibernate.Data.Entity where ConnectionDescriptive : IConnectionDescriptive
    {
        public virtual void OnPreInsert(Repository<ConnectionDescriptive> repository, out bool vetoed)
            => vetoed = false;

        public virtual void OnPreUpdate(Repository<ConnectionDescriptive> repository, out bool vetoed)
            => vetoed = false;

        public virtual void OnPreDelete(Repository<ConnectionDescriptive> repository, out bool vetoed)
            => vetoed = false;
    }

    public abstract class EntityMap<T, ConnectionDescriptive> : ClassMap<T> where T : FluentNHibernate.Data.Entity where ConnectionDescriptive : IConnectionDescriptive
    {
        public EntityMap()
            => Id(x => x.Id).GeneratedBy.Native();
    }
}