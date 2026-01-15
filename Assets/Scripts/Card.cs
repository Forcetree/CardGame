using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;

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
    public Vector3 cardHome;
    
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
    public List<FieldMat> overlappingMats = new();

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
        MatFocuser();
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
    private void MatFocuser()
    {
        if (overlappingMats.Count > 0) 
        {
            overlappingMats = overlappingMats.Where(mat => mat.TryTarget(this)).OrderBy(mat => Vector2.Distance(transform.position, mat.transform.position)).ToList();

            for (int i = 0; i < overlappingMats.Count; i++) 
            { 
                    overlappingMats[i].highlighted = (i == 0); 
            } 

            hasMatFocus = true;
        }
        else { hasMatFocus = false; }
    }

    // Mouse Detectors
    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true; // Drag Card
        this.spriteRenderer.sortingLayerName = "Focus";
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Logic for dropping on the playfield
        if (hasMatFocus) // Only run if we have a focus
        {
            // Set new hand home based on the currently focused mat object by grabbing the first mat in the list (we know the focused mat is the first because the operation that sets it with OrderBy has enabled the true flag)
            transform.parent = overlappingMats[0].transform.parent; // Is this safe or is it better to reach to PlayHandler.Field to get the game object?
            cardHome = overlappingMats[0].transform.localPosition;
            PlayHandler.RemoveCardFromHand(this); // Remove the card from the hand
            overlappingMats[0].AddToStack(this); // Use a safe method in the PlayerHandler that removes the card from the hand and then passes that ref back to the mat?
            overlappingMats[0].highlighted = false; // Turn off the highlight now that we are placing a card (this might be best done internally in the mat when we trigger the AddToStack method after it is built out
            
            spriteRenderer.sortingLayerName = "Field";
            destinationsBuffer.Enqueue((cardHome, deHoverTime, deHoverEase)); // A new return to home animation or just reuse the hand return? Current implementation uses the hand home default params deHoverTime and deHoverEase
        }
        else // If we do not detect the field objects we throw the card back to the hand
        {
            destinationsBuffer.Enqueue((cardHome, deHoverTime, deHoverEase)); // Currently using the same dehome parameters unless we want custom ones for a different feel when dropping a card
            spriteRenderer.sortingLayerName = "Hand";
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

        destinationsBuffer.Enqueue((cardHome, deHoverTime, deHoverEase));
    }

    // Collision Detectors
    private void OnTriggerEnter2D(Collider2D collisions)
    {
        Debug.Log($"Collision detected with: {collisions.name}");

        FieldMat detected = collisions.gameObject.GetComponent<FieldMat>();
        if (detected != null && !overlappingMats.Contains(detected) && dragging)
        {
            overlappingMats.Add(detected);
            Debug.Log($"Hit a mat! Current mats detected: {overlappingMats.Count}");                      
        }
    }

    private void OnTriggerExit2D(Collider2D decollisions)
    {
        FieldMat detected = decollisions.gameObject.GetComponent<FieldMat>();
        if (detected != null && overlappingMats.Contains(detected))
        {
            overlappingMats.Remove(detected);
            detected.highlighted = false;
            Debug.Log($"Left a mat! Current mats detected: {overlappingMats.Count}");            
        }
    }
}