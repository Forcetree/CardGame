using DG.Tweening;
using UnityEngine;

public class PaintExampleCard : Card
{
    public enum PaintCardTypes 
    {
        Red,
        Yellow,
        Blue,
        White,
        Black,

        // Combos made on the board
        // Light Pures
        Coral, // Red + White
        Sun, // Yellow + White
        Hydro, // Blue + White

        // Dark Pures
        Blood, // Red + Black
        Gold, // Yellow + Black
        Abyss, // Blue + Black

        // Mixes
        Toxic, // -> Purple
        Amber, // -> Orange
        Life, // -> Green

        // Light Combos
        Iris, // Purple + White
        Nectar, // Orange + White
        Moss, // Green + White

        // Dark Combos
        Obsidian, // Purple + Black
        Lava, // Orange + Black
        Serpenite, // Green + Black

        Back // Card back when flipped
    }

    public PaintCardTypes mySpecificType;

    public override int CardTypeID
    {
        get => (int)mySpecificType;
        set
        {
            mySpecificType = (PaintCardTypes)value;
            SetSprite();
        }
    }

    // Should the sprites live here?
    // Does a live instance accept <List<Sprite>>

    public override string CardTypeName => mySpecificType.ToString();

    public override void SetSprite()
    {
        spriteRenderer.color = CardCombiner.GetVisual<Color>(CardTypeID);
    }
    protected override void PlayAnimation()
    {   // Add a wiggle animation to the card when it is thrown back to the hand for some satisfying feedback -> under review for how to implement this with the current mover system (potentially just add a rotation tween before the return home)

        DOTween.Sequence()
               .Append(transform.DOPunchRotation(new Vector3(0, 0, wiggle), 0.3f, 15, 1)) // Adjust the punch rotation parameters as needed (vector size adjustment, time, vibrato, elasticity)
               .Join(transform.DOPunchPosition(new Vector3(0.1f, 0, 0), 0.3f, 10, 1))
               .Join(spriteRenderer.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo)) // Currently can't override the color as we set that in the update loop -> need to create better handling for sprite color without polling
               .OnComplete(() =>
               {
                   destinationsBuffer.Enqueue((cardHome, deHoverTime, deHoverEase)); // Currently using the same dehome parameters unless we want custom ones for a different feel when dropping a card
                   spriteRenderer.sortingLayerName = "Hand";
                   ValueRenderer.UpdateRenderSorting();
               });
            //For other animations like idle could we leverage the CardState? check if card is in hand and how long it has been in hand? or how long it has been on field? 

    }

}
