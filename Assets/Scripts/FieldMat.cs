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

    [SerializeField] private bool _matIsFull;
    public bool matIsFull
    {
        get { return _matIsFull; } 
        
        set  // There is no room for an animation in this integration
        {
            _matIsFull = value;
            Topper.SetActive(_matIsFull); // Show the Topper renderer when the mat is full
        }
    }

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


        // comboType = CardCombiner.TryResolve(stack.Select(c => c.CardTypeID).Distinct(), out int resolved) ? resolved : -1; // Short compressed code: Default type listed as -1 if no combo is found, otherwise resolved type is returned
        var types = stack.Select(c => c.CardTypeID).Distinct();
        CardCombiner.TryResolve(types, out int resolved);
        comboType = resolved;

        // PlayAnimation(); // Can independantly determine the animation to play based on the comboType and the current stack of cards on the mat. This will allow for more dynamic animations based on the combo type and the number of cards in the stack.
        PlayComboAnimation(); 

        for (int i = 0; i < stack.Count; i++) // Fix the card sorting on the layer
        {
            stack[i].cardSortOrder = i; // Using our controlled order to set sorting order
            valueRenderer.UpdateRenderSorting();
        }

        highlighted = false;
        matIsFull = true;  
    }

    protected abstract void PlayAnimation();

    private void PlayComboAnimation() // We currently only have one sprite location and swap between field use and topper (why should we not do a bottom and cover it with the topper when needed?
    {
        // TopperRenderer.color = new Color(1, 1, 1, 0); // Reset the color to white with full opacity before starting the animation

        DG.Tweening.Sequence s = DOTween.Sequence();
                
        s.Append(TopperRenderer.DOColor(Color.white, 0.05f).SetEase(Ease.Flash)); // Flash bright

        Color comboColor = CardCombiner.GetVisual<Color>(comboType);
        s.Join(TopperRenderer.DOColor(comboColor, 0.1f).SetEase(Ease.InSine)); // Change to combo color

        s.Join(Topper.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.1f, 1, 0)); // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
    }

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

        // PlayAnimation(); // Can determine that the mat was cleared

        //valueRenderer.UpdateRenderSorting();  // Only required because we must reset the layer on the single sprite renderer that is used for both the topper and value renderer (we should consider separating these into two objects to avoid this issue)
    }

    
}
