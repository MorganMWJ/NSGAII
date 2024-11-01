namespace NSGAII.Extensions;

public static class CollectionExtensions
{
    public static T Rand<T>(this ICollection<T> collection)
    {
        if (collection == null || collection.Count == 0)
            throw new ArgumentException("Collection must not be null or empty.");

        var random = new Random();
        int index = random.Next(collection.Count);
        return collection.ToList()[index];
    }
}
