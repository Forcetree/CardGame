using UnityEngine;

public class NecroPlaySpace : PlaySpace<NecroMat>
{
    public Transform[] spawns;
    public BattleHandler battleHandler;

    public void StartNecro()
    {
        GenerateField(spawns);
        battleHandler.StartBattle();
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
