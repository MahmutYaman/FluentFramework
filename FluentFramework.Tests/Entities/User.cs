using FluentFramework.Types;
using System.Collections.Generic;

namespace FluentFramework.Tests.Entities
{
    public class User : Entity<DefaultConnection>
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual Dictionary<string, string> Details { get; set; } = new Dictionary<string, string>();
        public virtual ISet<Book> Books { get; set; }
    }

    public class UserMap : EntityMap<User, DefaultConnection>
    {
        public UserMap()
        {
            Map(x => x.Username).Not.Nullable().Unique().Index("idx_username");
            Map(x => x.Password).Not.Nullable();
            Map(x => x.Details).CustomType<Serialized<Dictionary<string, string>>>().Nullable();
            HasMany(x => x.Books).Cascade.All();
        }
    }
}
