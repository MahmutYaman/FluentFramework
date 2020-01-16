using FluentFramework.Types;
using NHibernate.Event;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FluentFramework.Listeners
{
    internal class PreInsertListener<ConnectionConfigurer> : IPreInsertEventListener where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (@event.Entity is Entity<ConnectionConfigurer> entity)
            {
                var persist = entity.OnPreInsert();
                foreach (var propertyName in @event.Persister.PropertyNames)
                    @event.State[Array.IndexOf(@event.Persister.PropertyNames, propertyName)] = @event.Entity.GetType().GetRuntimeProperties().SingleOrDefault(p => p.Name == propertyName)?.GetValue(@event.Entity);
                return !persist;
            }
            return false;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreInsert(@event), cancellationToken);
    }
}