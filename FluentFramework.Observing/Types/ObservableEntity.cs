using FluentNHibernate.Data;
using System.ComponentModel;

namespace FluentFramework.Observing.Types
{
    public abstract class ObservableEntity<ConnectionConfigurer> : Entity, IObservableEntity, INotifyPropertyChanged where ConnectionConfigurer : IConnectionConfigurer, new()
    {
#pragma warning disable CS0067 // The event 'ObservableEntity<ConnectionConfigurer>.PropertyChanged' is never used
        public virtual event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067 // The event 'ObservableEntity<ConnectionConfigurer>.PropertyChanged' is never used

        public virtual void OnPreInsert<ObservableEntity>(ObservableRepository<ObservableEntity> observableRepository, out bool vetoed) where ObservableEntity : IObservableEntity, new()
            => vetoed = false;

        public virtual void OnPreDelete<ObservableEntity>(ObservableRepository<ObservableEntity> observableRepository, out bool vetoed) where ObservableEntity : IObservableEntity, new()
            => vetoed = false;

        public virtual void OnPreUpdate<ObservableEntity>(ObservableRepository<ObservableEntity> observableRepository, out bool vetoed) where ObservableEntity : IObservableEntity, new()
            => vetoed = false;
    }
}
