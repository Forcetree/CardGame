using System;
using System.Collections.Generic;

public static class ListExtentions
{
    public static void Shuffle<T>(this IList<T> list) // Fisher-Yates Shuffle extention
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static T Pop<T>(this List<T> list) // Pops off an element from a list like in Python
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("Cannot pop: empty or null list.");
        }

        T itemToPop = list[^1];
        list.RemoveAt(list.Count - 1);

        return itemToPop;
    }
}
