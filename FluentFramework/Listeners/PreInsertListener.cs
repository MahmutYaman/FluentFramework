using FluentFramework.Types;
using NHibernate.Event;
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
                entity.OnPreUpdate(new Repository<ConnectionConfigurer>(@event.Session), out bool vetoed);
                return vetoed;
            }

            return false;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
            => new Task<bool>(() => OnPreInsert(@event), cancellationToken);
    }
}