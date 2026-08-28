using UnityEngine;

public class PaintPlaySpace : PlaySpace<PaintMat>
{
    public Transform[] spawns;

    public void StartPaint()
    {
        GenerateField(spawns);
    }

    public int ScoreOut()
    {
        int score = RawValue;
        ClearField();
        return score;
    }
}
