using FluentNHibernate.Cfg.Db;

namespace FluentFramework.Tests
{
    public class DefaultConnection : IConnectionConfigurer
    {
        public IPersistenceConfigurer Configuration()
        {
            return SQLiteConfiguration.Standard.ConnectionString("Data Source=Database.db;Version=3;");
        }
    }
}
