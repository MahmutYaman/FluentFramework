 1. Create a class implements from `IConnectionDescriptive` for return
    connection string and provider.
 2. Create entities as inherits from `Entity<YourConnectionDescriptive>`
 3. Map your entity with `EntityMap<YourEntity, YourConnectionDescriptive>`
 4. Get your database connection with `Repository<YourConnectionDescriptive>.CreateRepository()`

Examine test project to see how it works.
