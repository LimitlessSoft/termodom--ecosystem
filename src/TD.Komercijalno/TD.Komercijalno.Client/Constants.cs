namespace TD.Komercijalno.Client;

public static class Constants
{
    public static string KomercijalnoApiUrlFormat (int year, TDKomercijalnoEnvironment env) => $"https://{year}-komercijalno{env.GetDescription()}.termodom.rs";
}