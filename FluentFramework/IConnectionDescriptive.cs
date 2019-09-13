using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace FluentFramework
{
    public interface IConnectionConfigurer
    {
        IPersistenceConfigurer Configuration();
    }
}