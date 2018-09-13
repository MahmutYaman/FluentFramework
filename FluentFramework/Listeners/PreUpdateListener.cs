using FluentFramework.Types;
using NHibernate.Event;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework.Listeners
{
    internal class PreUpdateListener<ConnectionDescriptive> : IPreUpdateEventListener where ConnectionDescriptive : IConnectionDescriptive
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (@event.Entity is Entity<ConnectionDescriptive> entity)
            {
                entity.OnPreUpdate(Repository<ConnectionDescriptive>.CreateRepository(false, false), out bool vetoed);
                return vetoed;
            }

            return false;
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreUpdate(@event), cancellationToken);
    }
}