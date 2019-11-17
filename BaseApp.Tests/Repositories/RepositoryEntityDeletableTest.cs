using BaseApp.Data.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseApp.Tests.Repositories
{
    [TestClass]
    public class RepositoryEntityDeletableTest: UserRepositoryTestBase
    {
        [TestMethod]
        public void Get_NotExists()
        {
            var repMock = GetRepMockWithDefaultUser(true);
            Assert.ThrowsException<RecordNotFoundException>(() => repMock.Rep.Get(-1));
        }

        [TestMethod]
        public void Get_Deleted()
        {
            var repMock = GetRepMockWithDefaultUser(true);
            Assert.ThrowsException<RecordNotFoundException>(() => repMock.Rep.Get(DefaultUserId));
        }

        [TestMethod]
        public void GetOrNull_Deleted()
        {
            var repMock = GetRepMockWithDefaultUser(true);
            var usr = repMock.Rep.GetOrNull(DefaultUserId);

            Assert.AreEqual(null, usr);
        }

        [TestMethod]
        public void GetOrNull_NotDeleted()
        {
            var repMock = GetRepMockWithDefaultUser(false);
            var usr = repMock.Rep.GetOrNull(DefaultUserId);

            Assert.AreEqual(DefaultUserId, usr.Id);
        }

        [TestMethod]
        public void GetWithDeletedOrNull_Deleted()
        {
            var repMock = GetRepMockWithDefaultUser(true);
            var usr = repMock.Rep.GetWithDeletedOrNull(DefaultUserId);

            Assert.AreEqual(DefaultUserId, usr.Id);
        }

        [TestMethod]
        public void GetCustom()
        {
            var repMock = GetRepMockWithDefaultUser(false);
            var firstName = repMock.Rep.GetCustom(DefaultUserId, u => u.FirstName);

            Assert.AreEqual(DefaultFirstName, firstName);
        }

        [TestMethod]
        public void GetCustom_Deleted()
        {
            var repMock = GetRepMockWithDefaultUser(true);
            Assert.ThrowsException<RecordNotFoundException>(() => repMock.Rep.GetCustom(DefaultUserId, u => u.FirstName));
        }

        [TestMethod]
        public void GetCustomOrDefault_NotExists()
        {
            var repMock = GetRepMockWithDefaultUser(false);
            var firstName = repMock.Rep.GetCustomOrDefault(-1, u => u.FirstName);

            Assert.AreEqual(null, firstName);
        }

        [TestMethod]
        public void GetAll()
        {
            var repMock = GetRepMockWithDefaultUser(false);
            var users = repMock.Rep.GetAll();

            Assert.AreEqual(2, users.Count);
        }

        [TestMethod]
        public void GetAll_WithDeleted()
        {
            var repMock = GetRepMockWithDefaultUser(true);
            var users = repMock.Rep.GetAll();

            Assert.AreEqual(1, users.Count);
        }
    }
}
