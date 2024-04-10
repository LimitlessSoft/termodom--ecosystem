namespace TD.Web.Cronjobs.Common.Helpers
{
    public class ApiHelper
    {
        public static string GetDokumentiEndpoint(int? vrDok, int? brDok)
            => string.Format(Constants.Constants.KomercijalnoApiRoot, DateTime.Now.Year, vrDok, brDok);
    }
}
