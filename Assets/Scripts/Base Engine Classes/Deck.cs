using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Deck : MonoBehaviour
{
    [Header("Deck Settings: Prefab Parameters -> Can be set by code internal")]
    [Tooltip("Set this for deal rate")]
    public float TimeBetweenDeals;
    [Tooltip("The focus value is where the cards are pushed to before flying to the hand")]
    public Vector3 DrawFocusPos;
    [Tooltip("Set this for card speed on focus")]
    public float DealTime;
    [Tooltip("Set this for the type of focus movement")]
    public Ease DealEase;

    [Header("Deck Internal Settings")]
    [SerializeField] protected PlayHandler PlayHandler;
    [SerializeField] protected Card CardRef;
    [SerializeField] protected string DeckName;
    [SerializeField] protected bool IsFinite;
    [SerializeField] protected int DeckCount;
    [SerializeField] protected int[] TypeDistribution;
    [SerializeField] protected float KScalar;

    [Header("Visual Lists")]
    [SerializeField] protected float[] LiveDrawWeights;
    public List<Card> CardsInDeck = new();

    [Header("Deck Vis Indicators")]
    [Tooltip("Shows if we are processing a sequence of deals")]
    public bool IsProcessingDealBuffer = false;
    [SerializeField] protected Queue<Card> dealBuffer = new();

    public virtual void InitDeck()
    {
        // Assumes the linking has been baked into the scene/prefab
    }
    public void InitDeck(PlayHandler handler, Card cardRef, Action<DeckBuilder> configBlock)
    {
        this.PlayHandler = handler;
        this.CardRef = cardRef;

        DeckBuilder builder = new DeckBuilder(PlayHandler, CardRef);
        configBlock?.Invoke(builder);

        ApplySettings(builder, isInitialSetup: true);
    }

    public void UpdateSettings(Action<DeckBuilder> updateBlock)
    {
        if (PlayHandler == null)
        {
            Debug.LogError($"Deck on {gameObject.name} cannot be updated before initialization!");
            return;
        }

        DeckBuilder builder = new DeckBuilder(PlayHandler, CardRef);
        updateBlock?.Invoke(builder);

        ApplySettings(builder, isInitialSetup: false);
    }

    private void ApplySettings(DeckBuilder builder, bool isInitialSetup)
    {
        if (isInitialSetup)
        {            
            AssignDefaultValues(builder);
        }
        else // If parameter is provided override value else leave current
        {
            this.DeckName = builder.DeckName ?? this.DeckName;
            this.IsFinite = builder.IsFinite ?? this.IsFinite;
            this.DeckCount = builder.DeckCount ?? this.DeckCount;
            this.TypeDistribution = builder.TypeDistribution ?? this.TypeDistribution;
            this.TimeBetweenDeals = builder.TimeBetweenDeals ?? this.TimeBetweenDeals;
            this.DrawFocusPos = builder.DrawFocusPos ?? this.DrawFocusPos;
            this.DealTime = builder.DealTime ?? this.DealTime;
            this.DealEase = builder.DealEase ?? this.DealEase;
        }

        OnSettingsApplied();
    }

    // Abstractions
    protected abstract void AssignDefaultValues(DeckBuilder builder); // Default Value assignation owned by class extention
    protected abstract void OnSettingsApplied(); // Is called after settings are all freshly applied on Updates

    // Deck List Handlers
    public bool GenDeck()
    { 
        if (this.TypeDistribution == null) throw new InvalidOperationException("GenDeck failed: Null distribution.");
        if (this.TypeDistribution.Length == 0) throw new InvalidOperationException("GenDeck failed: No elements in distribution.");
        
        if (this.IsFinite)
        {
            GenFiniteDeck(DeckCount);           
        }
        else
        {
            FillDeck(DeckCount);
        }

        return false; 
    }

    protected void GenFiniteDeck(int size)
    {
        if (size == 0) throw new InvalidOperationException("GenFiniteDeck failed: Size zero.");

        int total = this.TypeDistribution.Sum();        
        if (total == 0) throw new InvalidOperationException("GenFiniteDeck failed: Empty distribution.");

        if (CardsInDeck.Count > 0) { Debug.LogWarning($"GenFiniteDeck: Non-empty deck. [Clearing deck] -> count={CardsInDeck.Count}", this); CardsInDeck.Clear(); }
        if (size % total > 0) Debug.LogWarning($"GenFiniteDeck uneven: Not a multiple. [Proceeding] -> deckCount={DeckCount}, total={total}", this);

        int[] prefix = new int[TypeDistribution.Length];
        int running = 0;

        for (int i = 0; i < TypeDistribution.Length; i++)
        {
            running += TypeDistribution[i];
            prefix[i] = running;
        }

        for (int i = 0; i < size; i++)
        {
            int pos = i % total;

            int typeIndex = 0;
            while (pos >= prefix[typeIndex]) typeIndex++;

            CardsInDeck.Add(CreateCard(i, typeIndex));
        }

        CardsInDeck.Shuffle();
    }

    public void FillDeck() => FillDeck(DeckCount);
    protected void FillDeck(int bufferDepth) 
    {   
        
    }

    public virtual bool GenDeck(int deckCount , int typeCount) // Old: Overload allows current functionality to avoid errors while rebuilding
    {
        for (int i = 0; i < deckCount; i++)
        {
            CardsInDeck.Add(CreateCard(i, i % typeCount));
        }

        CardsInDeck.Shuffle();

        return true;
    }

    // Temp Until Static Card Factory
    private Card CreateCard(int i, int type) // Ugly Temp SRP -> Need a static card method for handling this (encapsulate the card generation logic in the card class and call it from here) -> this will allow for better scalability and maintainability of the code
    {
        Card nCard = Instantiate(CardRef, this.transform.position, Quaternion.identity);

        // Under Review -> Consider creating custom card constructor that takes required arguments and handles this in one line for better clarity in the code

        nCard.gameObject.transform.parent = this.transform;
        nCard.PlayHandler = this.PlayHandler;

        nCard.playCost = 0; // Currently not used as all cards cost the same (this offers card scalability)

        nCard.state = Card.cardState.Deck; // Start the card in the deck state (should this be set here or in prefab? -> cards could be initialized anywhere in the game and not always in the deck)

        nCard.value = 0; // Made safe with introduction of ValueDigitizer (0 does not display)

        nCard.CardTypeID = (type); //Deck generates cards of a number of types based on the typeCount int 
        nCard.name = $"Card | {i + 1} | {nCard.CardTypeName}";
        nCard.title = $"{nCard.CardTypeName} Card";
        nCard.flavor = "Lorum Ipsum"; // Do we need flavor text for the base cards? Should this be defined in a dictionary set in card class?

        return nCard;
    }

    private IEnumerator DealSequence(PlayerHand hand)
    {
        IsProcessingDealBuffer = true;

        while (dealBuffer.Count > 0)
        {
            Card dCard = dealBuffer.Dequeue();
            dCard.destinationsBuffer.Enqueue((DrawFocusPos, DealTime, DealEase));
            hand.AddCardToHand(dCard);

            yield return new WaitForSeconds(TimeBetweenDeals);
        }

        IsProcessingDealBuffer = false;
        hand.AssignSequentialValues();
    }
    
    // Future abstraction in draw needed?
    public void DrawHand(PlayerHand hand) // Draw if possible -> automatic adjustment of hand positions triggered when a valid card is dealt to the hand
    {
        // Check and fill hand with cards up to the hand limit (ensure to check number of cards currently in the deal buffer and cards available in the deck)
        while ((CardsInDeck.Count > 0) && (hand.Count + dealBuffer.Count) < hand.handLimit)
        {
            Card dCard = CardsInDeck.Pop();
            dCard.gameObject.SetActive(true);
            dealBuffer.Enqueue(dCard);
        }
        StartCoroutine(DealSequence(hand));
    }
}
