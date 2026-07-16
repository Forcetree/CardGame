using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

public class DeckBuilder
{
    // Required References
    public PlayHandler Handler { get; private set; }
    public Card CardRef { get; private set; }

    // Deck Internal Settings
    public string? DeckName { get; private set; } = null;
    public bool? IsFinite { get; private set; } = null; // This setting decides if the deck is finite or infinite. If infinite, the deck will generate cards on the fly and will not have a set number of cards nor preset list
    public int? DeckCount { get; private set; } = null; // How many cards appear in the deck (this should be configurable in the deck builder)
    public int[]? TypeDistribution { get; private set; } = null; // This array will be used to determine the distribution of types in the deck (this should be configurable in the deck builder)
    public int? KScalar { get; private set; } = null; // The Gain Factor of the debt algorithm

    // Functional Animation Settings
    public float? TimeBetweenDeals { get; private set; } = null; // How long to wait between dealing cards (this should be configurable in the deck builder)
    public Vector3? DrawFocusPos { get; private set; } = null; // The position where cards are drawn from (this should be configurable in the deck builder)
    public float? DealTime { get; private set; } = null; // The time/speed it takes to deal a card (this should be configurable in the deck builder)
    public Ease? DealEase { get; private set; } = null; // The ease function for the deal animation (this should be configurable in the deck builder)

    // Required API
    public DeckBuilder(PlayHandler handler, Card cardRef)
    {
        this.Handler = handler;
        this.CardRef = cardRef;
    }

    // Fluent API
    public DeckBuilder SetDeckName(string? name)
    {
        this.DeckName = name;
        return this;
    }
    public DeckBuilder SetIsFinite(bool? isFinite)
    {
        this.IsFinite = isFinite;
        return this;
    }
    public DeckBuilder SetDeckCount(int? count)
    {
        this.DeckCount = count;
        return this;
    }
    public DeckBuilder SetTimeBetweenDeals(float? time)
    {
        this.TimeBetweenDeals = time;
        return this;
    }
    public DeckBuilder SetDrawFocusPos(Vector3? pos)
    {
        this.DrawFocusPos = pos;
        return this;
    }
    public DeckBuilder SetDealTime(float? time)
    {
        this.DealTime = time;
        return this;
    }
    public DeckBuilder SetDealEase(Ease? ease)
    {
        this.DealEase = ease;
        return this;
    }
    public DeckBuilder SetKScalar(int? K)
    {
        this.KScalar = K;
        return this;
    }

    // Dynamic Fluents
    public DeckBuilder SetTypeDistribution(uint[]? distribution)
    {
        if (distribution == null)
        {
            this.TypeDistribution = null;
            return this;
        }
        else if (distribution.Length == 0)
        {
            throw new InvalidOperationException("[DECK BUILDER ERROR] The distribution array is defined as empty.");
        }
        else if (!distribution.Any(x => x > 0))
        {
            throw new InvalidOperationException("[DECK BUILDER ERROR] The distribution array is all 0. At least one item must be greater than 0.");
        }

        this.TypeDistribution = distribution.Select(x => (int)x).ToArray();
        return this;
    }
}
