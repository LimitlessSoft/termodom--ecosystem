namespace TD.DbMigrations.Contracts.Constants
{
    public static class WebApiEndpoints
    {
        public static string GetToken(string username, string password) => $"/api/Korisnik/GetToken?username={username}&password={password}";
    }
}
