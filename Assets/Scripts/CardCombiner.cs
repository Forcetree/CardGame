using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardCombiner
{
    // private static readonly Dictionary<int, Card.cardType> Combos = new();
    // private static readonly Dictionary<Card.cardType, Sprite> SpriteMatrix = new();
    private static bool _initialized;

    public static void InitMatrix()
    {
        if (_initialized) return;
        _initialized = true;

       

        
    }

    private static void EnsureInit()
    {
        if (!_initialized) InitMatrix();
    }

    //private static void AddCombo(IEnumerable<Card.cardType> types, Card.cardType result)
    //{
    //    int mask = ToMask(types);
    //    Combos[mask] = result;
    //}

    private static int ToMask(IEnumerable<Card.cardType> types)
    {
        int mask = 0;
        foreach (var t in types.Distinct())
        {
            mask |= 1 << (int)t;
        }
        return mask;
    }

    //public static bool TryResolve(IEnumerable<Card.cardType> types, out Card.cardType resolved)
    //{
    //    EnsureInit();
    //    int mask = ToMask(types);
    //    return Combos.TryGetValue(mask, out resolved);
    //}

    //public static Sprite GetSprite(Card.cardType type)
    //{
    //    EnsureInit();
    //    if (SpriteMatrix.TryGetValue(type, out var s)) return s;
    //    return null;
    //}
}
