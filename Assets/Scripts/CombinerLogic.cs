using System.Collections.Generic;
using System.Linq;

public abstract class CombinerLogic<TVisualType>
{
    protected readonly Dictionary<int, int> Combos = new();
    protected readonly Dictionary<int, TVisualType> VisualMatrix = new();

    public abstract void SetupCombinations();

    public bool TryResolve(IEnumerable<int> types, out int resolved)
    {
        // EnsureInit(); // Issue here is not knowing where the initialization is happening -> should be in the constructor of the derived class?
        int mask = ToMask(types);
        return Combos.TryGetValue(mask, out resolved);
    }

    public TVisualType GetVisual(int type)
    {
        return VisualMatrix.TryGetValue(type, out TVisualType visual) ? visual : default;
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
