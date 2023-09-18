using System.Reflection;

namespace TD.Core.Contracts.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetDescription(this Enum value)
        {
            if (value == null)
                return null;

            var fi = value.GetType().GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);

            if (fi == null)
                return null;

            return fi.GetDescription();
        }
        public static string GetDescription(this Enum value, string defaultValue) => value.GetDescription() ?? defaultValue;
    }
}
