using FluentFramework.Types;

namespace FluentFramework.Tests.Entities
{
    public class Book : Entity<DefaultConnection>
    {
        public virtual string Name { get; set; }
        public virtual User User { get; set; }
    }

    public class BookMap : EntityMap<Book, DefaultConnection>
    {
        public BookMap()
        {
            Map(x => x.Name).Not.Nullable();
            References(x => x.User).Not.Nullable();
        }
    }
}
