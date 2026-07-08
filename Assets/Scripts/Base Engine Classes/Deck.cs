using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class Deck : MonoBehaviour
{
    [Header("Deck References: Set automatically")]
    [Tooltip("The hand is set during active play by the PlayHandler")]
    public PlayerHand hand; // Never used -> filled on initialization of the deck in PlayHandler -> all methods that require the hand are passed the hand as an argument -> consider removing this variable
    [Tooltip("The PlayHandler is set during active play by the PlayHandler")]
    public PlayHandler PlayHandler; // Needs PlayHandler object to talk to -> filled on initialization of the deck in PlayHandler

    [Header("Deck Settings: Prefab Links")]
    [Tooltip("The Prefab for Cards is NOT set during active play by the PlayHandler -> baked in the prefab")]
    public Card CardRef;

    [Header("Deck Settings: Parameters")]
    [Tooltip("Set this for deal rate")]
    public float timeBetweenDeals;
    [Tooltip("The focus value is where the cards are pushed to before flying to the hand")]
    public Vector3 drawFocusPos = new(-6, 1.5f, 0);
    [Tooltip("Set this for card speed on focus")]
    public float dealTime;
    [Tooltip("Set this for the type of focus movement")]
    public Ease dealEase;

    [Header("Deck Vis Indicators")]
    [Tooltip("Shows if we are processing a sequence of deals")]
    public bool isProcessingDealBuffer = false;
    public int DeckSize => cardsInDeck.Count; // Is needed? Not currently used

    // Required -> The deal buffer is used to store cards that are being dealt to the hand. This allows for a smooth deal animation and prevents cards from being dealt too quickly.
    private Queue<Card> dealBuffer = new();

    // Debating removal
        // Some use cases need a deck object that does not need a list of cards in the deck (for example, a deck that is used to generate cards on the fly). This allows for a deck object to be created without needing to generate cards in the deck.
        // Consider moving this down to the child class if required
    public List<Card> cardsInDeck = new();
    
    public virtual void InitDeck()
    {
        // Assumes the linking has been baked into the scene/prefab
    }
    public void InitDeck(Card CardRef, PlayHandler handler) // Under Construction -> Introducing the Builder objects
    {
        this.CardRef = CardRef;
        this.PlayHandler = handler;
    }

    public virtual bool GenDeck() { return false; } // Base class does not generate a deck, child classes should override this method to generate a special deck of cards
    public virtual bool GenDeck(int deckCount , int typeCount) // Overload allows current functionality to be used in child classes that need to generate a deck of cards with a specific number of cards and types
    {
        for (int i = 0; i < deckCount; i++) // Need a static card method for handling this (encapsulate the card generation logic in the card class and call it from here) -> this will allow for better scalability and maintainability of the code
        {
            Card nCard = Instantiate(CardRef, this.transform.position, Quaternion.identity);

            // Under Review -> Consider creating custom card constructor that takes required arguments and handles this in one line for better clarity in the PlayHandler code

            nCard.gameObject.transform.parent = this.transform;
            nCard.PlayHandler = this.PlayHandler;

            nCard.playCost = 0; // Currently not used as all cards cost the same (this offers card scalability)

            nCard.state = Card.cardState.Deck; // Start the card in the deck state (should this be set here or in prefab? -> cards could be initialized anywhere in the game and not always in the deck)

            nCard.value = 0; // Made safe with introduction of ValueDigitizer (0 does not display)

            nCard.CardTypeID = (i % typeCount); //Deck generates cards of a number of types based on the typeCount int 
            nCard.name = $"Card | {i + 1} | {nCard.CardTypeName}";
            nCard.title = $"{nCard.CardTypeName} Card";
            nCard.flavor = "Lorum Ipsum"; // Do we need flavor text for the base cards? Should this be defined in a dictionary set in card class?

            cardsInDeck.Add(nCard);
        }

        cardsInDeck.Shuffle();

        return true;
    }

    public virtual void UpdateDeck() { } // Base class does not update the deck, child classes could override this method to update the deck based on game logic
    // Consider creating a data object for the deck that can be used to store the deck state and allow for saving/loading of the deck state as well as changing other deck parameters (for example, changing the number of cards in the deck or the types of cards in the deck) -> this would allow for better scalability and maintainability of the code
    // This could be the same data object used in the init -> consider the core differences between initialization and updating the deck from a functional standpoint

    private IEnumerator DealSequence(PlayerHand hand)
    {
        isProcessingDealBuffer = true;

        while (dealBuffer.Count > 0)
        {
            Card dCard = dealBuffer.Dequeue();
            dCard.destinationsBuffer.Enqueue((drawFocusPos, dealTime, dealEase));
            hand.AddCardToHand(dCard);

            yield return new WaitForSeconds(timeBetweenDeals);
        }

        isProcessingDealBuffer = false;
        hand.AssignSequentialValues();
    }

    public void DrawHand(PlayerHand hand) // Draw if possible -> automatic adjustment of hand positions triggered when a valid card is dealt to the hand
    {
        // Check and fill hand with cards up to the hand limit (ensure to check number of cards currently in the deal buffer and cards available in the deck)
        while ((cardsInDeck.Count > 0) && (hand.Count + dealBuffer.Count) < hand.handLimit)
        {
            Card dCard = cardsInDeck.Pop();
            dCard.gameObject.SetActive(true);
            dealBuffer.Enqueue(dCard);
        }
        StartCoroutine(DealSequence(hand));
    }
}
