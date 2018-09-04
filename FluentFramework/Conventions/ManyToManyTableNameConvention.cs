using FluentNHibernate.Conventions.Inspections;
using Pluralize.NET.Core;

namespace FluentFramework.Conventions
{
    internal class ManyToManyTableNameConvention : FluentNHibernate.Conventions.ManyToManyTableNameConvention
    {
        private readonly Pluralizer pluralizer;
        public ManyToManyTableNameConvention()
            => pluralizer = new Pluralizer();

        protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
            => pluralizer.Pluralize(collection.EntityType.Name) + pluralizer.Pluralize(otherSide.EntityType.Name);

        protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
            => collection.EntityType.Name + pluralizer.Pluralize(collection.ChildType.Name);
    }
}
