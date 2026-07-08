using DG.Tweening;
using UnityEngine;

public class ManaPoint : MonoBehaviour
{
    public ParticleSystem pop_FX;

    public SpriteRenderer spriteRenderer;

    public bool active = false;
    public Vector3 manaHome;

    public void InitMana(Vector3 home) // Is this needed?
    {
        manaHome = home;
        active = true;
    }

    public void Fill()
    {
        this.gameObject.SetActive(true);

        transform.localScale = Vector3.zero; // Start from zero scale for the fill animation

        // Animate Fill
        DOTween.Sequence()
            .Append(spriteRenderer.DOFade(1f, .3f).SetEase(Ease.OutCubic))
            .Join(transform.DOScale(1f, .3f).SetEase(Ease.InBack))
            .OnComplete(() =>
            {
                active = true;
            });
    }

    public void Burn()
    {
        active = false;

        // Animate Burn
        DOTween.Sequence()
            .Append(transform.DOScale(1.2f, .10f).SetEase(Ease.OutBack))
            .Join(transform.DOPunchRotation(new Vector3(0, 0, 20), 0.10f, 20, 1)) // Add a punch rotation for a more dynamic effect (vector size adjustment, time, vibrato, elasticity)
            .Append(transform.DOScale(Vector3.zero, 0.05f).SetEase(Ease.InExpo))
            .Join(spriteRenderer.DOColor(Color.red, 0.05f))
            .Join(spriteRenderer.DOFade(0f, 0.05f))
            .AppendCallback(() =>
            {
                ParticleSystem pop = Instantiate(pop_FX, transform.position, Quaternion.identity); // Spawn the pop effect at the mana point's position
                pop.Play(); // Play the particle system
            })           
            .OnComplete(() =>
            {
                transform.localScale = Vector3.zero; // Reset scale for next time
                transform.rotation = Quaternion.identity; // Reset rotation for next time
                spriteRenderer.color = Color.white; // Reset color for next time

                active = false;

                this.gameObject.SetActive(false); // Deactivate the mana object after burning, can be reactivated and reused when filling again
            })
            .SetLink(gameObject);
    }
}
