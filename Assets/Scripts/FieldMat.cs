using UnityEngine;
using System.Collections.Generic;

public class FieldMat : MonoBehaviour
{
    // Sprite Refs
    public Sprite basicCard;
    public Sprite highlightCard;

    // Components
    public SpriteRenderer spriteRenderer;

    // Attributes
    public bool highlighted = false;
    public List<Card> stack = new();

    public void Awake()
    {
        CardCombiner.InitMatrix();
    }

    public bool TryTarget(Card cCard)
    {
        // Current debug step to check only if the mat is currently empty (expand in the future to check against the card and stack for combos)
        if (stack.Count == 0) 
        {
            return cCard.type != Card.cardType.Black && cCard.type != Card.cardType.White;
        }
        else // We do not care how many cards are in the stack per se (we confirmed the stack is not empty)
        {
            return CardCombiner.TryCombo(stack[^1].type, cCard.type, out _); // Discard the out as we do not need it
        }
    }

    public void TargetStatus(bool value) // Should I make this public to call from card or should we call internally if TryTarget allows targeting OR should I just leave the highlight public and allow it to be handled by cards
    {
        highlighted = value;   
    }

    public void AddToStack(Card nCard)
    {
        stack.Add(nCard);
        nCard.dragLock = true;

        // Combo Effect?
        if (stack.Count > 1) 
        {
            CardCombiner.TryCombo(stack[^2].type, stack[^1].type, out Card.cardType comboType); // This is agnostic of the incoming card as that card was already added to the stack (left this way for future if we want a special combine animation after the drop)
            nCard.type = comboType;
        }

        for (int i = 0; i < stack.Count; i++) // Fix the card sorting on the layer
        {
            stack[i].spriteRenderer.sortingOrder = i;
        }
    }

    void Update()
    {
        if (highlighted)
        {
            spriteRenderer.sprite = highlightCard;
        }
        else
        {
            spriteRenderer.sprite = basicCard;
        }
    }
}
