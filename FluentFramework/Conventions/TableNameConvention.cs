using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace FluentFramework.Database.Conventions
{
    internal class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
            => instance.Table(PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-US")).Pluralize(instance.EntityType.Name));
    }
}