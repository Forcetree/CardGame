using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using Unity.VisualScripting;
using DG.Tweening;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;

public class PlayHandler : MonoBehaviour
{
    // Objects
    public GameObject Hand;
    public GameObject Deck;
    public GameObject Graveyard;
    public GameObject Field;

    // Prefabs
    public Card CardRef; // To create new cards

    // Play Vars
    public bool playActive = false;
        // Score
        // Win/Loss

    // Deck Vars
    public int DeckSize => deck.Count; // Is needed?
    public List<Card> deck = new();
    
    private Queue<Card> dealBuffer = new();
    private bool isProcessingDealBuffer = false;
    public float timeBetweenDeals;
    public float dealTime;
    public Ease dealEase;

    public Vector3 drawFocusPos = new(-6, 1.5f, 0); // World location -> prepare for trouble (consider converting to live read from the deck position in future -> must convert world to local to achieve)

    // Hand Vars
    public int handLimit;
    
    private List<(Card card, Vector3 pos)> hand = new(); // Hand is splayed backwards for O(1) effciency in draw and add operations
    public bool handChange = false;
    public float handSpread = 1f;
    public float toHandTime = .2f;
    public Ease toHandEase = Ease.InQuad;

    public Vector3 handHome = new(0, -3, 0); // This is not currently used (do I need it?)

    // Graveyard Vars
    public List<Card> graveyard = new();

    // Runtime
    void Start()
    {
        StartPlay(20, 6);
    }

    void Update()
    {
        if (playActive) // Allows pausing and other operations
        {
            // Deck Updates
            

            // Hand Updates
            UpdateHand();
        }
    }

    // Public Methods
    public void StartPlay(int deckCount, int handLimit)
    {
        // Create and shuffle the deck
        GenDeck(deckCount);
        deck.Shuffle();

        this.handLimit = handLimit;

        playActive = true;
    }

    // Deck Methods
    private bool GenDeck(int deckCount)
    {
        for (int i = 0; i < deckCount; i++)
        {
            Card nCard = Instantiate(CardRef, Deck.transform.position, Quaternion.identity);

            // Under Review -> Consider creating custom card constructor that takes required arguments and handles this in one line for better clarity in the PlayHandler code

            nCard.gameObject.transform.parent = Deck.transform;
            nCard.PlayHandler = this;

            nCard.playCost = 0;
            nCard.value = 0;
            nCard.type = (Card.cardType)(i % 5);
            nCard.name = $"Card | {i + 1} | {nCard.type}";
            nCard.title = $"{nCard.type} Card";
            nCard.flavor = "Lorum Ipsum"; // Do we need flavor text for the base cards? Should this be defined in a dictionary set in card class?

            deck.Add(nCard);
        }

        return true;
    }

    private void Deal(Card card)
    {
        dealBuffer.Enqueue(card);

        if (!isProcessingDealBuffer)
        {
            StartCoroutine(DealSequence());
        }        
    }

    private IEnumerator DealSequence()
    {
        isProcessingDealBuffer = true;

        while (dealBuffer.Count > 0)
        {
            Card dCard = dealBuffer.Dequeue();
            dCard.destinationsBuffer.Enqueue((drawFocusPos, dealTime, dealEase));
            AddCardToHand(dCard);
            dCard.transform.parent = Hand.transform;

            yield return new WaitForSeconds(timeBetweenDeals);
        }

        isProcessingDealBuffer = false;
    }

    // Hand Methods
    public void UpdateHand() // Draw if possible then adjust the position of the cards in the hand
    {
        // Check and fill hand with cards (ensure to check number of cards in the deal buffer and cards available in the deck)
        while ((deck.Count > 0) && (hand.Count + dealBuffer.Count) < handLimit)
        {
            Card dCard = deck.Pop();
            dCard.gameObject.SetActive(true);
            Deal(dCard);
        }
        
        // Update the card positions for each card in the hand if there was a change for only cards which are not already queued to the intended destination
        if (handChange)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].card.spriteRenderer.sortingOrder != i+1) // Update render order if it changed
                { 
                    hand[i].card.spriteRenderer.sortingOrder = i+1; 
                } 
                
                if (hand[i].card.handHome != hand[i].pos)
                {
                    hand[i].card.handHome = hand[i].pos;
                }

                if (hand[i].card.destinationsBuffer.Count > 0)
                {
                    if (hand[i].pos != hand[i].card.destinationsBuffer.Peek().pos) // Add a destination if card is not already headed to the destination (only checks if the destination is queued not if the card is resting at the location)
                    {
                        hand[i].card.destinationsBuffer.Enqueue((hand[i].pos, toHandTime, toHandEase));
                    }
                }
                else if (hand[i].card.transform.localPosition != hand[i].pos)
                {
                    hand[i].card.destinationsBuffer.Enqueue((hand[i].pos, toHandTime, toHandEase));
                }
            }

            handChange = false;
        }        
    }

    private void AddCardToHand(Card nCard)
    {
        hand.Add((nCard, new()));

        for (int i = 0;i < hand.Count; i++)
        {
            (Card card, Vector3 pos) unpackedTup = hand[i];

            float xOffset = (i - (hand.Count - 1) * 0.5f) * handSpread;
            unpackedTup.pos = new Vector3(-xOffset, 0, 0); // Set the pos value for the determined hand location

            hand[i] = unpackedTup;
        }

        handChange = true;
    }

    private void RemoveCardFromHand(Card nCard)
    {
        // Under construction
    }

}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list) // Fisher-Yates Shuffle extention
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static T Pop<T>(this List<T> list) // Pops off an element from a list like in Python (return null if list is null or empty)
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("Cannot pop: empty or null list.");
        }

        T itemToPop = list[^1];
        list.RemoveAt(list.Count - 1);

        return itemToPop;
    }
}
