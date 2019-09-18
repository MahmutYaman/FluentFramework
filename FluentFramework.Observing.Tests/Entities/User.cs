using FluentFramework.Observing.Types;
using FluentFramework.Types;

namespace FluentFramework.Observing.Tests.Entities
{
    public class User : ObservableEntity<DefaultConnection>
    {
        public virtual string Name { get; set; }
    }

    public class UserMap : EntityMap<User, DefaultConnection>
    {
        public UserMap()
        {
            Map(x => x.Name).Not.Nullable();
        }
    }
}
