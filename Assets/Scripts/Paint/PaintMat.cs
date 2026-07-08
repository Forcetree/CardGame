using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PaintMat : FieldMat
{
    protected override void PlayAnimation() // Overhauling for Animator controller additions 
    {
        Color comboColor = CardCombiner.GetVisual<Color>(comboType);

        if (stack.Count == 0) // Clear Animation
        {
            DOTween.Sequence()
                .Append(TopperRenderer.DOColor(new Color(1f, 1f, 1f, 0f), 0.5f).SetEase(Ease.OutBack)) // Fade out the topper
                .OnComplete(() => TopperMatAnimator.enabled = false); // Deactivate the topper animator after the fade-out is complete
        }
        else if(stack.Count == 1) // Play Animation for the first card in the stack
        {
            TopperMatAnimator.enabled = true; // Activate the topper animator for the first card in the stack
            TopperMatAnimator.Play("Combo", 0, 0); // Play the animation from the beginning controller state (0) at time 0 with a normalized time of 0

            DOTween.Sequence()
                .Append(Topper.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 1), 0.3f, 1, 0)) // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
                .Join(TopperRenderer.DOColor(comboColor, 0.3f).SetEase(Ease.InSine)); // Change to combo color (also unfades the topper because it was faded out)
        }
        else // Combo Animation
        {
            DOTween.Sequence()
                .Append(TopperRenderer.DOColor(comboColor, 0.3f).SetEase(Ease.InExpo)) // Change to combo color
                .Join(Topper.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.3f, 1, 0)); // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
        }
    }

    public override bool TryTarget(Card cCard) // Location for additional rules to enforce on a mat beyond valid combos. Returns true if the card can be played on this mat, false otherwise.
    {
        if (stack.Count >= 3) return false; // Paint Mat can only hold 3 cards at a time

        var prospective = stack.Select(c => c.CardTypeID).Append(cCard.CardTypeID).Distinct(); // Create a list of the available card types, including the new card, and remove duplicates

        return CardCombiner.TryResolve(prospective, out _); // Returns if comb is valid and discards the resolved type since we don't need it here. If the prospective list is invalid, this will return false.
    }
}

