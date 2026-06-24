using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FieldMat : MonoBehaviour
{
    // Objects
    public GameObject Topper;
    public SpriteRenderer TopperRenderer;

    // Sprite Refs
    public Sprite basicCard;
    public Sprite highlightCard;

    // Components
    public SpriteRenderer spriteRenderer;
    public ValueDigitizer valueRenderer;

    // Attributes
    public bool highlighted = false;
    public List<Card> stack = new();

    public Card.cardType comboType;

    public bool matIsEmpty = true;

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

    public void Awake()
    {
        CardCombiner.InitMatrix();
        matIsEmpty = true;
        spriteRenderer.color = new(1f, 1f, 1f, .3f);
    }

    //public bool TryTarget(Card cCard) // Under reconstruction -> For Necro the card combiner class needs to do many differnt things moving forward
    //{
    //    // New logic under construction
    //    var prospective = stack.Select(c => c.type).Append(cCard.type).Distinct();
    //    if (prospective.Count() > 3) return false;
    //    return CardCombiner.TryResolve(prospective, out _);
    //}

    public void TargetStatus(bool value) // Should I make this public to call from card or should we call internally if TryTarget allows targeting OR should I just leave the highlight public and allow it to be handled by cards
    {
        highlighted = value;   
    }

    public void AddToStack(Card nCard)
    {
        stack.Add(nCard);
        nCard.transform.SetParent(this.transform);
        
        // Safer to turn off the card
        // nCard.dragLock = true;
        nCard.gameObject.SetActive(false); // Hide the card when added to the mat

        value += nCard.value; // Update the mat value when adding a card

        var types = stack.Select(c => c.type).Distinct();
        //if (CardCombiner.TryResolve(types, out Card.cardType resolved))
        //{
        //    PlayComboAnimation(CardCombiner.GetSprite(resolved));            
        //    comboType = resolved;
        //}
        //else // This should never happen as TryTarget should prevent invalid cards from being added
        //{
        //    // Fall back: use the sprite for the lone card type
        //    TopperRenderer.sprite = CardCombiner.GetSprite(nCard.type);
        //    TopperRenderer.sortingLayerName = "Topper";
        //    valueRenderer.UpdateRenderSorting();

        //    comboType = nCard.type;
        //}

        for (int i = 0; i < stack.Count; i++) // Fix the card sorting on the layer
        {
            stack[i].cardSortOrder = i; // Using our controlled order to set sorting order
            valueRenderer.UpdateRenderSorting();
        }

        highlighted = false;
        matIsEmpty = false;
    }

    private void PlayComboAnimation(Sprite comboSprite)
    {
        TopperRenderer.sprite = comboSprite;
        TopperRenderer.sortingLayerName = "Topper";
        valueRenderer.UpdateRenderSorting();

        // Simple reveal animation: fade in the topper and add a punch scale
        TopperRenderer.color = new Color(1f, 1f, 1f, 0f);

        DG.Tweening.Sequence s = DOTween.Sequence();
        s.Append(TopperRenderer.DOFade(1f, 0.08f).SetEase(Ease.InSine));
        s.Join(Topper.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.1f, 1, 0));
    }

    public void ClearMat() // Need to expand this to combine and calculate the total value of the cards on the mat before clearing
    {
        foreach (var card in stack)
        {
            Destroy(card.gameObject); // Destroy the card objects when clearing the mat
        }
        
        stack.Clear();
        value = 0;

        matIsEmpty = true;
        spriteRenderer.color = new(1f, 1f, 1f, .3f);
        TopperRenderer.sortingLayerName = "Field";
        valueRenderer.UpdateRenderSorting();
    }

    // Need to find a better approach to live update the sprite state based on a mat state - under construction to refine how the mat displays and takes collisions (check card class for which layers it checks against (unless sorting layer is independant of physics which I think it is))
    void Update()
    {
        Topper.SetActive(!matIsEmpty);

        if (highlighted) // Look for a way to make this change occur when the operations run rather than check each update
        {
            spriteRenderer.sprite = highlightCard;
        }
        else
        {
            spriteRenderer.sprite = basicCard;
        }
    }
}
