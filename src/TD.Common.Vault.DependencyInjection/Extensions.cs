using Microsoft.Extensions.Configuration;

namespace TD.Common.Vault.DependencyInjection;

public static class Extensions
{
    public static void AddVault<TSecretsDto>(this IConfigurationBuilder configurationBuilder)
    {
        var configuration = configurationBuilder.Build();
        // Load all secrets from vault and inject them in the configuration
        var vaultMangager = new TDVaultManager<TSecretsDto>(
            new TDVaultConfiguration
            {
                Uri = configuration["VAULT_URI"]!,
                Username = configuration["VAULT_USERNAME"]!,
                Password = configuration["VAULT_PASSWORD"]!,
                Engine = configuration["VAULT_ENGINE"]!,
                DefaultPath = configuration["VAULT_PATH"]!,
            });
    
        var secrets = vaultMangager.GetSecretsAsync().GetAwaiter().GetResult();

        foreach (var secret in secrets.GetType().GetProperties())
        {
            // In debug mode, we WANT to override secrets that are already set
#if DEBUG
            if (configuration[secret.Name] != null)
                continue;
#endif
            if (secret.PropertyType.IsArray)
                configuration[secret.Name] = string.Join(',', secret.GetValue(secrets) as string[] ?? []);
            else
                configuration[secret.Name] = secret.GetValue(secrets)?.ToString()!;
        }
    }
}