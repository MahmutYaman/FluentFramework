using NHibernate;
using System.Data;

namespace FluentFramework.Observing
{
    public class ObservableTransaction
    {
        private readonly ISession _session;

        internal ObservableTransaction(ISession session)
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

        public void Rollback()
           => _session.Transaction.Rollback();
    }
}
