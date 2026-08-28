using System;
using UnityEngine;

[Serializable]
public class ScoreData
{
    public int score;
    public int turns;

    public ScoreData(int score, int turns)
    {
        this.score = score;
        this.turns = turns;
    }
}
