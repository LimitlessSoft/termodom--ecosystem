using VaultSharp;
using VaultSharp.V1;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.UserPass;

namespace TD.Common.Vault;

public class TDVaultManager : ITDVaultManager
{
    private readonly IAuthMethodInfo _authMethodInfo;
    private readonly VaultClientSettings _vaultClientSettings;
    private readonly IVaultClient _vaultClient;
    internal readonly TDVaultConfiguration _tdVaultConfiguration;

    protected TDVaultManager(TDVaultConfiguration tdVaultConfiguration)
    {
        _authMethodInfo = new UserPassAuthMethodInfo(tdVaultConfiguration.Username, tdVaultConfiguration.Password);
        _vaultClientSettings = new VaultClientSettings(tdVaultConfiguration.Uri, _authMethodInfo);
        _vaultClient = new VaultClient(_vaultClientSettings);
        _tdVaultConfiguration = tdVaultConfiguration;
    }

    /// <summary>
    /// Returns secrets from specified path
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    public async Task<TDto> GetSecretsAsync<TDto>(string path) =>
        (await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync<TDto>(path, mountPoint: _tdVaultConfiguration.Engine)).Data.Data;
}

public class TDVaultManager<TDefaultDto> (TDVaultConfiguration tdVaultConfiguration)
    : TDVaultManager(tdVaultConfiguration), ITDVaultManager<TDefaultDto>
{
    /// <summary>
    /// Returns secrets from default path parsed to <see cref="TDefaultDto"/>
    /// </summary>
    /// <returns></returns>
    public async Task<TDefaultDto> GetSecretsAsync() =>
        await GetSecretsAsync<TDefaultDto>(_tdVaultConfiguration.DefaultPath!);
}