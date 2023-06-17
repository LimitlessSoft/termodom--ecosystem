using System.Collections;

namespace TD.Core.Framework.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty(this ICollection sender)
        {
            if (sender.Count == 0)
                return true;

            return false;
        }
    }
}
