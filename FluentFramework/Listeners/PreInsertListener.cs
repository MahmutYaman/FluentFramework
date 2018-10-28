using FluentFramework.Types;
using NHibernate.Event;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework.Listeners
{
    internal class PreInsertListener<ConnectionDescriptive> : IPreInsertEventListener where ConnectionDescriptive : IConnectionDescriptive
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (@event.Entity is Entity<ConnectionDescriptive> entity)
            {
                entity.OnPreUpdate(new Repository<ConnectionDescriptive>(@event.Session), out bool vetoed);
                return vetoed;
            }

            return false;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreInsert(@event), cancellationToken);
    }
}