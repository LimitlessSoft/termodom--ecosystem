namespace TD.Common.Vault;

public class TDVaultConfiguration
{
    public required string Engine { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Uri { get; init; }
    /// <summary>
    /// Specifies the default path for secrets inside vault engine
    /// Pass null if you are not going to use TDVaultManager with default SecretsDto
    /// </summary>
    public required string? DefaultPath { get; init; }
}