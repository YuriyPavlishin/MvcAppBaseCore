using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseApp.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTest: UserRepositoryTestBase
    {
        [TestMethod]
        public void GetByEmailOrNull_Exists()
        {
            var repMock = GetRepMockWithDefaultUser(false);
            var usr = repMock.Rep.GetByEmailOrNull(DefaultEmail);

            Assert.AreEqual(DefaultEmail, usr.Email);
        }

        [TestMethod]
        public void GetByEmailOrNull_IncludeDeleted_Exists()
        {
            var repMock = GetRepMockWithDefaultUser(true);
            var usr = repMock.Rep.GetByEmailOrNull(DefaultEmail, true);

            Assert.AreEqual(DefaultEmail, usr.Email);
        }

        [TestMethod]
        public void GetByEmailOrNull_Exists_As_Deleted()
        {
            var repMock = GetRepMockWithDefaultUser(true);
            var usr = repMock.Rep.GetByEmailOrNull(DefaultEmail);

            Assert.AreEqual(null, usr);
        }

        [TestMethod]
        public void GetByEmailOrNull_Not_Exists()
        {
            var repMock = GetRepMockWithDefaultUser(false);
            var usr = repMock.Rep.GetByEmailOrNull(DefaultEmail+"notexists.com");

            Assert.AreEqual(null, usr);
        }
    }
}
