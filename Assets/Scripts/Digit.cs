using DG.Tweening;
using UnityEngine;

public class Digit : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void UpdateSorting(string nLayer, int nOrder)
    {
        spriteRenderer.sortingLayerName = nLayer;
        spriteRenderer.sortingOrder = nOrder;
    }

    public void Fade()
    {
        if (spriteRenderer.enabled)
        {
            spriteRenderer.DOFade(0, .5f).OnComplete(() => 
            {
                spriteRenderer.enabled = false;
            });
        }
    }

    public void AnimateValueChange(Sprite nDigit) // Enforce 0 is not displayed here
    {        
        if (spriteRenderer.enabled)
        {
            spriteRenderer.DOFade(0, .25f).OnComplete(() => 
            {
                spriteRenderer.sprite = nDigit;
                spriteRenderer.DOFade(1, .25f);
            });
        }
        else
        {
            spriteRenderer.sprite = nDigit;
            spriteRenderer.enabled = true;
            spriteRenderer.DOFade(1, .25f).SetDelay(.25f);
        }
    }
}
