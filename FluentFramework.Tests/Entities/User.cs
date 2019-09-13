using FluentFramework.Types;
using Iesi.Collections.Generic;
using System;
using System.Collections.Generic;

namespace FluentFramework.Tests.Entities
{
    public class User : Entity<DefaultConnection>
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual Dictionary<string, string> Details { get; set; } = new Dictionary<string, string>();
        public virtual ISet<Book> Books { get; set; } = new LinkedHashSet<Book>();

        public virtual DateTime CreatedOn { get; set; }
        public virtual bool SoftDeleted { get; set; }

        public override void OnPreInsert(Repository<DefaultConnection> repository, out bool vetoed)
        {
            CreatedOn = DateTime.UtcNow;
            vetoed = false;
        }

        public override void OnPreDelete(Repository<DefaultConnection> repository, out bool vetoed)
        {
            SoftDeleted = true;
            vetoed = true;
        }
    }

    public class UserMap : EntityMap<User, DefaultConnection>
    {
        public UserMap()
        {
            Where("SoftDeleted=0");
            Map(x => x.SoftDeleted).Nullable();
            Map(x => x.Username).Not.Nullable().Unique().Index("idx_username");
            Map(x => x.Password).Not.Nullable();
            Map(x => x.Details).CustomType<Serialized<Dictionary<string, string>>>().Nullable();
            HasMany(x => x.Books).Cascade.All();
        }
    }
}
