using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System;

public abstract class Card : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static Vector2Int standardCardSize = new Vector2Int(3, 5); // Card Ratios (not utilized)
    
    // Scene References
    public PlayHandler PlayHandler;

    // Sprites
    // public Sprite[] valueSprites; // Moved to digitizer class
        // Moved sprite controller to CardCombiner class -> under review for how to handle sprite management in the future

    // Under review -> Interaction Handles
    public bool hovering = false;
    public bool dragging = false;
    public bool dragLock = false;

    // Animation Controlers
    public Queue<(Vector3 pos, float time, Ease ease)> destinationsBuffer = new(); // Destinations are read FIFO

    // Attributes for the mover
    public bool InFlight = false;

    // Attributes for hover controls when in hand
    public Vector3 cardHome;

    // Static hover attributes
    public static float wiggle = 15;
    public static float bump = .4f;
    public static float hoverTime = .08f;
    public static Ease hoverEase = Ease.OutBack;
    public static float deHoverTime = .08f;
    public static Ease deHoverEase = Ease.OutBack;

    // Collider mask attributes
    public LayerMask fieldLayer;
    public List<Collider2D> touchColliders = new();

    public bool hasMatFocus;
    public FieldMat MatFocus;
    public List<FieldMat> overlappingMats = new();

    // Player Visible Card Attributes
    public string title;
    public int playCost; // not utilized
    public string flavor; // not utilized

    [Tooltip("Don't change this in inspector or the value will not update")]
    [SerializeField] private int _value;
    public int value
    {
        get { return _value; }
        set
        {
            _value = value;
            ValueRenderer.value = value;
        }

    }

    // Internal Card Attribute Controllers
    public int cardSortOrder
    {
        get { return spriteRenderer.sortingOrder; }
        set
        {
            spriteRenderer.sortingOrder = value;            
            ValueRenderer.UpdateRenderSorting(); // Ensure the value is always on top of the card art
        }
    }

    // Internal Card Structs
    public cardState state; // Not currently being leveraged (may not be needed in the long run as it is a dependant variable not a state controller)

    // Card Components
    public SpriteRenderer spriteRenderer; // Art link -> workshop how this is defined later or if it is more organic and we make it later
    public Animator animator;

    public PolygonCollider2D myCollider;
    public ValueDigitizer ValueRenderer;

    // public SpriteRenderer ValueRenderer; // Removing to use new digitizer class

    public Quaternion orbit = Quaternion.identity; // Might not use this anymore -> unless we need for the swing motion tilt

    public enum cardState
    {
        Deck,
        Hand,
        Field,
        Grave
    }

    // Runtime
    public void Update()
    {

    }

    public void FixedUpdate()
    {
        Mover();
        if (PlayHandler.manaPool.ManaCount > 0) { MatFocuser(); }
    }

    // Card Creator and Abstraction
    public static Card Create(Card cardRef, PlayHandler handler, GameObject deck, Action<CardBuilder> configBlock)
    {
        Card nCard = Instantiate(cardRef, deck.transform.position, Quaternion.identity);

        nCard.PlayHandler = handler;

        nCard.gameObject.transform.parent = deck.transform;

        CardBuilder builder = new CardBuilder();
        configBlock?.Invoke(builder);

        // ApplySettings(builder)

        return nCard;
    }

    // Abstracts
    public abstract int CardTypeID { get; set; }
    public int numberOfTypesForDeck;
    public int numberOfTotalTypes;
    public abstract string CardTypeName { get; }

    public abstract void SetSprite();
    protected abstract void PlayAnimation();

    // Animation and Movement Managers
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
                transform.DOLocalMove(pos, time).SetEase(ease)
                    .OnComplete(() => 
                    { 
                        if (state == cardState.Hand) // Only unlocks cards if they are in the hand -> this is to avoid issues with cards being dragged to the field and then not being locked out of movement due to the dragLock being reset
                        {
                            dragLock = false;
                        }
                    });
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
        if (dragLock || PlayHandler.myDeck.IsProcessingDealBuffer || dragging) // Currently blocking drags during deal animations to avoid bugs and other weird interactions -> need to consider a more robust solution for handling this type of issue
        {
            return;
        }
        else
        {
            dragging = true; // Drag Card

            destinationsBuffer.Clear(); // Clear the buffer to avoid conflicts with any queued movements 
            
            this.spriteRenderer.sortingLayerName = "Focus";
            ValueRenderer.UpdateRenderSorting();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!dragLock && dragging) // Confirm that this is a valid release to trigger the drop logic
        {
            dragging = false;
            dragLock = true; // Lock the card to prevent multiple triggers of the drop logic -> currently needed to avoid bugs with multiple triggers of the drop logic due to the way we are handling the click and drag interactions

            // Logic for dropping on the playfield
            if (hasMatFocus && PlayHandler.manaPool.ManaCount > 0) 
            {
                PlayHandler.manaPool.BurnMana(); // Only running the mana handler if we have a focus and we had valid mana count (custom method for future animation controls and other mana related actions currently lives in the PlayHandler)

                // Set new hand home based on the currently focused mat object by grabbing the first mat in the list (we know the focused mat is the first because the operation that sets it with OrderBy has enabled the true flag)
                cardHome = new Vector3(0, 0, 0); // Reset the home to zero as the card is now on the field and no longer in the hand

                PlayHandler.hand.RemoveCardFromHand(this); // Remove the card from the hand

                overlappingMats[0].AddToStack(this);
                overlappingMats.Clear();

                spriteRenderer.sortingLayerName = "Field";
                ValueRenderer.UpdateRenderSorting();
                
                destinationsBuffer.Enqueue((cardHome, deHoverTime, deHoverEase)); // A new return to home animation or just reuse the hand return? Current implementation uses the hand home default params deHoverTime and deHoverEase

                dragging = false;
            }
            else // If we do not detect the field objects 
            {
                PlayAnimation();
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData) // This triggers after the pointer is released -> to avoid complexity don't support two click control and force drags
    {
        // No longer needed in the card class -> think of potential use cases
    }
    public void OnPointerEnter(PointerEventData eventData) // Consider how to handle the card moving to the field or to the grave (we want to move it and then deactivate it -> but the card could be snatched during the transition!)
    {
        hovering = true;
        if (!dragging && !InFlight && !dragLock)
        {
            destinationsBuffer.Enqueue((new(this.transform.localPosition.x, this.transform.localPosition.y + bump, transform.localPosition.z), hoverTime, hoverEase));
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        if (!dragging && !dragLock)
        {
            destinationsBuffer.Enqueue((cardHome, deHoverTime, deHoverEase));
        }        
    }

    // Collision Detectors
    private void OnTriggerEnter2D(Collider2D collisions)
    {
        FieldMat detected = collisions.gameObject.GetComponent<FieldMat>();
        if (detected != null && !overlappingMats.Contains(detected) && dragging && !dragLock)
        {
            overlappingMats.Add(detected);               
        }
    }

    private void OnTriggerExit2D(Collider2D decollisions)
    {
        FieldMat detected = decollisions.gameObject.GetComponent<FieldMat>();
        if (detected != null && overlappingMats.Contains(detected))
        {
            overlappingMats.Remove(detected);
            detected.highlighted = false;        
        }
    }
}