using UnityEngine;

public class ForgePlaySpace : PlaySpace<ForgeMat>
{
    public Transform[] spawns;

    public void StartForge()
    {
        // GenerateField(spawns);
    }

    public int ScoreOut()
    {
        int score = RawValue;
        ClearField();
        return score;
    }
}
