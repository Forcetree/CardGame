using System.Collections.Generic;
using UnityEngine;

public static class CardCombiner
{
    private static CombinerLogic _activeLogic;
    private static bool _initialized;

    public static void Initialize<TVisualType>(CombinerLogic logic)
    {
        if (_initialized)
        {
            Debug.LogWarning("CardCombiner: Already initialized. Ignoring overwrite attempt.");
            return;
        }

        _activeLogic = logic;
        logic.SetupCombinations();

        _initialized = true;
    }

    private static void EnsureInit()
    {
        if (!_initialized)
        {
            throw new System.InvalidOperationException("CardCombiner Error: A script tried to resolve a combo or look up a visual before the GameManager initialized the branch logic!");
            // Temporary fix and debug: Initialize with default logic if not initialized?
        }
    }

    public static bool TryResolve(IEnumerable<int> types, out int resolved)
    {
        EnsureInit();

        return _activeLogic.TryResolve(types, out resolved);
    }

    public static TVisualType GetVisual<TVisualType>(int type)
    {
        EnsureInit();

        var visualLayer = _activeLogic as CombinerLogic<TVisualType>;

        return visualLayer != null ? visualLayer.GetVisual(type) : default;
    }
}
