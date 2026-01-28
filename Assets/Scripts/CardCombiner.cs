using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardCombiner
{
    private static readonly Dictionary<int, Card.cardType> Combos = new();
    private static readonly Dictionary<Card.cardType, Color> ColorMatrix = new();
    private static bool _initialized;

    public static void InitMatrix()
    {
        if (_initialized) return;
        _initialized = true;

        // --- Combo table (seed values) ---
        AddCombo(new[] { Card.cardType.Red }, Card.cardType.Red);
        AddCombo(new[] { Card.cardType.Blue }, Card.cardType.Blue);
        AddCombo(new[] { Card.cardType.Yellow }, Card.cardType.Yellow);
        AddCombo(new[] { Card.cardType.White }, Card.cardType.White);
        AddCombo(new[] { Card.cardType.Black }, Card.cardType.Black);

        AddCombo(new[] { Card.cardType.Red, Card.cardType.Blue }, Card.cardType.Toxic);
        AddCombo(new[] { Card.cardType.Red, Card.cardType.Yellow }, Card.cardType.Amber);
        AddCombo(new[] { Card.cardType.Blue, Card.cardType.Yellow }, Card.cardType.Life);

        AddCombo(new[] { Card.cardType.Red, Card.cardType.White }, Card.cardType.Coral);
        AddCombo(new[] { Card.cardType.Blue, Card.cardType.White }, Card.cardType.Hydro);
        AddCombo(new[] { Card.cardType.Yellow, Card.cardType.White }, Card.cardType.Sun);

        AddCombo(new[] { Card.cardType.Red, Card.cardType.Black }, Card.cardType.Blood);
        AddCombo(new[] { Card.cardType.Blue, Card.cardType.Black }, Card.cardType.Abyss);
        AddCombo(new[] { Card.cardType.Yellow, Card.cardType.Black }, Card.cardType.Gold);

        AddCombo(new[] { Card.cardType.Red, Card.cardType.Blue, Card.cardType.White }, Card.cardType.Iris);
        AddCombo(new[] { Card.cardType.Red, Card.cardType.Blue, Card.cardType.Black }, Card.cardType.Obsidian);

        AddCombo(new[] { Card.cardType.Red, Card.cardType.Yellow, Card.cardType.White }, Card.cardType.Nectar);
        AddCombo(new[] { Card.cardType.Red, Card.cardType.Yellow, Card.cardType.Black }, Card.cardType.Lava);

        AddCombo(new[] { Card.cardType.Blue, Card.cardType.Yellow, Card.cardType.White }, Card.cardType.Moss);
        AddCombo(new[] { Card.cardType.Blue, Card.cardType.Yellow, Card.cardType.Black }, Card.cardType.Serpenite);

        // --- Colors ---
        ColorMatrix[Card.cardType.Red] = Color.red;
        ColorMatrix[Card.cardType.Blue] = Color.blue;
        ColorMatrix[Card.cardType.Yellow] = Color.yellow;
        ColorMatrix[Card.cardType.White] = Color.white;
        ColorMatrix[Card.cardType.Black] = Color.black;

        ColorMatrix[Card.cardType.Toxic] = new Color32(0x53, 0x07, 0x5B, 0xFF);
        ColorMatrix[Card.cardType.Amber] = new Color32(0xC7, 0x3F, 0x25, 0xFF);
        ColorMatrix[Card.cardType.Life] = new Color32(0x55, 0x85, 0x00, 0xFF);

        ColorMatrix[Card.cardType.Coral] = new Color32(0xE3, 0x6F, 0x74, 0xFF);
        ColorMatrix[Card.cardType.Hydro] = new Color32(0x2C, 0x50, 0xDE, 0xFF);
        ColorMatrix[Card.cardType.Sun] = new Color32(0xFF, 0xD4, 0x17, 0xFF);

        ColorMatrix[Card.cardType.Iris] = new Color32(0x90, 0x5A, 0xFF, 0xFF);
        ColorMatrix[Card.cardType.Nectar] = new Color32(0xF0, 0xD9, 0x6C, 0xFF);
        ColorMatrix[Card.cardType.Moss] = new Color32(0x3A, 0x52, 0x3B, 0xFF);

        ColorMatrix[Card.cardType.Blood] = new Color32(0x3C, 0x07, 0x07, 0xFF);
        ColorMatrix[Card.cardType.Abyss] = new Color32(0x03, 0x1A, 0x2A, 0xFF);
        ColorMatrix[Card.cardType.Gold] = new Color32(0xFF, 0xAB, 0x1D, 0xFF);

        ColorMatrix[Card.cardType.Obsidian] = new Color32(0x17, 0x0E, 0x24, 0xFF);
        ColorMatrix[Card.cardType.Lava] = new Color32(0x65, 0x15, 0x00, 0xFF);
        ColorMatrix[Card.cardType.Serpenite] = new Color32(0x0B, 0x27, 0x00, 0xFF);
    }

    private static void EnsureInit()
    {
        if (!_initialized) InitMatrix();
    }

    private static void AddCombo(IEnumerable<Card.cardType> types, Card.cardType result)
    {
        int mask = ToMask(types);
        Combos[mask] = result;
    }

    private static int ToMask(IEnumerable<Card.cardType> types)
    {
        int mask = 0;
        foreach (var t in types.Distinct())
        {
            mask |= 1 << (int)t;
        }
        return mask;
    }

    public static bool TryResolve(IEnumerable<Card.cardType> types, out Card.cardType resolved)
    {
        EnsureInit();
        int mask = ToMask(types);
        return Combos.TryGetValue(mask, out resolved);
    }

    public static Color GetColor(Card.cardType type)
    {
        EnsureInit();
        if (ColorMatrix.TryGetValue(type, out var c)) return c;
        return Color.white;
    }
}
