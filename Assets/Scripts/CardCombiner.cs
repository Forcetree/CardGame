using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardCombiner
{
    // private static readonly Dictionary<int, Card.cardType> Combos = new();
    private static readonly Dictionary<Card.cardType, Sprite> SpriteMatrix = new();
    private static bool _initialized;

    // SpriteRefs
    public static Sprite fireSprite;
    public static Sprite growthSprite;
    public static Sprite earthSprite;
    public static Sprite ironSprite;
    public static Sprite frostSprite;
    public static Sprite waterSprite;
    public static Sprite windSprite;
    public static Sprite stormSprite;
    public static Sprite blightSprite;
    public static Sprite backSprite;

    public static void InitMatrix()
    {
        if (_initialized) return;
        _initialized = true;

        // Combos
        // AddCombo(new[] { Card.cardType.Fire, Card.cardType.Water }, Card

        // SpriteMatrix Init
        SpriteMatrix[Card.cardType.Fire] = fireSprite;
        SpriteMatrix[Card.cardType.Growth] = growthSprite;
        SpriteMatrix[Card.cardType.Earth] = earthSprite;
        SpriteMatrix[Card.cardType.Iron] = ironSprite;
        SpriteMatrix[Card.cardType.Frost] = frostSprite;
        SpriteMatrix[Card.cardType.Water] = waterSprite;
        SpriteMatrix[Card.cardType.Wind] = windSprite;
        SpriteMatrix[Card.cardType.Storm] = stormSprite;
        SpriteMatrix[Card.cardType.Blight] = blightSprite;
        SpriteMatrix[Card.cardType.Back] = backSprite;

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

    public static Sprite GetSprite(Card.cardType type)
    {
        EnsureInit();
        if (SpriteMatrix.TryGetValue(type, out var s)) return s;
        return null;
    }
}
