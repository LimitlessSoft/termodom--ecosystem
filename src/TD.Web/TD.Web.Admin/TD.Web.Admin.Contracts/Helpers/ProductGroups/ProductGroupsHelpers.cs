using System.Text;

namespace TD.Web.Admin.Contracts.Helpers.ProductGroups
{
    public static class ProductGroupsHelpers
    {
        public static string FormatName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            // Replace spaces with hyphens
            name = name.Replace(' ', '-');

            // Replace non-ASCII characters with their ASCII equivalents
            var replacements = new Dictionary<char, char>
            {
                { 'á', 'a' },
                { 'é', 'e' },
                { 'í', 'i' },
                { 'ó', 'o' },
                { 'ú', 'u' },
                { 'Á', 'A' },
                { 'É', 'E' },
                { 'Í', 'I' },
                { 'Ó', 'O' },
                { 'Ú', 'U' },
                { 'ñ', 'n' },
                { 'Ñ', 'N' },
                { 'ü', 'u' },
                { 'Ü', 'U' },
                { 'ć', 'c' },
                { 'č', 'c' },
                { 'đ', 'd' },
                { 'š', 's' },
                { 'ž', 'z' },
                { 'Ć', 'C' },
                { 'Č', 'C' },
                { 'Đ', 'D' },
                { 'Š', 'S' },
                { 'Ž', 'Z' }
            };

            var sb = new StringBuilder(name.Length);
            foreach (var c in name)
            {
                sb.Append(replacements.ContainsKey(c) ? replacements[c] : c);
            }

            // Convert to lowercase
            return sb.ToString().ToLower();
        }
    }
}
