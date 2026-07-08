using UnityEngine;

public class NecroCombiner : CombinerLogic<Sprite>
{
    public override void SetupCombinations()
    {
        NecroCardSpriteAssetHandler NecroSprites = GameObject.Find("CardAssets").GetComponent<NecroCardSpriteAssetHandler>();

        AddCombo(new[] { 0 }, 0);
        AddCombo(new[] { 1 }, 1);
        AddCombo(new[] { 2 }, 2);
        AddCombo(new[] { 3 }, 3);
        AddCombo(new[] { 4 }, 4);
        AddCombo(new[] { 5 }, 5);
        AddCombo(new[] { 6 }, 6);
        AddCombo(new[] { 7 }, 7);
        AddCombo(new[] { 8 }, 8);
        AddCombo(new[] { 9 }, 9);

        // Add Sprites to the Combiner Logic
        VisualMatrix[0] = NecroSprites.fireSprite;
        VisualMatrix[1] = NecroSprites.growthSprite;
        VisualMatrix[2] = NecroSprites.earthSprite;
        VisualMatrix[3] = NecroSprites.ironSprite;
        VisualMatrix[4] = NecroSprites.frostSprite;
        VisualMatrix[5] = NecroSprites.waterSprite;
        VisualMatrix[6] = NecroSprites.windSprite;
        VisualMatrix[7] = NecroSprites.stormSprite;
        VisualMatrix[8] = NecroSprites.blightSprite;
        VisualMatrix[9] = NecroSprites.backSprite;
    }
}
