using UnityEngine;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;

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

    public bool TryTarget(Card card)
    {
        // Current debug step to check only if the mat is currently empty (expand in the future to check against the card and stack for combos)
        if (stack.Count == 0) 
        {
            return true; 
        }
        else { return false; }
    }

    public void TargetStatus(bool value) // Should I make this public to call from card or should we call internally if TryTarget allows targeting OR should I just leave the highlight public and allow it to be handled by cards
    {
        highlighted = value;   
    }

    public void AddToStack(Card card)
    {
        stack.Add(card);
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
