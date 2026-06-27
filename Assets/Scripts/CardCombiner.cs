using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardCombiner
{
    private static readonly Dictionary<int, int> Combos = new(); // Key: bitmask of card types, Value: resulting card type in int form for implicit conversion to CardType enum
    private static readonly Dictionary<int, Color> ColorMatrix = new();
    private static bool _initialized;

    public static void InitMatrix()
    {
        if (_initialized) return;
        _initialized = true;

        // Need to provide combos for matrix -> how and where is yet to be determined. For now, we will hardcode some example combos.

        // --- Combo table (seed values) ---
        AddCombo(new[] { 0 }, 0);
        AddCombo(new[] { 2 }, 2);
        AddCombo(new[] { 1 }, 1);
        AddCombo(new[] { 3 }, 3);
        AddCombo(new[] { 4 }, 4);

        AddCombo(new[] { 0, 2 }, 11);
        AddCombo(new[] { 0, 1 }, 12);
        AddCombo(new[] { 2, 1 }, 13);

        AddCombo(new[] { 0, 3 }, 5);
        AddCombo(new[] { 2, 3 }, 7);
        AddCombo(new[] { 1, 3 }, 6);

        AddCombo(new[] { 0, 4 }, 8);
        AddCombo(new[] { 2, 4 }, 10);
        AddCombo(new[] { 1, 4 }, 9);

        AddCombo(new[] { 0, 2, 3 }, 14);
        AddCombo(new[] { 0, 2, 4 }, 17);

        AddCombo(new[] { 0, 1, 3 }, 15);
        AddCombo(new[] { 0, 1, 4 }, 18);

        AddCombo(new[] { 2, 1, 3 }, 16);
        AddCombo(new[] { 2, 1, 4 }, 19);

        // --- Colors ---
        ColorMatrix[0] = Color.red;
        ColorMatrix[2] = Color.blue;
        ColorMatrix[1] = Color.yellow;
        ColorMatrix[3] = Color.white;
        ColorMatrix[4] = Color.black;

        ColorMatrix[11] = new Color32(0x53, 0x07, 0x5B, 0xFF);
        ColorMatrix[12] = new Color32(0xC7, 0x3F, 0x25, 0xFF);
        ColorMatrix[13] = new Color32(0x55, 0x85, 0x00, 0xFF);

        ColorMatrix[5] = new Color32(0xE3, 0x6F, 0x74, 0xFF);
        ColorMatrix[7] = new Color32(0x2C, 0x50, 0xDE, 0xFF);
        ColorMatrix[6] = new Color32(0xFF, 0xD4, 0x17, 0xFF);

        ColorMatrix[14] = new Color32(0x90, 0x5A, 0xFF, 0xFF);
        ColorMatrix[15] = new Color32(0xF0, 0xD9, 0x6C, 0xFF);
        ColorMatrix[16] = new Color32(0x3A, 0x52, 0x3B, 0xFF);

        ColorMatrix[8] = new Color32(0x3C, 0x07, 0x07, 0xFF);
        ColorMatrix[10] = new Color32(0x03, 0x1A, 0x2A, 0xFF);
        ColorMatrix[9] = new Color32(0xFF, 0xAB, 0x1D, 0xFF);

        ColorMatrix[17] = new Color32(0x17, 0x0E, 0x24, 0xFF);
        ColorMatrix[18] = new Color32(0x65, 0x15, 0x00, 0xFF);
        ColorMatrix[19] = new Color32(0x0B, 0x27, 0x00, 0xFF);

    }

    private static void EnsureInit()
    {
        if (!_initialized) InitMatrix();
    }

    private static void AddCombo(IEnumerable<int> types, int result)
    {
        int mask = ToMask(types);
        Combos[mask] = result;
    }

    private static int ToMask(IEnumerable<int> types)
    {
        int mask = 0;
        foreach (var t in types.Distinct())
        {
            mask |= 1 << t;
        }
        return mask;
    }

    public static bool TryResolve(IEnumerable<int> types, out int resolved)
    {
        EnsureInit();
        int mask = ToMask(types);
        return Combos.TryGetValue(mask, out resolved);
    }

    public static Color GetColor(int type)
    {
        EnsureInit();
        if (ColorMatrix.TryGetValue(type, out var c)) return c;
        return Color.white;
    }
}
