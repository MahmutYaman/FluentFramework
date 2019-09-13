using FluentFramework.Types;

namespace FluentFramework.Tests.Entities
{
    public class Book : Entity<DefaultConnection>
    {
        public virtual string Name { get; set; }
        public virtual User User { get; set; }

        public override void OnPreInsert(Repository<DefaultConnection> repository, out bool vetoed)
        {
            base.OnPreInsert(repository, out vetoed);

            if (Name == "maybe a banned word check?")
            {
                vetoed = true;
            }
        }

        public override void OnPreUpdate(Repository<DefaultConnection> repository, out bool vetoed)
        {
            base.OnPreUpdate(repository, out vetoed);
        }

        public override void OnPreDelete(Repository<DefaultConnection> repository, out bool vetoed)
        {
            base.OnPreDelete(repository, out vetoed);
        }
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
