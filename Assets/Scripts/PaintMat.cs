using UnityEngine;
using DG.Tweening;


public class PaintMat : FieldMat
{
    protected override void PlayAnimation()
    {
        Color comboColor = CardCombiner.GetVisual<Color>(comboType);

        if (stack.Count == 0) // Clear Animation
        {
            DOTween.Sequence()
                .Append(TopperRenderer.DOColor(new Color(1f, 1f, 1f, 0f), 0.5f).SetEase(Ease.OutBack)); // Fade out the topper
        }
        else if(stack.Count == 1) // Play Animation for the first card in the stack
        {
            DOTween.Sequence()
                .Append(Topper.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 1), 0.3f, 1, 0)) // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
                .Join(TopperRenderer.DOColor(comboColor, 0.3f).SetEase(Ease.InSine)); // Change to combo color
        }
        else // Combo Animation
        {
            DOTween.Sequence()
                .Append(TopperRenderer.DOColor(comboColor, 0.3f).SetEase(Ease.InExpo)) // Change to combo color
                .Join(Topper.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.3f, 1, 0)); // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
        }
        
    }
}

