using FluentFramework.Types;
using NHibernate.Event;
using System;
using System.Linq;
using System.Reflection;
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
                var persist = entity.OnPreUpdate();
                foreach (var propertyName in @event.Persister.PropertyNames)
                    @event.State[Array.IndexOf(@event.Persister.PropertyNames, propertyName)] = @event.Entity.GetType().GetRuntimeProperties().SingleOrDefault(p => p.Name == propertyName)?.GetValue(@event.Entity);
                return !persist;
            }
            return false;
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreUpdate(@event), cancellationToken);
    }
}