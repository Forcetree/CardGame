using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

public class NecroMat : FieldMat
{
    public int[] stackValues = new int[9]; // Distinct value sets for the stack -> total value by index (0: Fire, 1: Growth, 2: Earth, 3: Iron, 4: Frost, 5: Water, 6: Wind, 7: Storm, 8: Blight)
    
    protected override void PlayAnimation() // Overhauling for Animator controller additions 
    {
        Sprite cardSprite = CardCombiner.GetVisual<Sprite>(comboType);

        if (stack.Count == 0) // Clear Animation
        {
            DOTween.Sequence()
                .Append(TopperRenderer.DOColor(new Color(1f, 1f, 1f, 0f), 0.5f).SetEase(Ease.OutBack)) // Fade out the topper
                .OnComplete(() => TopperMatAnimator.enabled = false); // Deactivate the topper animator after the fade-out is complete
        }
        else if (stack.Count == 1) // Play Animation for the first card in the stack
        {
            TopperMatAnimator.enabled = true; // Activate the topper animator for the first card in the stack
            // TopperMatAnimator.Play("Combo", 0, 0); // Play the animation from the beginning controller state (0) at time 0 with a normalized time of 0

            DOTween.Sequence()
                .Append(Topper.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 1), 0.3f, 1, 0)) // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
                .Join(TopperRenderer.DOColor(Color.white, 0.3f).SetEase(Ease.InSine)); // To fade back in the card
            
            // Set the new sprite here
            TopperRenderer.sprite = cardSprite; // Set the new sprite for the topper renderer
        }
        else // Combo Animation
        {
            // Set Sprite for the topper renderer to the resolved combo sprite
            TopperRenderer.sprite = cardSprite;

            // Play the combo animation for the topper
            // No animation yet created

            DOTween.Sequence()
                .Append(Topper.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.3f, 1, 0)); // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
        }
    }

    public override bool TryTarget(Card cCard) // Location for additional rules to enforce on a mat beyond valid combos. Returns true if the card can be played on this mat, false otherwise.
    {
        return (stack.Select(c => c.CardTypeID).Append(cCard.CardTypeID).Distinct().Count() <= 3); // Returns if stack can take additional card based on distinct types in the stack. If the count is greater than 3, return false (cannot play), otherwise true (can play).
    }

    public override void AddToStack(Card nCard)
    {
        stack.Add(nCard);
        nCard.transform.SetParent(this.transform);
        nCard.state = Card.cardState.Field; // Set the card state to Field when it is placed on the mat

        nCard.dragLock = true; // Should change the destination buffer mover method to NOT unlock drag state if the card is in state = Field

        stackValues[nCard.CardTypeID] += nCard.value; // Add the value of the incoming card to the corresponding index in the stackValues array based on its CardTypeID
        value = stackValues.Sum();
         
        if (stack.Count == 1) { comboType = nCard.CardTypeID; } // If this is the first card in the stack, set the comboType to the CardTypeID of the incoming card
        if (stackValues[(int)comboType] < stackValues.Max()) { comboType = Array.IndexOf(stackValues, stackValues.Max()); } // Update the comboType to the index of the maximum value in stackValues if the current comboType's value is less than the maximum value in stackValues

        PlayAnimation(); // Can independantly determine the animation to play based on the comboType and the current stack of cards on the mat. This will allow for more dynamic animations based on the combo type and the number of cards in the stack.

        for (int i = 0; i < stack.Count; i++) // Fix the card sorting on the layer
        {
            stack[i].cardSortOrder = i; // Using our controlled order to set sorting order
            valueRenderer.UpdateRenderSorting();
        }

        highlighted = false;
        matIsFull = true;
    }

    public NecroDamage CalculateDamage()
    {
        //if (stack.Count == 0) return new NecroDamage { damage = 0, type = NecroCard.NecroCardTypes.None }; // What if the mat across from enemy is empty -> Carry over damage to player

        var types = stack.Select(c => c.CardTypeID).Distinct(); // Distinct types in the list

        CardCombiner.TryResolve(types, out int resolvedType);
        int totalDamage = stack.Sum(c => c.value);
        return new NecroDamage { damage = totalDamage, type = (NecroCard.NecroCardTypes)resolvedType };
    }
}
