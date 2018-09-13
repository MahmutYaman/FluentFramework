using FluentNHibernate.Cfg;

namespace FluentFramework
{
    public interface IConnectionDescriptive
    {
        FluentConfiguration Configuration(FluentConfiguration cfg, bool useSecondLevelCache, bool useQueryCache, out bool useDefaultCachingMechanism, out bool autoCreateDatabase);
    }
}