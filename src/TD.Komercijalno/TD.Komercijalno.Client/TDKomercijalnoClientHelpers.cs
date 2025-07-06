namespace TD.Komercijalno.Client;

public static class TDKomercijalnoClientHelpers
{
	public static TDKomercijalnoEnvironment ParseEnvironment(string environment) =>
		environment.ToLower() switch
		{
			"production" => TDKomercijalnoEnvironment.Production,
			"develop" => TDKomercijalnoEnvironment.Development,
			"automation" => TDKomercijalnoEnvironment.Automation,
			_ => throw new ArgumentException($"Invalid environment: {environment}")
		};
}
