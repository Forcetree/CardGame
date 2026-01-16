using System.Collections.Generic;
using UnityEngine;

public static class CardCombiner
{
    private static Dictionary<(Card.cardType Base, Card.cardType Add), Card.cardType> ComboMatrix;

    public static void InitMatrix()
    {
        // Fill dictionary with the card combos and the created return combo
        ComboMatrix = new Dictionary<(Card.cardType Base, Card.cardType Add), Card.cardType>
        {
            // Primary mixes
            { (Card.cardType.Red, Card.cardType.Blue),   Card.cardType.Toxic   }, // Red + Blue -> Toxic (Purple)
            { (Card.cardType.Red, Card.cardType.Yellow), Card.cardType.Amber   }, // Red + Yellow -> Amber (Orange)

            { (Card.cardType.Blue,   Card.cardType.Red),    Card.cardType.Toxic   }, // Blue + Red -> Toxic (Purple)
            { (Card.cardType.Blue,   Card.cardType.Yellow), Card.cardType.Life    }, // Blue + Yellow -> Life (Green)

            { (Card.cardType.Yellow, Card.cardType.Red),    Card.cardType.Amber   }, // Yellow + Red -> Amber (Orange)
            { (Card.cardType.Yellow, Card.cardType.Blue),   Card.cardType.Life    }, // Yellow + Blue -> Life (Green)

            // Light Pures (Can only add white to a color - not start as white)
            { (Card.cardType.Red,    Card.cardType.White), Card.cardType.Coral   }, // Red + White -> Coral (Pink)
            { (Card.cardType.Yellow, Card.cardType.White), Card.cardType.Sun     }, // Yellow + White -> Sun (Light Yellow)
            { (Card.cardType.Blue,   Card.cardType.White), Card.cardType.Hydro   }, // Blue + White -> Coral (Cyan)

            // Light Mixes (Can only add white to a color - not start as white)
            { (Card.cardType.Toxic,  Card.cardType.White), Card.cardType.Iris    }, // Toxic (Purple) + White -> Iris (Lavender)
            { (Card.cardType.Amber,  Card.cardType.White), Card.cardType.Nectar  }, // Amber (Orange) + White -> Nectar (Light Orange)
            { (Card.cardType.Life,   Card.cardType.White), Card.cardType.Moss    }, // Life (Green) + White -> Moss (Light Green)

            // Dark Pures (Can only add black to a color - not start as black)
            { (Card.cardType.Red,    Card.cardType.Black), Card.cardType.Blood   }, // Red + Black -> Blood (Dark Red)
            { (Card.cardType.Yellow, Card.cardType.Black), Card.cardType.Gold    }, // Yellow + Black -> Gold (Dark Yellow)
            { (Card.cardType.Blue,   Card.cardType.Black), Card.cardType.Abyss   }, // Blue + Black -> Abyss (Dark Blue)

            // Dark Mixes (Can only add black to a color - not start as black)
            { (Card.cardType.Toxic,  Card.cardType.Black), Card.cardType.Obsidian}, // Toxic (Purple) + Black -> Obsidian (Dark Purple)
            { (Card.cardType.Amber,  Card.cardType.Black), Card.cardType.Lava    }, // Amber (Orange)  Black -> Lava (Dark Orange)
            { (Card.cardType.Life,   Card.cardType.Black), Card.cardType.Serpenite} // Life (Green) + Black -> Serpenite (Dark Green)
        };
    }

    public static bool TryCombo(Card.cardType bottom,  Card.cardType top, out Card.cardType comboType) => ComboMatrix.TryGetValue((bottom, top), out comboType);

}
