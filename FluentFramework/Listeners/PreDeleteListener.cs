using FluentFramework.Types;
using NHibernate.Event;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework.Listeners
{
    internal class PreDeleteListener<ConnectionConfigurer> : IPreDeleteEventListener where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        public bool OnPreDelete(PreDeleteEvent @event)
            => @event.Entity is Entity<ConnectionConfigurer> entity ? !entity.OnPreDelete() : false;

        public Task<bool> OnPreDeleteAsync(PreDeleteEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreDelete(@event), cancellationToken);
    }
}