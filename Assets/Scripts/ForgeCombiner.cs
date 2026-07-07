using UnityEngine;

public class ForgeCombiner : CombinerLogic<Color>
{
    public override void SetupCombinations()
    {
        AddCombo(new[] { 0 }, 0);
        AddCombo(new[] { 2 }, 2);
        AddCombo(new[] { 1 }, 1);
        AddCombo(new[] { 3 }, 3);
        AddCombo(new[] { 4 }, 4);

        AddCombo(new[] { 0, 2 }, 11);
        AddCombo(new[] { 0, 1 }, 12);
        AddCombo(new[] { 2, 1 }, 13);

        AddCombo(new[] { 0, 3 }, 5);
        AddCombo(new[] { 2, 3 }, 7);
        AddCombo(new[] { 1, 3 }, 6);

        AddCombo(new[] { 0, 4 }, 8);
        AddCombo(new[] { 2, 4 }, 10);
        AddCombo(new[] { 1, 4 }, 9);

        AddCombo(new[] { 0, 2, 3 }, 14);
        AddCombo(new[] { 0, 2, 4 }, 17);

        AddCombo(new[] { 0, 1, 3 }, 15);
        AddCombo(new[] { 0, 1, 4 }, 18);

        AddCombo(new[] { 2, 1, 3 }, 16);
        AddCombo(new[] { 2, 1, 4 }, 19);

        // --- Colors ---
        VisualMatrix[0] = Color.red;
        VisualMatrix[2] = Color.blue;
        VisualMatrix[1] = Color.yellow;
        VisualMatrix[3] = Color.white;
        VisualMatrix[4] = new Color32(0x20, 0x20, 0x20, 0xFF); // Off Black Tweak

        VisualMatrix[11] = new Color32(0x53, 0x07, 0x5B, 0xFF);
        VisualMatrix[12] = new Color32(0xC7, 0x3F, 0x25, 0xFF);
        VisualMatrix[13] = new Color32(0x55, 0x85, 0x00, 0xFF);

        VisualMatrix[5] = new Color32(0xE3, 0x6F, 0x74, 0xFF);
        VisualMatrix[7] = new Color32(0x2C, 0x50, 0xDE, 0xFF);
        VisualMatrix[6] = new Color32(0xFF, 0xD4, 0x17, 0xFF);

        VisualMatrix[14] = new Color32(0x90, 0x5A, 0xFF, 0xFF);
        VisualMatrix[15] = new Color32(0xF0, 0xD9, 0x6C, 0xFF);
        VisualMatrix[16] = new Color32(0x3A, 0x52, 0x3B, 0xFF);

        VisualMatrix[8] = new Color32(0x3C, 0x07, 0x07, 0xFF);
        VisualMatrix[10] = new Color32(0x03, 0x1A, 0x2A, 0xFF);
        VisualMatrix[9] = new Color32(0xFF, 0xAB, 0x1D, 0xFF);

        VisualMatrix[17] = new Color32(0x17, 0x0E, 0x24, 0xFF);
        VisualMatrix[18] = new Color32(0x65, 0x15, 0x00, 0xFF);
        VisualMatrix[19] = new Color32(0x0B, 0x27, 0x00, 0xFF);
    }
}
