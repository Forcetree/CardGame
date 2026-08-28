using System;
using UnityEngine;

public class CardBuilder
{
    // Card Internal Settings
    public int? PlayCost { get; private set; } = null;
    public int? Value { get; private set; } = null;
    public int? CardTypeID { get; private set; } = null;
    public Card.cardState? State { get; private set; } = null;
    public string? Name { get; private set; } = null;
    public string? Title { get; private set; } = null;
    public string? Flavor { get; private set; } = null;

    // Fluent API
    public CardBuilder SetPlayCost(int? playCost) { PlayCost = playCost; return this; }
    public CardBuilder SetValue(int? value) { Value = value; return this; }
    public CardBuilder SetCardTypeID(int? cardTypeId) { CardTypeID = cardTypeId; return this; }
    public CardBuilder SetState(Card.cardState? state) { State = state; return this; }
    public CardBuilder SetName(string? name) { Name = name; return this; }
    public CardBuilder SetTitle(string? title) { Title = title; return this; }
    public CardBuilder SetFlavor(string? flavor) { Flavor = flavor; return this; }
}