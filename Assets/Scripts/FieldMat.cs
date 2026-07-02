using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FieldMat : MonoBehaviour
{
    // Objects
    public GameObject Topper;
    
    // Sprite Refs
    public Sprite basicCard;
    public Sprite highlight;

    // Components
    public SpriteRenderer TopperRenderer;
    public SpriteRenderer BottomRenderer;
    public SpriteRenderer HighlightRenderer;

    public ValueDigitizer valueRenderer;

    // Attributes
    [SerializeField] private bool _highlighted;
    public bool highlighted
    {
        get { return _highlighted; }
        set
        {
            _highlighted = value;
            
            if (_highlighted)
            {
                HighlightRenderer.sprite = highlight;
            }
            else
            {
                HighlightRenderer.sprite = null;
            }
        }
    }

    
    

    public List<Card> stack = new();

    public int comboType;

    public bool matIsFull = false;
    

    // Value Digitizer Controller
    [SerializeField] private int _value;
    public int value
    {
        get { return _value; }
        set
        {
            _value = value;
            valueRenderer.value = _value;
        }
    }

    public bool TryTarget(Card cCard) // Under reconstruction -> need to check against current stack for possible combos to exapnd the combos to support any number of duplicate cards
    {
        // New logic under construction
        var prospective = stack.Select(c => c.CardTypeID).Append(cCard.CardTypeID).Distinct();
        if (prospective.Count() > 3) return false;
        return CardCombiner.TryResolve(prospective, out _);
    }

    public void TargetStatus(bool value) // Should I make this public to call from card or should we call internally if TryTarget allows targeting OR should I just leave the highlight public and allow it to be handled by cards
    {
        highlighted = value;   
    }

    public void AddToStack(Card nCard)
    {
        stack.Add(nCard);
        nCard.transform.SetParent(this.transform);
        nCard.dragLock = true;

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
