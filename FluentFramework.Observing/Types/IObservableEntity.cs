using System;
using System.Collections.Generic;
using System.Text;

namespace FluentFramework.Observing.Types
{
    public interface IObservableEntity
    {
        void OnPreUpdate<ObservableEntity>(ObservableRepository<ObservableEntity> observableRepository, out bool vetoed) where ObservableEntity : IObservableEntity, new();
        void OnPreInsert<ObservableEntity>(ObservableRepository<ObservableEntity> observableRepository, out bool vetoed) where ObservableEntity : IObservableEntity, new();
        void OnPreDelete<ObservableEntity>(ObservableRepository<ObservableEntity> observableRepository, out bool vetoed) where ObservableEntity : IObservableEntity, new();
    }
}
