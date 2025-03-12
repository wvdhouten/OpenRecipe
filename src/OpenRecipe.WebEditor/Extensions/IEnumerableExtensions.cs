namespace OpenRecipe.WebEditor.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool ContainsAll<TType>(this IEnumerable<TType> items, IEnumerable<TType> filter)
        {
            return filter.All(items.Contains);
        }
    }
}
