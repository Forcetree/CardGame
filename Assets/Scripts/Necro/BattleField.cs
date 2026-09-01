using UnityEngine;
using System.Collections.Generic;

public class BattleField : MonoBehaviour
{
    // Game Manager
    public PlayHandler PlayHandler; // Reference to the PlayHandler instance -> hard set in the inspector

    // Prefabs
    public NecroEnemy enemyPrefab; // Reference to the enemy prefab

    // Objects
    public Transform[] spawns;
    private Queue<NecroEnemy>[] enemies; // Array of queues to hold the enemies for each spawn point

    // Controllers
    public int[] battleClock { get; private set; } // BattleClock is an array of integers that represent the turn order for attacks.

    public void StartBattle(/*battleObject*/)
    {
        // Implement the logic to start the battle
        Debug.Log("Battle started!");

        // CreateBattleField
            // -> Implement battle object(?)
        CreateBattleField();

        BattleTurn();
    }

    public void EndBattle()
    {
        // Implement the logic to end the battle
        Debug.Log("Battle ended!");
    }

    public void BattleTurn()
    {
        // Implement the logic for a battle turn
        Debug.Log("Battle turn start!");

        for (int i = 0; i < enemies.Length; i++) // Should only be 2, but this is more flexible for future expansion
        {
            NecroEnemy cEnemy = enemies[i].Peek(); // Get the next enemy in the queue

            if (cEnemy.isSpawned)
            {
                cEnemy.Spawn();
                battleClock[i] = cEnemy.turnCycle; // Set the battle clock for the enemy
            }

            if (battleClock[i] > 0) 
            {
                battleClock[i]--; // Decrement the battle clock for the enemy
                return; 
            }

            if (battleClock[i] < 0) { cEnemy.Attack(PlayHandler.NecroField.activeMats[i]); } // Associated with the PlaySpace field that the enemy is aligned with by INDEX (no hard association with the PlaySpace field object itself)
        }

    }

    public void BattleEndTurn()
    {
        // Implement the logic to end the battle turn -> this method can be queued into the animation system to allow for a delay before the next turn starts
        Debug.Log("Battle turn end!");

        // Reactivate the buttons and UI (need to introduce a game play freeze so player can see the results of the turn before they can take their next action)
    }

    public void PlayerTurn()
    {
        // Implement the logic for the player's turn -> This triggers when the player has completed their turn and the game is ready to process the next turn in the battle sequence (attack sequence)
        Debug.Log("Player turn ended!");

        for (int i = 0; i < PlayHandler.NecroField.activeMats.Count; i++)
        {

        }

        BattleTurn();
    }

    private void CreateBattleField()
    {
        // Implement the logic to create the battle field
        Debug.Log("Creating battle field!");

        for (int i = 0; i < 2; i++) // Instantiate enemies and set their properties based on the battleObject (2 wide based on the PlaySpace)
        {
            for (int j = 0; j < 3; j++)
            {
                NecroEnemy nEnemy = Instantiate(enemyPrefab, spawns[i].position, Quaternion.identity); // Spawns based on x value alignment with PlaySpace fields
                nEnemy.Initialize(26, j, NecroCard.NecroCardTypes.Fire, 3); // Intro values -> set attack to j for fun variation

                nEnemy.transform.SetParent(this.transform); // Set the parent of the enemy to the BattleHandler for organization in the hierarchy
                nEnemy.name = $"Temp Enemy {i}-{j}"; // Name the enemy based on its spawn point and index

                enemies[i].Enqueue(nEnemy);
            }
        }
    }
}
