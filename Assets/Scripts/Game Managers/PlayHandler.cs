using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayHandler : MonoBehaviour
{
    // Members
    public enum GameMode
    {
        Paint,
        Forge,
        Necro
    }

    public GameMode currentGameMode = GameMode.Paint; // Default to Paint mode -> can be changed in the inspector or via code

    // Objects
    // Hand(s)
    public PlayerHand hand;

    // Deck(s)
    public Deck myDeck;
    public PaintDeck PaintDeck;
    public NecroDeck NecroDeck;
    //public ForgeDeck forgeDeck; // Not implemented yet

    public ManaPool manaPool;

    // Play Spaces
    public PaintPlaySpace PaintField; // Must be hard set to the PaintPlaySpace object in the scene
    public NecroPlaySpace NecroField; // Must be hard set to the NecroPlaySpace object in the scene

    // Deprecated
    public GameObject Graveyard;

    // Prefabs
    public Card myCardRef; // To create new cards | Default card is paint (?) -> needs to be owned by deck! Major fix required
    public NecroCard necroCardRef; // To create new Necro cards
    //public ForgeCard forgeCardRef;

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
            // No current live operations
        }
    }

    // Public Methods
    public void StartPlay(int deckCount, int handLimit, int manaLimit) // Basic simple setup for the play session -> to be expanded to support dynamic setup based on game mode or player choice
    {
        /* WHY: We are initializing the game mode and setting up the field and deck based on the selected game mode.
         * This allows for different gameplay experiences based on our game design pathing. 
         * Each game mode has its own unique logic for combining cards and generating the play field, which is why we need to initialize the appropriate combiner logic and generate the corresponding field.
         * Additionally, we are initializing the player's hand and mana pool to ensure that they have the necessary resources to play the game. 
         * HOWEVER: This is a temporary setup and will be expanded in the future to support both play types to be run in parallel or to allow for dynamic selection of game mode and logic based on player choice or game state.
         * We would need a more complex system that will also include multiple hands and play areas.
         * This is why we do not need dynamic/agnostic PlaySpace objects -> we need them independantly instantiated. */

        // WARNING: Need major change -> card ref needs to be owned by Deck not PlayHandler
        
        switch (currentGameMode)
        {
            // Initialize the Combiner Logic -> to be expanded to support dynamic logic selection based on game mode or player choice
            // Create the Field -> to be expanded to support creating field from prefab? For now hard set to generate a PaintPlaySpace in the scene

            case GameMode.Necro:
                // Setup for Necro mode
                CardCombiner.Initialize(new NecroCombiner());
                myDeck = NecroDeck; // Set the deck to the NecroDeck for this game mode
                myCardRef = necroCardRef; // Update the card reference for this game mode
                NecroField.StartNecro(); // Allows for dynamic field generation based on Necro Implementation
                break;
            case GameMode.Forge:
                // Setup for Forge mode
                //CardCombiner.Initialize(new ForgeCombiner());
                //ForgeField.GenerateField(ForgeField.spawns); // Allows for dynamic field generation based on the provided spawn points in the ForgePlaySpace object in scene
                //myDeck = ForgeDeck; // Set the deck to the ForgeDeck for this game mode
                //myCardRef = forgeCardRef; // Update the card reference for this game mode
                break;
            case GameMode.Paint:
                // Setup for Paint mode (Default)
                CardCombiner.Initialize(new PaintCombiner());
                PaintField.GenerateField(PaintField.spawns); // Allows for dynamic field generation based on the provided spawn points in the PaintPlaySpace object in scene
                myDeck = PaintDeck; // Set the deck to the PaintDeck for this game mode
                // myCardRef not updated
                break;
            default:
                Debug.LogWarning("Unknown game mode selected.");
                break;
        }

        // Deck Init methods
        myDeck.InitDeck(this, myCardRef, config => { });
        myDeck.GenDeck();

        // Initialize the hand and draw the starting hand -> to be expanded to support creating hand from prefab? (maybe not as we always want a hand in the scene)
        hand.InitHand(handLimit, myDeck); // Allows for dynamic hand limits and linking to the deck for drawing -> currently auto starts draw

        manaPool.InitManaPool(manaLimit); // Initializes the mana pool with the specified limit -> currently creates mana points as children of the pool and positions them with spacing

        playActive = true; // Signifies the setup tasks are complete -> animations will still be enqueued
    }
}