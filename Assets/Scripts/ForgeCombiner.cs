using UnityEngine;

public class ForgeCombiner : CombinerLogic<Sprite>
{
    public override void SetupCombinations()
    {
        ForgeSpriteAssetHandler ForgeSprites = GameObject.Find("CardAssets").GetComponent<ForgeSpriteAssetHandler>();

        AddCombo(new[] { 0,}, 0);
        AddCombo(new[] { 1,}, 1);
        AddCombo(new[] { 2,}, 2);
        AddCombo(new[] { 3,}, 3);
        AddCombo(new[] { 4,}, 4);
        AddCombo(new[] { 5,}, 5);
        AddCombo(new[] { 6,}, 6);
        AddCombo(new[] { 7,}, 7);
        AddCombo(new[] { 8,}, 8);
        
        AddCombo(new[] { 0, 1 }, 9);
        AddCombo(new[] { 0, 2 }, 10);
        AddCombo(new[] { 1, 2 }, 11);

        // --- Sprites ---
        VisualMatrix[0] = ForgeSprites.ironSprite;
        VisualMatrix[1] = ForgeSprites.copperSprite;
        VisualMatrix[2] = ForgeSprites.nickelSprite;
        VisualMatrix[3] = ForgeSprites.sulfurSprite;
        VisualMatrix[4] = ForgeSprites.carbonSprite;
        VisualMatrix[5] = ForgeSprites.waterSprite;
        VisualMatrix[6] = ForgeSprites.voiddustSprite;
        VisualMatrix[7] = ForgeSprites.moonsaltSprite;
        VisualMatrix[8] = ForgeSprites.quicksilverSprite;

        VisualMatrix[9] = ForgeSprites.bronzeSprite;
        VisualMatrix[10] = ForgeSprites.leadSprite;
        VisualMatrix[11] = ForgeSprites.brassSprite;


    }
}
