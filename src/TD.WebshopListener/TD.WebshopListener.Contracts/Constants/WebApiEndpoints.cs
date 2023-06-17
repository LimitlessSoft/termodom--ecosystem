namespace TD.WebshopListener.Contracts.Constants
{
    public static class WebApiEndpoints
    {
        public static string GetToken(string username, string password) => $"/api/Korisnik/GetToken?username={username}&password={password}";
        public static string GetAkcList() => $"/api/akc/list";
    }
}
