using UnityEngine;

public class PaintExampleCard : Card
{
    public enum CardTypes 
    {
        Red,
        Yellow,
        Blue,
        White,
        Black,

        // Combos made on the board
        // Light Pures
        Coral, // Red + White
        Sun, // Yellow + White
        Hydro, // Blue + White

        // Dark Pures
        Blood, // Red + Black
        Gold, // Yellow + Black
        Abyss, // Blue + Black

        // Mixes
        Toxic, // -> Purple
        Amber, // -> Orange
        Life, // -> Green

        // Light Combos
        Iris, // Purple + White
        Nectar, // Orange + White
        Moss, // Green + White

        // Dark Combos
        Obsidian, // Purple + Black
        Lava, // Orange + Black
        Serpenite, // Green + Black

        Back // Card back when flipped
    }

    public CardTypes mySpecificType;

    public override int CardTypeID
    {
        get => (int)mySpecificType;
        set => mySpecificType = (CardTypes)value;
    }
}
