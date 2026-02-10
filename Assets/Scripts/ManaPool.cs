using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManaPool : MonoBehaviour
{
    public ManaPoint ManaRef; // Static reference for easy access across scripts

    [Tooltip("Space between mana points, can be adjusted for better visual appeal")]
    public static float manaSpacing = 1.2f;
    [Tooltip("Speed at which mana points fill one after another, can be adjusted for better visual appeal")]
    public static float manaFillDelay = .1f;

    [Tooltip("Current size of the mana pool, not adjustable in inspector")]
    public int ManaLimit => manaPoints.Count;

    [SerializeField] private List<ManaPoint> manaPoints; // Array of mana points in the pool
    
    private bool _isInitialized = false; // Flag to ensure mana pool is initialized only once
    private bool _isFilling = false; // Flag to prevent multiple simultaneous fill operations

    public int ManaCount
    {
        get
        {
            int count = 0;
            foreach (var mana in manaPoints)
            {
                if (mana.active) count++;
            }
            return count;
        }
    }

    public void InitManaPool(int size) // Assumes first time creation of ManaPool
    {
        if (_isInitialized) return; // Prevent re-initialization
        _isInitialized = true;

        for (int i = 0; i < size; i++)
        {
            ManaPoint nMana = Instantiate(ManaRef, transform); // Create mana point as child of the pool

            manaPoints.Add(nMana);

            nMana.transform.localPosition = new Vector3(i * manaSpacing, 0, 0); // Position mana points with spacing
        }

        FillManaPool(); // Optionally fill the mana pool immediately after initialization
    }

    public void FillManaPool() // Assumes we always want to fill the mana pool to max, can be adjusted for more flexible use cases (e.g. filling a specific number of mana points)
    {
        if (_isInitialized && !_isFilling)
        {
            StartCoroutine(ManaRefill()); // Start the coroutine to fill mana points with a delay between each fill
        }
    }

    private IEnumerator ManaRefill()
    {
        _isFilling = true; // Set the filling flag to prevent multiple fills at the same time
        for (int i = 0; i < manaPoints.Count; i++)
        {
            if (!manaPoints[i].active)
            {
                manaPoints[i].Fill();
                yield return new WaitForSeconds(manaFillDelay); // Wait before filling the next mana point
            }
        }
        _isFilling = false; // Reset the filling flag after all mana points have been processed
    }

    public void UpgradeManaPool() // Method to upgrade the mana pool size, underconstruction to be called for multiple mana additions in one upgrade, can be adjusted for more flexible use cases (e.g. upgrading by a specific number of mana points instead of setting a new size)
    {
        // Add new mana point to the pool
        ManaPoint nMana = Instantiate(ManaRef, transform); // Create new mana point as child of the pool
        manaPoints.Add(nMana); 
        nMana.transform.localPosition = new Vector3((manaPoints.Count - 1) * manaSpacing, 0, 0); // Position the new mana point with spacing based on its index in the list

        // Optionally, we can fill the new mana points immediately after upgrading
        FillManaPool();
    }

    public void BurnMana()
    {
        foreach (var mana in manaPoints)
        {
            if (mana.active)
            {
                mana.Burn();
                break; // Use one mana point at a time
            }
        }
    }
}
