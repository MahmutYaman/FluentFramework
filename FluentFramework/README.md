## Implementation
 1. Create an implementation as like below and configure your connection. You can use different database connections instead of SQLite of course! Others are in FluentNHibernate.Cfg.Db namespace.
```
public class DefaultConnection : IConnectionConfigurer
{
    public IPersistenceConfigurer Configuration()
    {
        return SQLiteConfiguration.Standard.ConnectionString("Data Source=Database.db;Version=3;");
    }
}
```
 2. Adding configuration to service example.
```
public partial class App : Application
{
    public App()
    {
        ConnectionDescriptors.Add<DefaultConnection>(true, false, false);
    }
}
```
 3. Entity example;
```
public class Book : Entity<DefaultConnection>
{
    public virtual string Name { get; set; }
    public virtual User User { get; set; }

    public override void OnPreInsert(Repository<DefaultConnection> repository, out bool vetoed)
    {
        base.OnPreInsert(repository, out vetoed);

        if (Name == "maybe a banned word check?")
        {
            vetoed = true;
        }
    }

    public override void OnPreUpdate(Repository<DefaultConnection> repository, out bool vetoed)
    {
        base.OnPreUpdate(repository, out vetoed);
    }

    public override void OnPreDelete(Repository<DefaultConnection> repository, out bool vetoed)
    {
        base.OnPreDelete(repository, out vetoed);
    }
}
```
 4. Mapping example;
```
public class BookMap : EntityMap<Book, DefaultConnection>
{
    public BookMap()
    {
        Map(x => x.Name).Not.Nullable();
        References(x => x.User).Not.Nullable();
    }
}
```
 5. Query example;
```
public User GetUser(string username)
{
    using (var repository = new Repository<DefaultConnection>())
    {
        return repository.Query<User>().Where(x => x.Username == username).SingleOrDefault();
    }
}
```
 6. Transaction example;
```
public void AddBook(string bookName, User user)
{
    using (var repository = new Repository<DefaultConnection>())
    {
        repository.Transaction.Begin();
        var book = new Book { Name = bookName, User = user };
        repository.Add(book);
        repository.SaveChanges();
    
        if(!doSomethingWithBook(book))
        {
            repository.Transaction.Rollback();
        }
    }
}
```

Examine the project to see what you can do more.

## Features
- Caching
- Multiple ways of ID generation such as Identity, Sequence, HiLo, GUID, Pooled, and the Native mechanism used in databases. Uses Native by default.
- Lazy Loading
- Generation and updating system like migration API in Entity framework.
- Cryptography helper for hashing and verifying passwords.
- JSON entity serialization
-  Multiple database connections
 - Multiple database providers
   - SQL Server
   - SQL Server Azure
   - Oracle
   - PostgreSQL
   - MySQL
   - SQLite
   - DB2
   - Sybase Adaptive Server
   - Firebird
   - Informix
   - It can even support the use of OLE DB (Object Linking and Embedding) and ODBC (Open Database Connectivity). 
