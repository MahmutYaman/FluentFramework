using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pluralize.NET.Core;

namespace FluentFramework.Database.Conventions
{
    internal class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
            => instance.Table(new Pluralizer().Pluralize(instance.EntityType.Name));
    }
}
