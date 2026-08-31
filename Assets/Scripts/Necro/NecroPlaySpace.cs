using UnityEngine;

public class NecroPlaySpace : PlaySpace<NecroMat>
{
    public Transform[] spawns;
    public BattleHandler battleHandler;

    // private list of battleObjects(?)

    public void StartNecro()
    {
        GenerateField(spawns);
        // Determine what battleObject to use based on the current game state or player choice -> a dungeon run will be a list of possible battleObjects and the player will choose one to fight against. The battleObject will be a struct that holds the enemy data and any other relevant information for the battle.
        battleHandler.StartBattle(/*battleObject*/);
    }

    public int ScoreOut()
    {
        int score = RawValue;

        // Need to read the stack and create the final board piece
            // Item will need a special struct made for holding the data points

        ClearField();
        return score;
    }
}
