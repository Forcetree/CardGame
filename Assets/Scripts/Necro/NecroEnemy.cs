using UnityEngine;

public class NecroEnemy : MonoBehaviour
{
    // Private Fields for the NecroEnemy class
    private int MaxHealth; // Maximum health of the enemy

    // Public Read-Only Fields for the NecroEnemy class
    public bool isAlive { get; private set; } // Check if the enemy is alive
    public int health { get; private set; } // Health of the enemy
    public int attack { get; private set; } // Attack power
    public NecroCard.NecroCardTypes type { get; private set; } // Type of the enemy(?) [dual types?]

    // Objects
    public SpriteRenderer spriteRenderer; // Sprite renderer for the enemy
    public Animator animator; // Animator for the enemy
    public SpriteRenderer portraitRenderer; // Framed Portrait renderer for the enemy

    // Public Methods for the NecroEnemy class
    public void Activate(int maxHealth, int attack, NecroCard.NecroCardTypes type)
    {
        this.isAlive = true;

        this.MaxHealth = maxHealth;
        this.health = maxHealth;
        this.attack = attack;
        this.type = type;
        // Set the sprite based on the type
        // SetSprite();

        this.gameObject.SetActive(true);
    }

    public void TakeDamage(NecroDamage damage)
    {
        // How best to handle this?
    }

    public void Attack(NecroMat stack)
    {
        // How are we going to handle the attack? We need to read the stack and adjust the values inside it based on the type of each entry
    }

    public void Kill()
    {
        this.isAlive = false;
        this.health = 0;

        this.gameObject.SetActive(false);
    }

}
