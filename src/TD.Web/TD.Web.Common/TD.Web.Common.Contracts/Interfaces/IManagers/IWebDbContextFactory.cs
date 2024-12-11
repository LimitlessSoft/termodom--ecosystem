using Microsoft.EntityFrameworkCore;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IWebDbContextFactory
{
    T Create<T>();
}
