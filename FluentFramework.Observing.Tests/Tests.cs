using FluentFramework.Helpers;
using FluentFramework.Observing.Tests.Entities;
using FluentFramework.Observing.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FluentFramework.Observing.Tests
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
        public void TestMethod1()
        {
            ObservableRepository<Book>.Instance.Add(new Book
            {
                Name = "Book",
            });

            ObservableRepository<User>.Instance.Add(new User
            {
                Name = "User",
            });
        }
    }
}
