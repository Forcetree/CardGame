using UnityEngine;
using UnityEngine.EventSystems;

public class TurnBtn : MonoBehaviour, IPointerClickHandler
{
    public PlayHandler PlayHandler;
    
    public SpriteRenderer spriteRenderer; // For future animation and other activities
    public Collider2D myCollider; // Not sure if I need this

    public FinishPainting finisher;

    // Currently not handling hover, down, and up states of the button (in introductory phase of feature)

    public void OnPointerClick(PointerEventData eventData)
    {
        // Handle mana and counts here
        PlayHandler.deck.DrawHand(PlayHandler.hand);
        PlayHandler.RefreshMana();
        finisher.turns += 1;
        // Need to check deck state?
        // The deck can empty but actioning an empty deck to trigger end of season should be manual selection by player to finish current hand
        // -> may need to introduce either a button for moving to the next season or some logical check to detect the end of potential play (this will reduce player choice to intentionally bif paintings or do other silly actions)
    }
}
