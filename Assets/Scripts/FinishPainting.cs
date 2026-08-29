using UnityEngine;
using UnityEngine.EventSystems;

public class FinishPainting : MonoBehaviour, IPointerClickHandler
{
    // Scene References
    public PlayHandler PlayHandler;

    // Components
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D myCollider;

    // Attributes
    public int turns = 0;

    public void OnPointerClick(PointerEventData eventData) // Dirty hook up to the finish button in the scene. This is not a good way to handle this, but it works for now.
    {
        // Dirty Fix: More Dynamic -> super wrong need a better handle for this

        /* ISSUE(?): In future we need to handle the score differently for each game mode. 
         * For now, we will just add the scores to the Playhandle generic score list. */

        switch (PlayHandler.currentGameMode)
        {
            case PlayHandler.GameMode.Necro:
                PlayHandler.score.Add(new(PlayHandler.NecroField.RawValue, turns));
                PlayHandler.NecroField.ClearField();
                break;
            case PlayHandler.GameMode.Forge:
                //PlayHandler.score.Add(new(PlayHandler.ForgeField.RawValue, turns));
                //PlayHandler.ForgeField.ClearField();
                break;
            case PlayHandler.GameMode.Paint:
                PlayHandler.score.Add(new(PlayHandler.PaintField.RawValue, turns));
                PlayHandler.PaintField.ClearField();
                break;
            default:
                Debug.LogWarning("FinishPainting: Unhandled game mode: " + PlayHandler.currentGameMode);
                break;
        }

        // Reset the game state for the next round
        turns = 0;        
    }
}
