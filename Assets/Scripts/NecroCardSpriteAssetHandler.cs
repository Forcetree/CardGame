using UnityEngine;

public class NecroCardSpriteAssetHandler : MonoBehaviour
{

    [Header("Assign Sprites Here")]
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite growthSprite;
    [SerializeField] private Sprite earthSprite;
    [SerializeField] private Sprite ironSprite;
    [SerializeField] private Sprite frostSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite windSprite;
    [SerializeField] private Sprite stormSprite;
    [SerializeField] private Sprite blightSprite;
    [SerializeField] private Sprite backSprite;

    private void Awake()
    {
        CardCombiner.fireSprite = fireSprite;
        CardCombiner.growthSprite = growthSprite;
        CardCombiner.earthSprite = earthSprite;
        CardCombiner.ironSprite = ironSprite;
        CardCombiner.frostSprite = frostSprite;
        CardCombiner.waterSprite = waterSprite;
        CardCombiner.windSprite = windSprite;
        CardCombiner.stormSprite = stormSprite;
        CardCombiner.blightSprite = blightSprite;
        CardCombiner.backSprite = backSprite;
    }
}
