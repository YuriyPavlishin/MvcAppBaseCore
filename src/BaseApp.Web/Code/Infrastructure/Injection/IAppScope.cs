using System;

namespace BaseApp.Web.Code.Infrastructure.Injection;

public interface IAppScope : IDisposable
{
    T GetService<T>();
}