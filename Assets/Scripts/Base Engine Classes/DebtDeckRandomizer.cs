using System;
using System.Linq;

public class DebtDeckRandomizer
{
    // Row indices for _dwc data matrix layout
        // 0 = Distribution
        // 1 = Expected Draws
        // 2 = Actual Draws
        // 3 = Active Weights

    private int[][] _dwc; // DrawWeightCalculator [4][n]

    private int _sumOfBaseParts;
    private int _numberOfTypes;
    private int _kScalar;

    private bool _isInitialized = false;

    /// <summary>
    /// Initializes or resets the debt randomizer with a given cocktail recipe and K gain factor.
    /// </summary>
    /// <remarks>
    /// Uses a controllable scalar to adjust the weighted values of each draw to allow controlled variation in Psuedo random while allowing infinite <see cref="Deck"/> formats.
    /// Allows for dynamically updating the distribution to control the weights as the game is played without reseting the <see cref="Deck"/> or <see cref="PlayerHand"/>.
    /// </remarks>
    /// <param name="typeDistribution">The given card distribution to be used for the random distribution.</param>
    /// <param name="kScalar">Gain Factor for strength of the psuedo weight correction.</param>
    public void Initialize(int[] typeDistribution, int kScalar)
    {
        _numberOfTypes = typeDistribution.Length;
        _kScalar = kScalar;
        
        _dwc = new int[4][];
        for (int i = 0; i < 4; i++) _dwc[i] = new int[_numberOfTypes];

        UpdateDistribution(typeDistribution);

        _isInitialized = true;
    }

    /// <summary>
    /// Seamless mid-session upgrade. Updates the ground truth distribution without wiping the entire historical karma arrays.
    /// </summary>
    /// <remarks>
    /// If a single distribution element is found to be zero the internal debt matrix history will be cleared if present.
    /// It is not possible to modify the type count (distribution length) after initializing the PsuedoRandom. 
    /// </remarks>
    public void UpdateDistribution(int[] newDistribution) 
    {
        _sumOfBaseParts = newDistribution.Sum();
        if (_sumOfBaseParts <= 0) throw new InvalidOperationException("Update Distribution Failed: Empty distribution.");
        
        // Currently not supporting modifying the length of the distribution
        if (_numberOfTypes != newDistribution.Length) throw new InvalidOperationException("Update Distribution Failed: Attempted to change the number of types in the distribution after initialization");

        newDistribution.CopyTo(_dwc[0], 0);

        if (_isInitialized) // If this is a post initialization update -> reset ideal and actual draw histories for elements that are zeroed
        {
            for (int i = 0; i < _numberOfTypes; i++)
            {
                if (_dwc[0][i] == 0)
                {
                    _dwc[1][i] = 0;
                    _dwc[2][i] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Executes the Debt Algorithm loop and returns the drawn card type index.
    /// </summary>
    /// <remarks>
    /// Runs and updates the weighted array values to determine next card draw. 
    /// Compares actual draws to expected ideal draws (based on the provided distribution) and applies the magnitude of the gain factor to determine active weights.
    /// </remarks>
    /// <returns> Agnostic int for type index of the resulting draw.</returns>
    public int DrawCardTypeIndex()
    {
        if (!_isInitialized) throw new InvalidOperationException("Attempted to get draw type from uninitialized DebtDeckRandomizer");

        int[] distribution = _dwc[0];
        int[] ideal = _dwc[1];
        int[] actual = _dwc[2];
        int[] weights = _dwc[3];

        for (int i = 0; i < _numberOfTypes; i++)
        {
            int scaledDist = distribution[i] * _kScalar; // Adjust the distribution spread upwards by scalar to keep pure integer math
            int variance = scaledDist + (ideal[i] - actual[i]); // Determine and adjust the scaled distribution by the unscaled debt ratio between actual and ideal draws

            weights[i] = variance < 0 ? 0 : variance; // Clamp the final weights to zero if debt exceeds the scalar
        }

        // Roll random

        // Linear Search

        // Find type -> update actual draws as += _sumOfBaseParts

        // Return found type
        return 0;
    }
}
