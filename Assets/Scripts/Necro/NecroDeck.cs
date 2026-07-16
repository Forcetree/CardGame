using DG.Tweening;
using UnityEngine;

public class NecroDeck : Deck
{
    protected override void AssignDefaultValues(DeckBuilder builder)
    {
        this.DeckName = builder.DeckName ?? "NecroDeck";
        this.IsFinite = builder.IsFinite ?? false;
        this.DeckCount = builder.DeckCount ?? 6;
        this.KScalar = builder.KScalar ?? 20;
        this.TypeDistribution = builder.TypeDistribution ?? new int[] { 1, 0, 0, 0, 1, 0, 0, 3, 0 };
        this.TimeBetweenDeals = builder.TimeBetweenDeals ?? 0.2f;
        this.DrawFocusPos = builder.DrawFocusPos ?? new(-6, 1.5f, 0);
        this.DealTime = builder.DealTime ?? 0.1f;
        this.DealEase = builder.DealEase ?? Ease.InOutQuad;
    }

    protected override void OnSettingsApplied()
    {
        Debug.LogWarning($"[{this.GetType().Name}] Currently no action taken: {nameof(OnSettingsApplied)} is not implemented.");
    }
}
