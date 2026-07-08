using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using DG.Tweening;

// Refactor needed: Break the labeled main components apart into single purpose objects
    // Mana -> In Progress

public class PlayHandler : MonoBehaviour
{
    // Objects
    public PlayerHand hand;
    public Deck deck;
    public ManaPool manaPool;
    public PaintPlaySpace PaintField; // Must be hard set to the PaintPlaySpace object in the scene
    public NecroPlaySpace NecroField; // Must be hard set to the NecroPlaySpace object in the scene

    // Deprecated
    public GameObject Graveyard;

    // Prefabs
    public Card CardRef; // To create new cards

    // Play Vars
    public bool playActive = false;
    [SerializeField] public List<ScoreData> score; // Score from the completed paintings
        // Win/Loss -> Not implemented yet

    // Graveyard Vars
    public List<Card> graveyard = new();

    // Runtime
    void Start()
    {
        StartPlay(60, 6, 2); 
    }


    void Update()
    {
        if (playActive) // Allows pausing and other operations
        {
            // Deck Updates
            

            // Hand Updates
            // Triggered only by turn -> under reconstruction
                // UpdateHand();
        }
    }

    // Public Methods
    public void StartPlay(int deckCount, int handLimit, int manaLimit) // Basic simple setup for the play session -> to be expanded to support dynamic setup based on game mode or player choice
    {
        // Initialize the Combiner Logic -> to be expanded to support dynamic logic selection based on game mode or player choice
        CardCombiner.Initialize(new NecroCombiner());

        // Create the Field -> to be expanded to support creating field from prefab? For now hard set to generate a PaintPlaySpace in the scene
        NecroField.GenerateField(NecroField.spawns); // Allows for dynamic field generation based on the provided spawn points in the NecroPlaySpace object in scene

        // Create and shuffle the deck -> to be expanded to support creating deck from prefab
        // Under reconstruction: Deck generation is currently hard set to generate a deck based on the provided card reference and the number of types for the deck, but will be expanded to support dynamic deck generation based on game mode or player choice (out of the prefab)
        deck.InitDeck(CardRef, this); // Allows for dynamic deck generation based on the provided card reference
        deck.GenDeck(deckCount, CardRef.numberOfTypesForDeck);

        // Initialize the hand and draw the starting hand -> to be expanded to support creating hand from prefab? (maybe not as we always want a hand in the scene)
        hand.InitHand(handLimit, deck); // Allows for dynamic hand limits and linking to the deck for drawing -> currently auto starts draw

        manaPool.InitManaPool(manaLimit); // Initializes the mana pool with the specified limit -> currently creates mana points as children of the pool and positions them with spacing

        playActive = true; // Signifies the setup tasks are complete -> animations will still be enqueued
    }
}