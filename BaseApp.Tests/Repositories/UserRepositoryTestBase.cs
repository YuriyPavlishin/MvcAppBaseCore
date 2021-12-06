using System;
using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.DataRepository.Users.Impl;
using BaseApp.Tests.Utils;

namespace BaseApp.Tests.Repositories
{
    public abstract class UserRepositoryTestBase
    {
        protected const int DefaultUserId = 1;
        protected const string DefaultFirstName = "FirstNameTest";
        protected const string DefaultEmail = "test@example.com";        

        protected const int Default2UserId = 2;
        protected const string Default2FirstName = "FirstNameTest2";
        protected const string Default2Email = "test2@example.com";

        protected const string DefaultPasswordForAllUsers = "fortestpurpose";

        protected static RepositoryMockModel<UserRepository> GetRepMockWithDefaultUser(bool firstAsDeleted)
        {
            var repMock = RepositoryTestFactory.CreateMock<UserRepository>();

            repMock.DbData.Users.Add(
                new User
                {
                    Id = DefaultUserId,
                    FirstName = DefaultFirstName,
                    LastName = DefaultFirstName + "LastName",
                    Email = DefaultEmail,
                    Login = DefaultEmail,
                    Password = DefaultPasswordForAllUsers,
                    DeletedDate = firstAsDeleted ? DateTime.Now : (DateTime?)null
                });
            repMock.DbData.Users.Add(
                new User
                {
                    Id = Default2UserId,
                    FirstName = Default2FirstName,
                    LastName = Default2FirstName + "LastName",
                    Email = Default2Email,
                    Login = Default2Email,
                    Password = DefaultPasswordForAllUsers
                });
            repMock.DbData.SaveChanges();
            return repMock;
        }
    }
}
