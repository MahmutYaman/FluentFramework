using FluentFramework.Helpers;
using FluentFramework.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace FluentFramework.Tests
{
    [TestClass]
    public class Tests
    {
        public Tests()
        {
            if (!ConnectionDescriptors.Exists<DefaultConnection>())
            {
                ConnectionDescriptors.Add<DefaultConnection>(true, false, false);
            }
        }

        [TestMethod]
        public void Transactions()
        {
            using (var repository = new Repository<DefaultConnection>())
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
        }

        [TestMethod]
        public void Query()
        {
            using (var repository = new Repository<DefaultConnection>())
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

        [TestMethod]
        public void MultiThread()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var thread1 = new Thread(() =>
            {
                try
                {
                    while (stopWatch.Elapsed < TimeSpan.FromSeconds(5))
                    {
                        Query();
                        Transactions();
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            });

            var thread2 = new Thread(() =>
            {
                try
                {
                    while (stopWatch.Elapsed < TimeSpan.FromSeconds(5))
                    {
                        Query();
                        Transactions();
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }
    }
}
