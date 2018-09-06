using FluentNHibernate.Conventions.Inspections;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace FluentFramework.Conventions
{
    internal class ManyToManyTableNameConvention : FluentNHibernate.Conventions.ManyToManyTableNameConvention
    {
        private readonly PluralizationService pluralizer;
        public ManyToManyTableNameConvention()
            => pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-US"));

        protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
            => pluralizer.Pluralize(collection.EntityType.Name) + pluralizer.Pluralize(otherSide.EntityType.Name);

        protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
            => collection.EntityType.Name + pluralizer.Pluralize(collection.ChildType.Name);
    }
}
