using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace FluentFramework.Tests
{
    public class DefaultConnection : IConnectionDescriptive
    {
        public FluentConfiguration Configuration(FluentConfiguration cfg, bool useSecondLevelCache, bool useQueryCache, out bool useDefaultCachingMechanism, out bool autoCreateDatabase)
        {
            autoCreateDatabase = true;
            useDefaultCachingMechanism = true;
            return cfg.Database(SQLiteConfiguration.Standard.ConnectionString("Data Source=Database.db;Version=3;"));
        }
    }
}
