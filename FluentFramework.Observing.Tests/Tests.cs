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
            var books = ObservableRepository<Book>.Instance;
            books.Add(new Book
            {
                Name = "Book",
            });

            var users = ObservableRepository<User>.Instance;
            users.CollectionChanged += Instance_CollectionChanged;
            users.Add(new User
            {
                Name = "User",
            });
            Assert.IsTrue(collectionChanged);

            users.ItemPropertyChanged += Instance_ItemPropertyChanged;

            users[0].Name = "Changed User";
            Assert.IsTrue(propertyChanged);
        }

        bool collectionChanged = false;
        private void Instance_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            collectionChanged = true;
        }

        bool propertyChanged = false;
        private void Instance_ItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            propertyChanged = true;
        }
    }
}
