## Implementation
 1. Create a class implements from `IConnectionDescriptive` and configure your connection.
 2. Create entities as inherits from `Entity<YourConnectionDescriptive>`.
 3. Map your entity with `EntityMap<YourEntity, YourConnectionDescriptive>`.
 4. Get your database connection with `Repository<YourConnectionDescriptive>.CreateRepository()`.

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
