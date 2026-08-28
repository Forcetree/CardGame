using UnityEngine;
using UnityEngine.EventSystems;

public class FinishPainting : MonoBehaviour, IPointerClickHandler
{
    // Scene References
    public PlayHandler PlayHandler;

    // No longer needed
    //public FieldMat fieldMatLeft;
    //public FieldMat fieldMatCenter;
    //public FieldMat fieldMatRight;

    // Components
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D myCollider;

    // Attributes
    public int turns = 0;

    public void OnPointerClick(PointerEventData eventData) // Dirty hook up to the finish button in the scene. This is not a good way to handle this, but it works for now.
    {
        // Dirty Fix: For Necro Field -> Super bad

        NecroPlaySpace myField = PlayHandler.NecroField;

        // Add score to the list from PlayHandler
        PlayHandler.score.Add(new(myField.RawValue, turns));
        turns = 0;
        myField.ClearField();

        
    }


    // Depricated in dirty hook up

    //private int CollectScore()
    //{
    //    return fieldMatLeft.value + fieldMatCenter.value + fieldMatRight.value;
    //}

    //private void ClearField()
    //{
    //    fieldMatLeft.ClearMat();
    //    fieldMatCenter.ClearMat();
    //    fieldMatRight.ClearMat();
    //}
}
