using System.Reflection;

namespace TD.Core.Contracts.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null)
                return "Undefined description";

            var fi = value.GetType().GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);

            if (fi == null)
                return "Undefined description";

            return fi.GetDescription() ?? "Undefined description";
        }
    }
}
