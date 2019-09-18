using FluentFramework.Observing.Types;
using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace FluentFramework.Observing
{
    public sealed class ObservableRepository<ObservableEntity> : ObservableCollection<ObservableEntity>, IDisposable where ObservableEntity : IObservableEntity, new()
    {
        private static readonly Lazy<ObservableRepository<ObservableEntity>> _instance = new Lazy<ObservableRepository<ObservableEntity>>(() => new ObservableRepository<ObservableEntity>());
        public static ObservableRepository<ObservableEntity> Instance => _instance.Value;


        private readonly ISession _session;
        private ObservableRepository()
        {
            var connectionConfigurerInterface = typeof(IConnectionConfigurer);
            var connectionConfigurer = typeof(ObservableEntity).BaseType.GenericTypeArguments.Where(x => connectionConfigurerInterface.IsAssignableFrom(x)).First();
            var sessionFactory = (ISessionFactory)typeof(ConnectionDescriptors).GetMethod("GetSessionFactory", BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(connectionConfigurer).Invoke(null, null);
            if (sessionFactory is null)
                throw new ArgumentException("Settings for this connection is not defined. Use ConnectionDescriptors.Add().", "ConnectionConfigurer");

            _session = sessionFactory.OpenSession();
            _session.FlushMode = FlushMode.Manual;
            Transaction = new ObservableTransaction(_session);

            var propertyChangedMethodInfo = GetType().GetMethod("OnItemPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var item in _session.Query<ObservableEntity>())
            {
                var eventInfo = item.GetType().GetEvent("PropertyChanged");
                eventInfo.AddEventHandler(item, Delegate.CreateDelegate(eventInfo.EventHandlerType, this, propertyChangedMethodInfo));
                Add(item);
            }
        }

        /// <summary>
        /// If you begin a transaction nothing changes on database until you commit or rollback.
        /// </summary>
        public ObservableTransaction Transaction { get; }

        public event PropertyChangedEventHandler ItemPropertyChanged;

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((ObservableEntity)sender).OnPreUpdate(this, out bool vetoed);
            if (!vetoed)
            {
                _session.Update(sender);

                if (!Transaction.IsActive)
                    _session.Flush();
            }

            ItemPropertyChanged?.Invoke(sender, e);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ObservableEntity item in e.NewItems)
                    {
                        item.OnPreInsert(this, out bool vetoed);
                        if (!vetoed)
                            _session.Save(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ObservableEntity item in e.OldItems)
                    {
                        item.OnPreDelete(this, out bool vetoed);
                        if (!vetoed)
                            _session.Delete(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (ObservableEntity item in _session.Query<ObservableEntity>())
                    {
                        item.OnPreDelete(this, out bool vetoed);
                        if (!vetoed)
                            _session.Delete(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (ObservableEntity item in e.NewItems)
                    {
                        item.OnPreUpdate(this, out bool vetoed);
                        if (!vetoed)
                            _session.Update(item);
                    }
                    break;
                default:
                    break;
            }

            if (!Transaction.IsActive)
                _session.Flush();

            base.OnCollectionChanged(e);
        }

        #region Disposing
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_session != null)
                {
                    if (_session.IsOpen)
                        _session.Close();

                    _session.Dispose();
                }
            }
        }

        ~ObservableRepository() => Dispose(false);
        #endregion
    }
}
