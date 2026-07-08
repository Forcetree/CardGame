using UnityEngine;

public class NecroPlaySpace : PlaySpace<NecroMat>
{
    public Transform[] spawns;

    public void StartNecro()
    {
        // GenerateField(spawns);
        // Here is how we start the Necro PlayHandling to determine what and who to spawn
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
