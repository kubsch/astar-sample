namespace RandomEngine.Extensions;

public static class DictionaryExtensions
{
    public static bool ContainsAllKeys<T, U>(this Dictionary<T, U> instance, params T[] keys)
        where T : notnull
    {
        foreach (var key in keys)
            if (!instance.ContainsKey(key))
                return false;

        return true;
    }
}