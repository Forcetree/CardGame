using UnityEngine;
using DG.Tweening;

public class PaintDeck : Deck
{
    protected override void AssignDefaultValues(DeckBuilder builder)
    {
        this.DeckName = builder.DeckName ?? "PaintDeck";
        this.IsFinite = builder.IsFinite ?? true;
        this.DeckCountLimit = builder.DeckCount ?? 60;
        this.KScalar = builder.KScalar ?? 20;
        this.TypeDistribution = builder.TypeDistribution ?? new int[] { 1, 1, 1, 1, 1 };
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
