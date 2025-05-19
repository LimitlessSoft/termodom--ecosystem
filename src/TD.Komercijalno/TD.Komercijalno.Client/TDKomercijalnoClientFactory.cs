namespace TD.Komercijalno.Client;

public interface ITDKomercijalnoClientFactory
{
	TDKomercijalnoClient Create(
		int year,
		TDKomercijalnoEnvironment environment,
		TDKomercijalnoFirma firma
	);
}

public class TDKomercijalnoClientFactory : ITDKomercijalnoClientFactory
{
	private readonly Dictionary<
		Tuple<int, TDKomercijalnoEnvironment, TDKomercijalnoFirma>,
		TDKomercijalnoClient
	> _clients = new();

	public TDKomercijalnoClient Create(
		int year,
		TDKomercijalnoEnvironment environment,
		TDKomercijalnoFirma firma
	)
	{
		var key = new Tuple<int, TDKomercijalnoEnvironment, TDKomercijalnoFirma>(
			year,
			environment,
			firma
		);
		if (!_clients.TryGetValue(key, out var client))
		{
			client = new TDKomercijalnoClient(year, environment, firma);
			_clients.Add(key, client);
		}

		return client;
	}
}
