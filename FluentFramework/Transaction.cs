using NHibernate;
using System.Data;
using System.Threading.Tasks;

namespace FluentFramework
{
    public class Transaction
    {
        private readonly ISession _session;

        internal Transaction(ISession session)
            => _session = session;

        public bool IsActive => _session.Transaction.IsActive;
        public bool WasCommited => _session.Transaction.WasCommitted;
        public bool WasRolledBack => _session.Transaction.WasRolledBack;

        public void Begin()
            => _session.Transaction.Begin();

        public void Begin(IsolationLevel isolationLevel)
            => _session.Transaction.Begin(isolationLevel);

        public void Commit()
            => _session.Transaction.Commit();

        public async Task CommitAsync()
            => await _session.Transaction.CommitAsync();

        public void Rollback()
           => _session.Transaction.Rollback();

        public async Task RollbackAsync()
            => await _session.Transaction.RollbackAsync();
    }
}