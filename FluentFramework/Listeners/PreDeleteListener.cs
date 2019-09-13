using FluentFramework.Types;
using NHibernate.Event;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework.Listeners
{
    internal class PreDeleteListener<ConnectionConfigurer> : IPreDeleteEventListener where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        public bool OnPreDelete(PreDeleteEvent @event)
        {
            if (@event.Entity is Entity<ConnectionConfigurer> entity)
            {
                entity.OnPreDelete(new Repository<ConnectionConfigurer>(@event.Session), out bool vetoed);
                return vetoed;
            }

            return false;
        }

        public Task<bool> OnPreDeleteAsync(PreDeleteEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreDelete(@event), cancellationToken);
    }
}