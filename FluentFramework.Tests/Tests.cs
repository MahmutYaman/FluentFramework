using FluentFramework.Helpers;
using FluentFramework.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FluentFramework.Tests
{
    [TestClass]
    public class Tests
    {
        public Repository<DefaultConnection> CreateRepository()
        {
            return Repository<DefaultConnection>.CreateRepository();
        }

        [TestMethod]
        public void CreateEntities()
        {
            var repository = CreateRepository();
            repository.Transaction.Begin();

            var user = new User
            {
                Username = "User_" + Guid.NewGuid().ToString(),
                Password = CryptoHelper.HashPassword("123456")
            };
            user.Details.Add("Bio", "A simple person.");

            repository.Add(user);
            repository.Add(new Book { Name = "Book name", User = user });

            repository.Transaction.Commit();
            repository.SaveChanges();
        }

        [TestMethod]
        public void GetUser()
        {
            var repository = CreateRepository();
            var user = repository.Query<User>().ToList().Where(x => x.Details.ContainsKey("Bio")).First();
            Assert.IsNotNull(user, "No user found.");
        }

        [TestMethod]
        public void VerifyUserPassword()
        {
            var repository = CreateRepository();
            var verified = CryptoHelper.VerifyHashedPassword(repository.Query<User>().First().Password, "123456");
            Assert.IsTrue(verified, "Password does not match.");
        }
    }
}
