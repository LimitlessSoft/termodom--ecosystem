using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers;

public class WebDbContextFactory : IWebDbContextFactory
{
    public T Create<T>() =>
        (T)Activator.CreateInstance(typeof(T),  new DbContextOptionsBuilder<WebDbContext>().Options)!;
}
