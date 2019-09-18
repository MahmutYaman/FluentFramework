using FluentNHibernate.Data;
using System.ComponentModel;

namespace FluentFramework.Observing.Types
{
    public abstract class ObservableEntity<ConnectionConfigurer> : Entity, INotifyPropertyChanged where ConnectionConfigurer : IConnectionConfigurer, new()
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public static ObservableRepository<ObservableEntity<ConnectionConfigurer>, ConnectionConfigurer> ObservableRepository => ObservableRepository<ObservableEntity<ConnectionConfigurer>, ConnectionConfigurer>.Instance;

        public virtual void OnPreInsert<ObservableEntity, _ConnectionConfigurer>(ObservableRepository<ObservableEntity, _ConnectionConfigurer> observableRepository, out bool vetoed)
            where ObservableEntity : ObservableEntity<_ConnectionConfigurer>
            where _ConnectionConfigurer : IConnectionConfigurer, new()
            => vetoed = false;

        public virtual void OnPreDelete<ObservableEntity, _ConnectionConfigurer>(ObservableRepository<ObservableEntity, _ConnectionConfigurer> observableRepository, out bool vetoed)
            where ObservableEntity : ObservableEntity<_ConnectionConfigurer>
            where _ConnectionConfigurer : IConnectionConfigurer, new()
            => vetoed = false;

        public virtual void OnPreUpdate<ObservableEntity, _ConnectionConfigurer>(ObservableRepository<ObservableEntity, _ConnectionConfigurer> observableRepository, out bool vetoed)
            where ObservableEntity : ObservableEntity<_ConnectionConfigurer>
            where _ConnectionConfigurer : IConnectionConfigurer, new()
            => vetoed = false;

    }
}
