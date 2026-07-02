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
                .Append(TopperRenderer.DOColor(new Color(1f, 1f, 1f, 0f), 0.5f).SetEase(Ease.OutExpo));
        }
        else if(stack.Count == 1) // Play Animation for the first card in the stack
        {
            DOTween.Sequence()
                // Fade in?
                .Append(Topper.transform.DOPunchScale(new Vector3(0.05f, 0.05f, 1), 0.3f, 1, 0)) // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
                .Join(TopperRenderer.DOColor(comboColor, 0.1f).SetEase(Ease.InSine)); // Change to combo color



        }
        else // Combo Animation
        {
            DOTween.Sequence()
                .Append(TopperRenderer.DOColor(Color.white, 0.05f).SetEase(Ease.Flash)) // Flash bright
                .Join(TopperRenderer.DOColor(comboColor, 0.1f).SetEase(Ease.InSine)) // Change to combo color
                .Join(Topper.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.1f, 1, 0)); // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
        }
        
    }
}

