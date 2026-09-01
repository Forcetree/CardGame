using UnityEngine;

public class NecroEnemy : MonoBehaviour
{
    // Private Fields for the NecroEnemy class
    private int MaxHealth; // Maximum health of the enemy

    // Public Read-Only Fields for the NecroEnemy class
    public bool isSpawned; // Check if the enemy is spawned}
    public bool isAlive; // Check if the enemy is alive
    public int health; // Health of the enemy
    public int attack; // Attack power
    public int turnCycle; // Turn cycle for the enemy
    public NecroCard.NecroCardTypes type; // Type of the enemy(?) [dual types?]

    // Objects
    public SpriteRenderer spriteRenderer; // Sprite renderer for the enemy
    public Animator animator; // Animator for the enemy
    public SpriteRenderer portraitRenderer; // Framed Portrait renderer for the enemy

    // Public Methods for the NecroEnemy class
    public void Initialize(int maxHealth, int attack, NecroCard.NecroCardTypes type, int turnCycle)
    {
        this.isSpawned = false;
        this.isAlive = true;

        this.MaxHealth = maxHealth;
        this.health = maxHealth;
        this.attack = attack;
        this.type = type;
        this.turnCycle = turnCycle;
        // Set the sprite based on the type
        // SetSprite();

        this.gameObject.SetActive(false);
    }

    public void Spawn()
    {        
        // Set the sprite based on the type
        // SetSprite();
        this.gameObject.SetActive(true);

        // Animate the enemy's entrance into the battle space
        this.isSpawned = true;
    }



    public void TakeDamage(NecroDamage damage)
    {
        // How best to handle this?
        // Need to determine how the stack will be handled, and how the damage will be applied to the enemy
    }

    public void Attack(NecroMat stack)
    {
        // How are we going to handle the attack? We need to read the stack and adjust the values inside it based on the type of each entry?
        // Need to finish designing this interaction to finalize the design of the attack method. This will be a key part of the game mechanics, and we need to ensure that it is balanced and fun for the player.
    }

    public void Kill()
    {
        this.isAlive = false;
        this.health = 0;

        this.gameObject.SetActive(false);
        Destroy(this.gameObject); // Check this is functional and not causing issues with the queue in BattleHandler
    }

}
