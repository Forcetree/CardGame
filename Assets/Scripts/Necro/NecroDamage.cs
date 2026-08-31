using UnityEditor;
using UnityEngine;

public class NecroDamage
{
    // Multipliers: rows = attacker, cols = defender.
    // Order: Fire, Growth, Earth, Iron, Frost, Water, Wind, Storm, Blight
    public static float[,] typeMultipliers = new float[,]
    {
        { 1.0f, 2.0f, 0.5f, 1.0f, 2.0f, 0.0f, 0.5f, 1.0f, 1.0f }, // Fire
        { 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 2.0f, 0.0f, 0.5f, 1.0f }, // Growth
        { 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 2.0f, 0.0f, 0.5f }, // Earth
        { 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 2.0f, 0.0f }, // Iron
        { 0.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 2.0f }, // Frost
        { 2.0f, 0.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f }, // Water
        { 1.0f, 2.0f, 0.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f }, // Wind
        { 0.5f, 1.0f, 2.0f, 0.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f }, // Storm
        { 2.0f, 0.5f, 1.0f, 2.0f, 0.0f, 0.5f, 1.0f, 1.0f, 1.0f }, // Blight
    };

    public int damage;
    public NecroCard.NecroCardTypes type;

    public static int operator -(int left, NecroDamage right)
    {
        return 0;
    }

    // Make damage method that takes in NecroMat?
}
