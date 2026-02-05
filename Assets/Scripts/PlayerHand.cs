using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHand : MonoBehaviour
{
    [Tooltip("The deck is set during active play by the PlayHandler")]
    public PaintDeck deck; // Needs Deck object to talk to -> filled on initialization of the hand in PlayHandler

    [Tooltip("Set automatically by PlayHandler: for hand size")]
    public int handLimit;

    [Tooltip("Readonly Field of Cards in the Hand")]
    private readonly List<Card> hand = new();

    [Tooltip("Set this for the spread between cards in the hand")]
    public float handSpread = 1f;
    [Tooltip("Set this for card fly speed to the hand")]
    public float toHandTime = .2f;
    [Tooltip("Set this for the animation type when the card snaps back to hand")]
    public Ease toHandEase = Ease.InQuad;

    [Tooltip("Set this for the location the hand lives")]
    public Vector3 handHome = new(0, -3, 0);

    public int Count => hand.Count;

    public Card this[int index] => hand[index]; // May not be used but allows for easier access to cards in hand with indexing

    public void InitHand(int handLimit, PaintDeck nDeck) // Do we want to allow this to be called multiple times for different playthroughs? If so we need to consider resetting the hand and other related variables
    {
        this.handLimit = handLimit;
        this.deck = nDeck;

        deck.DrawHand(this); // Seperate call to the deck to draw the hand after the hand is initialized to avoid potential issues with the deck trying to access the hand before it is ready?
    }

    public void AddCardToHand(Card nCard) // Adds a single card to the hand
    {
        hand.Add(nCard);
        nCard.transform.parent = this.transform;
        nCard.spriteRenderer.sortingLayerName = "Hand";
        UpdateCardPosInHand();
    }

    public void RemoveCardFromHand(Card rCard) // Discards a single card from the hand
    {
        hand.Remove(rCard);
        UpdateCardPosInHand();
    }

    public void AssignSequentialValues() // Assigns ordered list to the cards for values in hand
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].value = i + 1;
        }
    }

    public void UpdateCardPosInHand() // Updates card home and queues the home destination if it is not set
    {
        for (int i = 0; i < hand.Count; i++)
        {
            float xOffset = (i - (hand.Count - 1) * 0.5f) * handSpread;
            Vector3 offsetPos = new(-xOffset, 0, 0);

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

}
