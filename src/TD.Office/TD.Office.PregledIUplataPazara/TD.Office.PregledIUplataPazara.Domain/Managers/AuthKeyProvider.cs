using LSCore.Auth.Key.Contracts;

namespace TD.Office.PregledIUplataPazara.Domain.Managers;

public class AuthKeyProvider : ILSCoreAuthKeyProvider
{
    public bool IsValidKey(string key)
    {
        return true;
    }
}