using UnityEngine;
using UnityEngine.EventSystems;

public class FinishPainting : MonoBehaviour, IPointerClickHandler
{
    // Scene References
    public PlayHandler PlayHandler;

    public FieldMat fieldMatLeft;
    public FieldMat fieldMatCenter;
    public FieldMat fieldMatRight;

    // Components
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D myCollider;

    // Attributes
    public int turns = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Add score to the list from PlayHandler
        PlayHandler.score.Add(new(CollectScore(), turns));
        turns = 0;
        ClearField();
    }

    private int CollectScore()
    {
        return fieldMatLeft.value + fieldMatCenter.value + fieldMatRight.value;
    }

    private void ClearField()
    {
        fieldMatLeft.ClearMat();
        fieldMatCenter.ClearMat();
        fieldMatRight.ClearMat();
    }
}
