namespace FluentFramework.Tests
{
    public class DefaultConnection : IConnectionDescriptive
    {
        public string GetConnectionString()
        {
            return "Data Source=Database.db; Version=3;";
        }

        public ConnectionProvider GetProvider()
        {
            return ConnectionProvider.SQLITE;
        }
    }
}
