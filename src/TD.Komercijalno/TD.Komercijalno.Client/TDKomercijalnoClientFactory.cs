namespace TD.Komercijalno.Client;

public interface ITDKomercijalnoClientFactory;

public class TDKomercijalnoClientFactory : ITDKomercijalnoClientFactory
{
    private readonly Dictionary<Tuple<int, TDKomercijalnoEnvironment>, TDKomercijalnoClient> _clients = new();

    public TDKomercijalnoClient Create(int year, TDKomercijalnoEnvironment environment)
    {
        var key = new Tuple<int, TDKomercijalnoEnvironment>(year, environment);
        if (!_clients.TryGetValue(key, out var client))
        {
            client = new TDKomercijalnoClient(year, environment);
            _clients.Add(key, client);
        }

        return client;
    }
}