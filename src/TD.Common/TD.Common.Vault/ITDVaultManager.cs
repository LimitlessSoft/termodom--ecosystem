namespace TD.Common.Vault;

public interface ITDVaultManager
{
    public Task<TDto> GetSecretsAsync<TDto>(string path);
}

public interface ITDVaultManager<TDefaultDto> : ITDVaultManager
{
    public Task<TDefaultDto> GetSecretsAsync();
}