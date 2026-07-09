using UnityEngine;
using DG.Tweening;

public class PaintDeck : Deck
{
    protected override void AssignDefaultValues(DeckBuilder builder)
    {
        this.DeckName = builder.DeckName ?? "PaintDeck";
        this.IsFinite = builder.IsFinite ?? true;
        this.DeckCount = builder.DeckCount ?? 60;
        this.TypeDistribution = builder.TypeDistribution ?? new int[] { 1, 1, 1, 1, 1 };
        this.TimeBetweenDeals = builder.TimeBetweenDeals ?? 0.2f;
        this.DrawFocusPos = builder.DrawFocusPos ?? new(-6, 1.5f, 0);
        this.DealTime = builder.DealTime ?? 0.1f;
        this.DealEase = builder.DealEase ?? Ease.InExpo; // Unsure if this is the one we were using...
    }

    protected override void OnSettingsApplied()
    {
        Debug.LogWarning($"[{this.GetType().Name}] Currently no action taken: {nameof(OnSettingsApplied)} is not implemented.");
    }
}
