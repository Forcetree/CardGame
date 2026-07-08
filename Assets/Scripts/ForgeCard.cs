using DG.Tweening;
using UnityEngine;

public class ForgeCard : Card
{
    public enum ForgeCardTypes
    {
        //The 9 base materials
        ironSprite,
        copperSprite,
        nickelSprite,
        sulfurSprite,
        carbonSprite,
        waterSprite,
        voiddustSprite,
        moonsaltSprite,
        quicksilverSprite,

        //Pure Metals
        bronzeSprite, // Iron Copper 10
        leadSprite,   // Iron Nickel 11
        brassSprite,  // Copper Nickel 12

        // Alchemical Metal Combos

        gunmetalSprite,  // Iron Sulfur 13
        steelSprite,     // Iron Carbon 14 
        rustSprite,      // Iron Water  15
        blackslagSprite, // Copper Sulfur  16
        yellowslagSprite,// Copper Carbon  17
        greenslagSprite, // Copper Water   18
        ashSprite,       // Nickel Sulfur  19
        titaniumSprite,  // Nickel Carbon  20 
        nickelplateSprite,// Nickel Water  21

        // Alchemical non-metal combos

        explosionSprite, // Sulfur Carbon 22
        acidSprite,      // Sulfur Water  23
        sootSprite,      // Carbon Water  24

        // Magic Metals  

        voidsteelSprite, // Iron Voiddust 25
        silverSprite,    // Iron Moonsalt 26
        inertiumSprite,  // Iron Quicksilver 27
        lithiumSprite,   // Copper Voiddust 28
        verdigrisSprite, // Copper Moonsalt 29    
        chromiumSprite,  // Copper Quicksilver 30
        sandSprite,      // Nickel Voiddust 31
        platinumSprite,  // Nickel Moonsalt 32
        mirrorSprite,    // Nickel Quicksilver 33

        // Alchemical Magic
        gunpowderSprite, // Sulfur Voiddust 34
        iridiumSprite,   // Sulfur Moonsalt 35
        lavastoneSprite, // Sulfur Quicksilver 36
        gloomcrumbSprite,// Carbon Voiddust 37
        gleamtalcSprite, // Carbon Moonsalt 38
        pewterSprite,    // Carbon Quicksilver 39
        siltSprite,      // Water Voiddust 40
        manatearsSprite, // Water Moonsalt 41
        vaporSprite,     // Water Quicksilver 42

        // Pure Magic
        nothingSprite,   // Voiddust Moonsalt 43
        blacksludgeSprite,  // Voiddust Quicksilver 44
        glassSprite,     // Moonsalt Quicksilver 45

    }

    public ForgeCardTypes mySpecificType;

    public override int CardTypeID
    {
        get => (int)mySpecificType;
        set
        {
            mySpecificType = (ForgeCardTypes)value;
            SetSprite();
        }
    }

    public override string CardTypeName => mySpecificType.ToString();

    public override void SetSprite()
    {
        spriteRenderer.sprite = CardCombiner.GetVisual<Sprite>(CardTypeID);
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
