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
        Repository<DefaultConnection> repository;
        public Tests()
        {
            repository = Repository<DefaultConnection>.CreateRepository();
        }

        [TestMethod]
        public void Transactions()
        {
            var guid = Guid.NewGuid().ToString();
            repository.Transaction.Begin();

            var user = new User
            {
                Username = "User_" + guid,
                Password = CryptoHelper.HashPassword("123456")
            };
            user.Details.Add("Bio", "A simple person.");

            repository.Add(user);
            repository.Add(new Book { Name = "Book_" + guid, User = user });
            repository.SaveChanges();

            repository.Transaction.Rollback();

            var result = repository.Query<User>().Where(x => x.Username == "User_" + guid).SingleOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Query()
        {
            var guid = Guid.NewGuid().ToString();

            repository.Add(new User
            {
                Username = "User_" + guid,
                Password = CryptoHelper.HashPassword("123456")
            });

            var result = repository.Query<User>().SingleOrDefault(x => x.Username == "User_" + guid);
            Assert.IsNotNull(result);

            var passwordVerified = CryptoHelper.VerifyHashedPassword(result.Password, "123456");
            Assert.IsTrue(passwordVerified);
        }
    }
}
