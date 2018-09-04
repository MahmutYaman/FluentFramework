namespace FluentFramework
{
    public interface IConnectionDescriptive
    {
        string GetConnectionString();
        ConnectionProvider GetProvider();
    }

    public enum ConnectionProvider : int
    {
        MSSQL2012 = 0,
        MSSQL2008 = 1,
        SQLCE = 2,
        SQLITE = 3,
        MYSQL = 4,
        POSTGRESQL
    }
}
