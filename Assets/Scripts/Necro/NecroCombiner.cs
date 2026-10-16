using UnityEngine;

public class NecroCombiner : CombinerLogic<Sprite>
{
    public override void SetupCombinations()
    {
        NecroCardSpriteAssetHandler NecroSprites = Object.FindAnyObjectByType<NecroCardSpriteAssetHandler>();

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
