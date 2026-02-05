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
    public PaintDeck deck;

    // Deprecated
    public GameObject Graveyard;
    public GameObject Field;

    // Mana
    public Mana mana1; // Temp ugly implementation
    public Mana mana2; // -> Will change to use prefab and instantiate the mana in the scene on run time

    // Prefabs
    public Card CardRef; // To create new cards

    // Play Vars
    public bool playActive = false;
    [SerializeField] public List<ScoreData> score; // Score from the completed paintings
        // Win/Loss -> Not implemented yet

    // Mana Vars
    public int manaLimit;
    public int manaCount;

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
    public void StartPlay(int deckCount, int handLimit, int manaLimit)
    {
        // Create and shuffle the deck -> to be expanded to support creating deck from prefab
        deck.InitDeck(CardRef, this); // Allows for dynamic deck generation based on the provided card reference
        deck.GenDeck(deckCount);

        // Initialize the hand and draw the starting hand -> to be expanded to support creating hand from prefab? (maybe not as we always want a hand in the scene)
        hand.InitHand(handLimit, deck); // Allows for dynamic hand limits and linking to the deck for drawing -> currently auto starts draw

        this.manaLimit = manaLimit;
        RefreshMana();

        playActive = true; // Signifies the setup tasks are complete -> animations will still be enqueued
    }

    // Mana Methods
    public void UseMana() // Burns a mana for card action (future home for triggering more complex associated animations -> current dirty simple implementation) -> (consider bool return for checking for available mana in a conditional attempt from other sources - research if that is a professional application)
    {
        manaCount--;
        UpdateMana();
    }

    public void RefreshMana()
    {
        manaCount = manaLimit;
        UpdateMana();
    }

    public void UpdateMana() // Rough outline of public trigger for updating the Mana Animation State (will be a private method in future refactor of Mana handler object)
    {
        if (manaCount == 0)
        {
            mana1.spriteRenderer.color = new(1f, 1f, 1f, .3f);
            mana2.spriteRenderer.color = new(1f, 1f, 1f, .3f);
        }
        else if (manaCount == 1)
        {
            mana1.spriteRenderer.color = new(1f, 1f, 1f, 1f);
            mana2.spriteRenderer.color = new(1f, 1f, 1f, .3f);
        }
        else
        {
            mana1.spriteRenderer.color = new(1f, 1f, 1f, 1f);
            mana2.spriteRenderer.color = new(1f, 1f, 1f, 1f);
        }
    }

}