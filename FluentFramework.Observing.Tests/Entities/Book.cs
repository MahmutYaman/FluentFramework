using FluentFramework.Observing.Types;
using FluentFramework.Types;

namespace FluentFramework.Observing.Tests.Entities
{
    public class Book : ObservableEntity<DefaultConnection>
    {
        public virtual string Name { get; set; }
    }

    public class BookMap : EntityMap<Book, DefaultConnection>
    {
        public BookMap()
        {
            Map(x => x.Name).Not.Nullable();
        }
    }
}
