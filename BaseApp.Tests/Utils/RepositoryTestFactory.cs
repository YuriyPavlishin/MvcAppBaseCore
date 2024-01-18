using System;
using BaseApp.Data.DataContext;
using BaseApp.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Tests.Utils
{
    public static class RepositoryTestFactory
    {
        public static RepositoryMockModel<T> CreateMock<T>() where T : RepositoryBase
        {
            var options = new DbContextOptionsBuilder<DBData>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var dbData = new DBData(options);

            Type type = typeof(T);
            return new RepositoryMockModel<T>
                {
                    DbData = dbData,
                    Rep = (T)Activator.CreateInstance(type, new DataContextProvider(dbData))
                }; 
        }
    }

    public class RepositoryMockModel<T> where T:RepositoryBase
    {
        public T Rep { get; set; }
        public DBData DbData { get; set; }
    }
}
