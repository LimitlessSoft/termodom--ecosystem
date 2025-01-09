using System.ComponentModel;

namespace TD.Komercijalno.Client;

public static class Extensions
{
    public static string GetDescription(this TDKomercijalnoEnvironment env) =>
        (env.GetType()
            .GetField(env.ToString())!
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .First() as DescriptionAttribute)!.Description;
}