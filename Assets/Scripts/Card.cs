using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static Vector2Int standardCardSize = new Vector2Int(3, 5); // Card Ratios (not utilized)
    
    // Scene References
    public PlayHandler PlayHandler;

    // Under review -> Interaction Handles
    public bool hovering = false;
    public bool dragging = false;

    // Animation Controlers
    public Queue<(Vector3 pos, float time, Ease ease)> destinationsBuffer = new(); // Destinations are read FIFO

    // Attributes for the mover
    public bool InFlight = false;

    // Under review -> Attributes for hover controls when in hand
    public Vector3 handHome;
    
    public static float wiggle = 30;
    public static float bump = .3f;
    public static float hoverTime = .1f;
    public static Ease hoverEase = Ease.OutBack;
    public static float deHoverTime = .1f;
    public static Ease deHoverEase = Ease.OutBack;

    // Under Construction -> Collider mask attributes
    public LayerMask fieldLayer;
    public List<Collider2D> touchColliders = new();

    public bool hasMatFocus;
    public FieldMat MatFocus;
    public List<GameObject> overlappingMats = new();

    // Player Visible Card Attributes
    public string title;
    public int playCost; // ?
    public int value;
    public string flavor;

    // Internal Card Structs
    public cardState state;
    public cardType type;

    // Card Components
    public SpriteRenderer spriteRenderer; // Art link -> workshop how this is defined later or if it is more organic and we make it later
    public PolygonCollider2D myCollider;
    
    public Quaternion orbit = Quaternion.identity; // Might not use this anymore -> unless we need for the swing motion tilt

    // Card Class Structs
    public enum cardType
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
    public enum cardState
    {
        Deck,
        Hand,
        Grave
    }

    // Runtime
    public void Update()
    {
        SetSprite();        
    }
    public void FixedUpdate()
    {
        Mover();
    }

    // Runtime Managers
    private void SetSprite() // Sets the sprite for render
    {
        switch (type) // Set Sprite
        {
            case cardType.Red: spriteRenderer.color = Color.red; break;
            case cardType.Blue: spriteRenderer.color = Color.blue; break;
            case cardType.Yellow: spriteRenderer.color = Color.yellow; break;
            case cardType.White: spriteRenderer.color = Color.white; break;
            case cardType.Black: spriteRenderer.color = Color.gray1; break;
            case cardType.Back: spriteRenderer.color = Color.gray; break;
                // Need all color formats added eventually with actual sprites
        }
    }
    private void Mover()
    {
        InFlight = DOTween.IsTweening(transform);

        if (dragging) // Override the mover if card is being dragged
        {
            transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(0, 0, Camera.main.transform.position.z);
            // transform.rotation = Quaternion.identity;
        }
        else // Handle movements -> under review for handling moves which should terminate the card is active status on completion
        {
            // Check for a destination in the queue and confirm no active tween
            if (destinationsBuffer.Count > 0 && !DOTween.IsTweening(transform))
            {
                (Vector3 pos, float time, Ease ease) = destinationsBuffer.Dequeue();

                // Use the tween library to move smoothly to the target
                transform.DOLocalMove(pos, time).SetEase(ease).OnComplete(() => { /* Kill cards in grave here? */ });
            }
        }
    }

    // Mouse Detectors
    public void OnPointerDown(PointerEventData eventData) => dragging = true; // Drag Card
    public void OnPointerUp(PointerEventData eventData)
    {
        // Logic for dropping on the playfield
        if (myCollider.IsTouchingLayers(fieldLayer)) // Only run if we are touching something in the field
        {
            // Set new hand home
            handHome = new(0, 0, 0);
            // Queue? a new return to home animation
        }
        else // Current Debug test
        {
            destinationsBuffer.Enqueue((handHome, deHoverTime, deHoverEase)); // Currently using the same dehome parameters unless we want custom ones for a different feel when dropping a card
        }        

        dragging = false;
    }
    public void OnPointerClick(PointerEventData eventData) // This triggers after the pointer is released -> to avoid complexity don't support two click control and force drags
    {
        // No longer needed in the card class -> think of potential use cases
    }
    public void OnPointerEnter(PointerEventData eventData) // Consider how to handle the card moving to the field or to the grave (we want to move it and then deactivate it -> but the card could be snatched during the transition!)
    {
        hovering = true;
        if (!dragging && !InFlight)
        {
            destinationsBuffer.Enqueue((new(this.transform.localPosition.x, this.transform.localPosition.y + bump, transform.localPosition.z), hoverTime, hoverEase));
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;

        destinationsBuffer.Enqueue((handHome, deHoverTime, deHoverEase));
    }

    // Collision Detectors
    private void OnTriggerEnter2D(Collider2D collisions)
    {
        if (collisions.CompareTag("Mat") && !overlappingMats.Contains(collisions.gameObject)) // Determine different methods for pulling mat objects as apposed to other cards
        {
            // overlappingMats.Add(collisions.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D decollisions)
    {
        if (decollisions.CompareTag("Mat"))
        {
            // overlappingMats.Remove(decollisions.gameObject);
        }
    }
}