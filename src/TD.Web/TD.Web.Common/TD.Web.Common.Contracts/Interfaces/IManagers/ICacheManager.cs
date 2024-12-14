namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface ICacheManager
{
    Task<T> GetDataAsync<T>(string key, Func<T> getData, TimeSpan absoluteExpirationInterval);
}
