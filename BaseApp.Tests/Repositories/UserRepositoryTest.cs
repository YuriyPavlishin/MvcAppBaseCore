using System;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.DataRepository.Users.Impl;
using BaseApp.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseApp.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void GetByEmailOrNull_Exists()
        {
            var repMock = RepositoryTestFactory.CreateMock<UserRepository>();

            var email = "test@example.com";
            repMock.DbData.Users.Add(new User() {Id = 1, FirstName = "Test", LastName = "Purpose", Email = email});
            repMock.DbData.SaveChanges();
            
            var usr = repMock.Rep.GetByEmailOrNull(email);

            Assert.AreEqual(email, usr.Email);
        }

        [TestMethod]
        public void GetByEmailOrNull_Exists_As_Deleted()
        {
            var repMock = RepositoryTestFactory.CreateMock<UserRepository>();

            var email = "test@example.com";
            repMock.DbData.Users.Add(new User() { Id = 1, FirstName = "Test", LastName = "Purpose", Email = email, DeletedDate = DateTime.Now});
            repMock.DbData.SaveChanges();

            var usr = repMock.Rep.GetByEmailOrNull(email);
            Assert.AreEqual(null, usr);
        }

        [TestMethod]
        public void GetByEmailOrNull_Exists_Not_Exists()
        {
            var repMock = RepositoryTestFactory.CreateMock<UserRepository>();
            var email = "test@example.com";

            var usr = repMock.Rep.GetByEmailOrNull(email);
            Assert.AreEqual(null, usr);
        }
    }
}
