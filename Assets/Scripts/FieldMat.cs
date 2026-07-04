using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FieldMat : MonoBehaviour
{
    // Objects
    public GameObject Topper;

    // Sprite Refs
    [SerializeField] protected Sprite _basicCardSprite;
    [SerializeField] protected Sprite _highlightSprite;

    // Components
    public SpriteRenderer TopperRenderer;
    public SpriteRenderer BottomRenderer;
    public SpriteRenderer HighlightRenderer;

    public Animator TopperMatAnimtor;

    public ValueDigitizer valueRenderer;

    // Attributes

    public List<Card> stack = new();

    public int comboType;
    public bool matIsFull = false;

    [SerializeField] private bool _highlighted;
    public bool highlighted // This is a public property that controls the highlighting of the mat. When set to true, it will display the highlight sprite; when set to false, it will remove the highlight sprite.
    {
        get { return _highlighted; }
        set
        {
            _highlighted = value;
            
            if (_highlighted)
            {
                HighlightRenderer.sprite = _highlightSprite;
            }
            else
            {
                HighlightRenderer.sprite = null;
            }
        }
    }

    [SerializeField] private int _value;
    public int value // Value Digitizer Controller
    {
        get { return _value; }
        set
        {
            _value = value;
            valueRenderer.value = _value;
        }
    }

    public abstract bool TryTarget(Card cCard); // Abstracting the rules for targeting a mat with a card. Each mat type will have its own rules for what cards can be placed on it.

    public void AddToStack(Card nCard)
    {
        stack.Add(nCard);
        nCard.transform.SetParent(this.transform);
        nCard.state = Card.cardState.Field; // Set the card state to Field when it is placed on the mat
        
        nCard.dragLock = true; // Should change the destination buffer mover method to NOT unlock drag state if the card is in state = Field

        value = stack.Sum(c => c.value);

        var types = stack.Select(c => c.CardTypeID).Distinct();
        CardCombiner.TryResolve(types, out int resolved);
        comboType = resolved;

        PlayAnimation(); // Can independantly determine the animation to play based on the comboType and the current stack of cards on the mat. This will allow for more dynamic animations based on the combo type and the number of cards in the stack.

        for (int i = 0; i < stack.Count; i++) // Fix the card sorting on the layer
        {
            stack[i].cardSortOrder = i; // Using our controlled order to set sorting order
            valueRenderer.UpdateRenderSorting();
        }

        highlighted = false;
        matIsFull = true;  
    }

    protected abstract void PlayAnimation();

    public void ClearMat() // No need to expand (we will process the data out of the stack before we clear it using the abstract and child class definitions)
    {
        foreach (var card in stack)
        {
            Destroy(card.gameObject); // Destroy the card objects when clearing the mat
        }
        
        stack.Clear();
        value = 0;
        comboType = -1; // Set to default type when clearing the mat -> should be invisible in future

        matIsFull = false;

        PlayAnimation(); // Can determine that the mat was cleared
    }

    
}
