using UnityEngine;
using DG.Tweening;


public class PaintMat : FieldMat
{
    protected override void PlayAnimation()
    {
        if (stack.Count == 0)
        {
            // Play the empty mat animation -> not implemented yet
        }
        else if(stack.Count == 1)
        {
            // Play the Appear animation -> not implemented yet
        }
        else //stack.Count always greater than 1
        {
            DG.Tweening.Sequence s = DOTween.Sequence();

            s.Append(TopperRenderer.DOColor(Color.white, 0.05f).SetEase(Ease.Flash)); // Flash bright

            Color comboColor = CardCombiner.GetVisual<Color>(comboType);
            s.Join(TopperRenderer.DOColor(comboColor, 0.1f).SetEase(Ease.InSine)); // Change to combo color

            s.Join(Topper.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.1f, 1, 0)); // Add a punch scale for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
        }
        
    }
}

