using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PaintDeck : MonoBehaviour
{
    [Tooltip("The hand is set during active play by the PlayHandler")]
    public PlayerHand hand; // Needs Hand object to talk to -> filled on initialization of the deck in PlayHandler
    [Tooltip("The PlayHandler is set during active play by the PlayHandler")]
    public PlayHandler PlayHandler; // Needs PlayHandler object to talk to -> filled on initialization of the deck in PlayHandler
    [Tooltip("The Prefab for Cards is set during active play by the PlayHandler")]
    public Card CardRef;

    public int DeckSize => cardsInDeck.Count; // Is needed?
    public List<Card> cardsInDeck = new();

    private Queue<Card> dealBuffer = new();

    [Tooltip("Shows if we are processing a sequence of deals")]
    public bool isProcessingDealBuffer = false;

    [Tooltip("Set this for deal rate")]
    public float timeBetweenDeals;
    [Tooltip("Set this for card speed on focus")]
    public float dealTime;
    [Tooltip("Set this for the type of focus movement")]
    public Ease dealEase;

    [Tooltip("The focus value is where the cards are pushed to before flying to the hand")]
    public Vector3 drawFocusPos = new(-6, 1.5f, 0);

    public void InitDeck(Card CardRef, PlayHandler handler)
    {
        this.CardRef = CardRef;
        this.PlayHandler = handler;
    }

    public bool GenDeck(int deckCount)
    {
        for (int i = 0; i < deckCount; i++)
        {
            Card nCard = Instantiate(CardRef, this.transform.position, Quaternion.identity);

            // Under Review -> Consider creating custom card constructor that takes required arguments and handles this in one line for better clarity in the PlayHandler code

            nCard.gameObject.transform.parent = this.transform;
            nCard.PlayHandler = this.PlayHandler;

            nCard.playCost = 0;
            nCard.value = 0; // Made safe with introduction of ValueDigitizer (0 does not display)
            nCard.type = (Card.cardType)(i % 5);
            nCard.name = $"Card | {i + 1} | {nCard.type}";
            nCard.title = $"{nCard.type} Card";
            nCard.flavor = "Lorum Ipsum"; // Do we need flavor text for the base cards? Should this be defined in a dictionary set in card class?

            cardsInDeck.Add(nCard);
        }

        cardsInDeck.Shuffle();

        return true;
    }

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
