namespace TD.Office.Public.Contracts.Interfaces.IManagers;
public interface ICacheManager
{
    /// <summary>
    /// Retrieves data from the cache or executes a provided function to fetch and store it.
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve or store.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="getData">The function to fetch the data if not found in the cache.</param>
    /// <param name="absoluteExpirationInterval">The time interval after which the cache expires.</param>
    /// <returns>The retrieved or newly fetched data.</returns>
    Task<T> GetOrUpdateDataAsync<T>(string key, Func<T> getData, TimeSpan absoluteExpirationInterval);

    /// <summary>
    /// Stores data in the cache with an absolute expiration interval.
    /// </summary>
    /// <typeparam name="T">The type of data to store.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="getData">The data to store in the cache.</param>
    /// <param name="absoluteExpirationInterval">The time interval after which the cache expires.</param>
    Task SetDataAsync<T>(string key, Func<T> getData, TimeSpan absoluteExpirationInterval);

    /// <summary>
    /// Retrieves data from the cache.
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <returns>The data retrieved from the cache, or default if not found.</returns>
    Task<T> GetDataAsync<T>(string key);
}
