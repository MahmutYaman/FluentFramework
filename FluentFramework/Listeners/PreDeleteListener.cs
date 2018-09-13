using FluentFramework.Types;
using NHibernate.Event;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework.Listeners
{
    internal class PreDeleteListener<ConnectionDescriptive> : IPreDeleteEventListener where ConnectionDescriptive : IConnectionDescriptive
    {
        public bool OnPreDelete(PreDeleteEvent @event)
        {
            if (@event.Entity is Entity<ConnectionDescriptive> entity)
            {
                entity.OnPreDelete(Repository<ConnectionDescriptive>.CreateRepository(false, false), out bool vetoed);
                return vetoed;
            }

            return false;
        }

        public Task<bool> OnPreDeleteAsync(PreDeleteEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreDelete(@event), cancellationToken);
    }
}