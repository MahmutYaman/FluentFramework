using FluentFramework.Types;
using NHibernate.Event;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework.Listeners
{
    internal class PreUpdateListener<ConnectionConfigurer> : IPreUpdateEventListener where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (@event.Entity is Entity<ConnectionConfigurer> entity)
            {
                entity.OnPreUpdate(new Repository<ConnectionConfigurer>(@event.Session), out bool vetoed);
                return vetoed;
            }

            return false;
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreUpdate(@event), cancellationToken);
    }
}