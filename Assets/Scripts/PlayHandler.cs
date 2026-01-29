using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using DG.Tweening;

// Refactor needed: Break the labeled main components apart into single purpose objects
    // Hand
    // Deck
    // Mana

[Serializable] public class ScoreData
{
    public int score;
    public int turns;

    public ScoreData(int score, int turns)
    {
        this.score = score;
        this.turns = turns;
    }
}

public class PlayHandler : MonoBehaviour
{
    // Objects
    public GameObject Hand;
    public GameObject Deck;
    public GameObject Graveyard;
    public GameObject Field;

    public Mana mana1; // Temp ugly implementation
    public Mana mana2; // -> Will change to use prefab and instantiate the mana in the scene on run time

    // Prefabs
    public Card CardRef; // To create new cards

    // Play Vars
    public bool playActive = false;
    [SerializeField] public List<ScoreData> score; // Score from the completed paintings
                            // Win/Loss -> Not implemented yet

    // Deck Vars
    public int DeckSize => deck.Count; // Is needed?
    public List<Card> deck = new();
    
    private Queue<Card> dealBuffer = new();
    public bool isProcessingDealBuffer = false;
    public float timeBetweenDeals;
    public float dealTime;
    public Ease dealEase;

    public Vector3 drawFocusPos = new(-6, 1.5f, 0); // World location offset -> prepare for trouble (consider converting to live read from the deck position in future -> must convert world to local to understand position relative to changing parent)

    // Hand Vars
    public int handLimit;
    public int manaLimit;
    public int manaCount;

    private readonly List<Card> hand = new();
    public float handSpread = 1f;
    public float toHandTime = .2f;
    public Ease toHandEase = Ease.InQuad;

    public Vector3 handHome = new(0, -3, 0);

    // Graveyard Vars
    public List<Card> graveyard = new();

    // Runtime
    void Start()
    {
        StartPlay(60, 6, 2);
    }

    void Update()
    {
        if (playActive) // Allows pausing and other operations
        {
            // Deck Updates
            

            // Hand Updates
            // Triggered only by turn -> under reconstruction
                // UpdateHand();
        }
    }

    // Public Methods
    public void StartPlay(int deckCount, int handLimit, int manaLimit)
    {
        // Create and shuffle the deck
        GenDeck(deckCount);
        deck.Shuffle();

        this.handLimit = handLimit;
        DrawHand();

        this.manaLimit = manaLimit;
        RefreshMana();

        playActive = true; // Signifies the setup tasks are complete -> animations will still be enqueued
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
            nCard.value = 0; // Made safe with introduction of ValueDigitizer
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
            dCard.spriteRenderer.sortingLayerName = "Hand";
            
            yield return new WaitForSeconds(timeBetweenDeals);
        }

        isProcessingDealBuffer = false;
    }

    private IEnumerator SetCardValues()
    {
        // This coroutine waits until all cards are dealt before setting their values
        while (isProcessingDealBuffer || dealBuffer.Count > 0)
        {
            yield return null; // Wait for the next frame
        }

        if (hand.Count != handLimit)
        {
            Debug.LogWarning("Hand is not full when setting card values."); // Debug check to ensure the coroutine is waiting properly
        }

        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].value = handLimit - i;
        }
    }

    // Hand Methods
    public void DrawHand() // Draw if possible -> automatic adjustment of hand positions triggered when a valid card is dealt to the hand
    {
        // Check and fill hand with cards up to the hand limit (ensure to check number of cards currently in the deal buffer and cards available in the deck)
        while ((deck.Count > 0) && (hand.Count + dealBuffer.Count) < handLimit)
        {
            Card dCard = deck.Pop();
            dCard.gameObject.SetActive(true);
            Deal(dCard);
        }
        StartCoroutine(SetCardValues());
    }

    private void AddCardToHand(Card nCard) // Adds a single card to the hand
    {
        hand.Add(nCard);
        UpdateCardPosInHand();
    }

    public void RemoveCardFromHand(Card rCard) // Discards a single card from the hand
    {
        hand.Remove(rCard);
        UpdateCardPosInHand();
    }

    private void UpdateCardPosInHand() // Updates card home and queues the home destination if it is not set
    {
        for (int i = 0; i < hand.Count; i++)
        {
            float xOffset = (i - (hand.Count - 1) * 0.5f) * handSpread;
            Vector3 offsetPos = new (-xOffset, 0, 0);

            // Update render order if it changed
            if (hand[i].cardSortOrder != i + 1)
            {
                hand[i].cardSortOrder = i + 1;
            }
            // If the card home is not set update it
            if (hand[i].cardHome != offsetPos)
            {
                hand[i].cardHome = offsetPos;
            }
            // Check the buffer for queued movements and confirm the home movement is not pending
            if (hand[i].destinationsBuffer.Count > 0)
            {
                if (offsetPos != hand[i].destinationsBuffer.Peek().pos) // Add a destination if card is not already headed to the destination as it's last stop (only checks if the destination is queued not if the card is resting at the location)
                {
                    hand[i].destinationsBuffer.Enqueue((offsetPos, toHandTime, toHandEase));
                }
            }
            else if (hand[i].transform.localPosition != offsetPos) // Confirms the card is not at the intended home and sends it there if it is currently at rest
            {
                hand[i].destinationsBuffer.Enqueue((offsetPos, toHandTime, toHandEase));
            }
        }
    }

    // Mana Methods
    public void UseMana() // Burns a mana for card action (future home for triggering more complex associated animations -> current dirty simple implementation) -> (consider bool return for checking for available mana in a conditional attempt from other sources - research if that is a professional application)
    {
        manaCount--;
        UpdateMana();
    }

    public void RefreshMana()
    {
        manaCount = manaLimit;
        UpdateMana();
    }

    public void UpdateMana() // Rough outline of public trigger for updating the Mana Animation State (will be a private method in future refactor of Mana handler object)
    {
        if (manaCount == 0)
        {
            mana1.spriteRenderer.color = new(1f, 1f, 1f, .3f);
            mana2.spriteRenderer.color = new(1f, 1f, 1f, .3f);
        }
        else if (manaCount == 1)
        {
            mana1.spriteRenderer.color = new(1f, 1f, 1f, 1f);
            mana2.spriteRenderer.color = new(1f, 1f, 1f, .3f);
        }
        else
        {
            mana1.spriteRenderer.color = new(1f, 1f, 1f, 1f);
            mana2.spriteRenderer.color = new(1f, 1f, 1f, 1f);
        }
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

    public static T Pop<T>(this List<T> list) // Pops off an element from a list like in Python
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
