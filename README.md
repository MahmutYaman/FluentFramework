## Implementation
 1. Create an implementation as like below and configure your connection.
```
public class DefaultConnection : IConnectionDescriptive
{
    public FluentConfiguration Configuration(FluentConfiguration cfg, bool useSecondLevelCache, bool useQueryCache, out bool useDefaultCachingMechanism, out bool autoCreateDatabase)
    {
        autoCreateDatabase = true;
        useDefaultCachingMechanism = true;
        return cfg.Database(SQLiteConfiguration.Standard.ConnectionString("Data Source=Database.db;Version=3;"));
    }
}
```
 2. Create entities as like below;
```
public class Book : Entity<DefaultConnection>
{
    public virtual string Name { get; set; }
    public virtual User User { get; set; }
}
```
 3. Map your entity as like below;
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
 4. Get your database connection as like below;
```
public User GetUser(string username)
{
    var repository = Repository<DefaultConnection>.CreateRepository();
    return repository.Query<User>().Where(x => x.Username == username).SingleOrDefault()
}
```
 5. You can also use transactions easy like below;
```
public void AddBook(string bookName, User user)
{
    var repository = Repository<DefaultConnection>.CreateRepository();
    repository.Transaction.Begin();
    var book = new Book { Name = bookName, User = user };
    repository.Add(book);
    repository.SaveChanges();
    
    if(!doSomethingWithBook(book))
    {
        repository.Transaction.Rollback();
    }
}
```

Examine test project to see how it works.

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
