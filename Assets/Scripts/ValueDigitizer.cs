using UnityEngine;

public class ValueDigitizer : MonoBehaviour
{
    // Class handles conversion of integer values to digit sprites for display on UI elements. Should be capable of handling up to 3 digits currently.
    public Sprite[] digitSprites; // Array of digit sprites from 0-9 (should we create a better method to manage this?)

    public SpriteRenderer hundredsRenderer;
    public SpriteRenderer tensRenderer;
    public SpriteRenderer unitsRenderer;

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

    public void UpdateRenderSorting()
    {
        SpriteRenderer parentRenderer = this.transform.parent.GetComponent<SpriteRenderer>(); // Accesses the parent object's SpriteRenderer to align sorting layers and orders (this is simple for Card objects but needs consideration for field)

        hundredsRenderer.sortingLayerName = parentRenderer.sortingLayerName;
        tensRenderer.sortingLayerName = parentRenderer.sortingLayerName;
        unitsRenderer.sortingLayerName = parentRenderer.sortingLayerName;

        hundredsRenderer.sortingOrder = parentRenderer.sortingOrder + 1;
        tensRenderer.sortingOrder = parentRenderer.sortingOrder + 1;
        unitsRenderer.sortingOrder = parentRenderer.sortingOrder + 1;
    }

    private void UpdateDigits() // Consider adding animation effects later
    {
        if (currentValue == 0)
        {
            hundredsRenderer.enabled = false;
            tensRenderer.enabled = false;
            unitsRenderer.enabled = false;
            return;
        }

        int hundreds = currentValue / 100;
        int tens = (currentValue / 10) % 10;
        int units = currentValue % 10;

        // Hundreds: show only when > 0
        if (hundreds > 0)
        {
            hundredsRenderer.enabled = true;
            hundredsRenderer.sprite = digitSprites[hundreds];
        }
        else
        {
            hundredsRenderer.enabled = false;
            hundredsRenderer.sprite = null;
        }

        // Tens: show when hundreds > 0 or tens > 0
        if (hundreds > 0 || tens > 0)
        {
            tensRenderer.enabled = true;
            tensRenderer.sprite = digitSprites[tens];
        }
        else
        {
            tensRenderer.enabled = false;
            tensRenderer.sprite = null;
        }

        // Units: always show for non-zero values
        unitsRenderer.enabled = true;
        unitsRenderer.sprite = digitSprites[units];
    }
}
