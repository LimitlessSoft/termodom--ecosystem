using LSCore.Contracts.Extensions;

namespace TD.Office.InterneOtpremnice.Client;

public class Constants
{
    public static string ApiUrlFormat (TDInterneOtpremniceEnvironment env) =>
        $"https://api-interne-otpremnice{env.GetDescription()}.termodom.rs";
}