using System.Collections.Generic;
using System.Linq;

public abstract class CombinerLogic
{
    protected readonly Dictionary<int, int> Combos = new();

    public abstract void SetupCombinations();

    public bool TryResolve(IEnumerable<int> types, out int resolved)
    {
        int mask = ToMask(types);
        return Combos.TryGetValue(mask, out resolved);
    }

    protected static int ToMask(IEnumerable<int> types)
    {
        int mask = 0;
        foreach (var t in types.Distinct())
        {
            mask |= 1 << t;
        }
        return mask;
    }

    protected void AddCombo(IEnumerable<int> types, int result)
    {
        Combos[ToMask(types)] = result;
    }
}

public abstract class CombinerLogic<TVisualType> : CombinerLogic
{
    protected readonly Dictionary<int, TVisualType> VisualMatrix = new();

    public TVisualType GetVisual(int type)
    {
        return VisualMatrix.TryGetValue(type, out TVisualType visual) ? visual : default;
    }
}