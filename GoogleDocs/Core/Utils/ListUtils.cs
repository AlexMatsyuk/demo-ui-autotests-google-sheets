namespace GoogleDocs.Utils;

public static class ListUtils
{
    public static void MoveToTheEnd<T>(this List<T> list, T element)
    {
        list.Remove(element);
        list.Add(element);
    }

    public static List<T> Exclude<T>(this List<T> list, List<T> elementsToExclude, params T[] otherElementsToExclude)
    {
        return list.Exclude(elementsToExclude).Exclude(otherElementsToExclude);
    }

    public static T[] Exclude<T>(this T[] list, params T[] elementsToExclude)
    {
        return Exclude(list.ToList(), elementsToExclude.ToList()).ToArray();
    }

    public static List<T> Exclude<T>(this List<T> list, params T[] elementsToExclude)
    {
        return Exclude(list, elementsToExclude.ToList());
    }

    public static List<T> Exclude<T>(this List<T> list, List<T> elementsToExclude)
    {
        return list.Except(elementsToExclude?.ToList() ?? []).ToList();
    }

    public static T[] With<T>(this IEnumerable<T> array, params T[] elements)
    {
        return array.ToList().With(elements).ToArray();
    }

    public static T[] With<T>(this IEnumerable<T> array, IEnumerable<T> elements)
    {
        return array.ToList().With(elements.ToArray()).ToArray();
    }

    public static List<T> With<T>(this T element, List<T> elements)
    {
        return new List<T> { element }.With(elements.ToArray());
    }

    public static Dictionary<TK, TV> AddRange<TK, TV>(this Dictionary<TK, TV> dictionary, Dictionary<TK, TV> addition)
        where TK : notnull
    {
        addition.ToList().ForEach(keyValuePair => dictionary.Add(keyValuePair.Key, keyValuePair.Value));
        return dictionary;
    }

    public static List<T> With<T>(this List<T> list, params T[] elements)
    {
        List<T> newList = [..list];
        newList.AddRange(elements.ToList());
        return newList;
    }
}