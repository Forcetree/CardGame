using UnityEngine;
using DG.Tweening;

public class ValueDigitizer : MonoBehaviour
{
    // Class handles conversion of integer values to digit sprites for display on UI elements. Should be capable of handling up to 3 digits currently.
    public Sprite[] digitSprites; // Array of digit sprites from 0-9 (should we create a better method to manage this?)

    public Digit hundredsRenderer;
    public Digit tensRenderer;
    public Digit unitsRenderer;

    private int currentValue = 0;
    public int value
    {
        get => currentValue;
        set
        {
            if(currentValue < 0 || currentValue > 999)
            {
                Debug.LogError("ValueDigitizer: Value out of range (0-999)");
                return;
            }
            currentValue = value;
            UpdateDigits();            
        }
    }

    [Tooltip("Alignment of the digits")]
    public Alignment alignment = Alignment.Right; // Future feature for aligning the digits (left, center, right)
    public enum Alignment
    {
        Left,
        Center,
        Right
    }
    [Tooltip("Space between digit centers, can be adjusted for better visual appeal")]
    public float digitSpacing = 0.35f; // Space between digits, can be adjusted for better visual appeal

    public void UpdateRenderSorting()
    {
        SpriteRenderer parentRenderer = this.transform.parent.GetComponent<SpriteRenderer>(); // Accesses the parent object's SpriteRenderer to align sorting layers and orders (this is simple for Card objects but needs more consideration for field assets)

        hundredsRenderer.UpdateSorting(parentRenderer.sortingLayerName, parentRenderer.sortingOrder + 1);
        tensRenderer.UpdateSorting(parentRenderer.sortingLayerName, parentRenderer.sortingOrder + 1);
        unitsRenderer.UpdateSorting(parentRenderer.sortingLayerName, parentRenderer.sortingOrder + 1);
    }

    private void UpdateDigits() // Adding Animations
    {
        if (currentValue == 0)
        {
            hundredsRenderer.Fade();
            tensRenderer.Fade();
            unitsRenderer.Fade();
            return;
        }

        int hundreds = currentValue / 100;
        int tens = (currentValue / 10) % 10;
        int units = currentValue % 10;

        unitsRenderer.AnimateValueChange(digitSprites[units]);
        int digitCount = 1;

        if (hundreds > 0 || tens > 0)
        {
            tensRenderer.AnimateValueChange(digitSprites[tens]);
            digitCount = 2;
        }
        else
        {
            tensRenderer.Fade();
        }

        if (hundreds > 0)
        {            
            hundredsRenderer.AnimateValueChange(digitSprites[hundreds]);
            digitCount = 3;
        }
        else
        {
            hundredsRenderer.Fade();
        }        

        UpdateAlignment(digitCount);
    }

    private void UpdateAlignment(int digitCount)
    {
        float s = digitSpacing;
                
        float groupStart;
        switch (alignment)
        {
            case Alignment.Left:
                groupStart = 0f;
                break;
            case Alignment.Center:
                groupStart = -s * (digitCount - 1) * 0.5f;
                break;
            case Alignment.Right:                
                groupStart = -s * (digitCount - 1);
                break;
            default:
                groupStart = -s * (digitCount - 1) * 0.5f;
                break;
        }

        Vector3 hPos, tPos, uPos;

        if (digitCount == 3)
        {
            hPos = new Vector3(groupStart + (0 * s), 0f, 0f);
            tPos = new Vector3(groupStart + (1 * s), 0f, 0f);
            uPos = new Vector3(groupStart + (2 * s), 0f, 0f);
        }
        else if (digitCount == 2)
        {            
            hPos = new Vector3(groupStart - (1 * s), 0f, 0f);
            tPos = new Vector3(groupStart + (0 * s), 0f, 0f);
            uPos = new Vector3(groupStart + (1 * s), 0f, 0f);
        }
        else // digitCount == 1
        {            
            hPos = new Vector3(groupStart - (2 * s), 0f, 0f);
            tPos = new Vector3(groupStart - (1 * s), 0f, 0f);
            uPos = new Vector3(groupStart + (0 * s), 0f, 0f);
        }

        hundredsRenderer.transform.DOLocalMove(hPos, 0.25f).SetEase(Ease.OutCubic);
        tensRenderer.transform.DOLocalMove(tPos, 0.25f).SetEase(Ease.OutCubic);
        unitsRenderer.transform.DOLocalMove(uPos, 0.25f).SetEase(Ease.OutCubic);

    }
}
