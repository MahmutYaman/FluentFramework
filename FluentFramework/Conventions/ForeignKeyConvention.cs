using FluentNHibernate;
using System;

namespace FluentFramework.Database.Conventions
{
    internal class ForeignKeyConvention : FluentNHibernate.Conventions.ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
            => property == null ? type.Name + "_Id" : property.Name + "_Id";
    }
}