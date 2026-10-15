using DG.Tweening;
using System.Linq;
using UnityEngine;

public class NecroMat : FieldMat
{

    public int[,] stackValues = new int[3, 2]; // 2 distinct value sets for the stack -> type and total value (0: Fire, 1: Growth, 2: Earth, 3: Iron, 4: Frost, 5: Water, 6: Wind, 7: Storm, 8: Blight, 9: Empty)

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
        if (stack.Select(c => c.CardTypeID).Append(cCard.CardTypeID).Distinct().Count() > 3) return false; // Necro Mat can only hold 3 distinct card types at a time. If the stack is full, return false. Current set to 1 for debugging
        
        // Determine if the incoming type is already in the stack
        if (stack.Any(c => c.CardTypeID == cCard.CardTypeID))
        {
            
        }
        else
        {

        }

        var prospective = stack.Select(c => c.CardTypeID).Append(cCard.CardTypeID).Distinct(); // Create a list of the available card types, including the new card, and remove duplicates

        // Might completely skip the combiner logic if it does not support overloading the try target -> This is because we need to compare the total stack values to determine the highest type value and set the stack to represent that value.
        return CardCombiner.TryResolve(prospective, out _); // Returns if comb is valid and discards the resolved type since we don't need it here. If the prospective list is invalid, this will return false.
    }

    // New flow needed:
        /* Stack determines its type based on what is inside the stack (3 distinct value sets)
            * ~ Actual stack is a list of card objects, we need an interim object to parse the cards into value sets to determine the highest value type (and then set the stack to represent that value) public override bool TryGetTarget(Card cCard)?
            * Damage is calced against the intrim object of set values (not against the actual stack)
         * Based on type determine the damage value incoming
         * Method to take in the damage value and parse against the value set
        */

    public NecroDamage CalculateDamage()
    {
        //if (stack.Count == 0) return new NecroDamage { damage = 0, type = NecroCard.NecroCardTypes.None }; // What if the mat across from enemy is empty -> Carry over damage to player

        var types = stack.Select(c => c.CardTypeID).Distinct(); // Distinct types in the list

        CardCombiner.TryResolve(types, out int resolvedType);
        int totalDamage = stack.Sum(c => c.value);
        return new NecroDamage { damage = totalDamage, type = (NecroCard.NecroCardTypes)resolvedType };
    }
}
