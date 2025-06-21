using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperHalfController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float initialJumpForce = 10f;        // First jump force
    public float doubleJumpForce = 5f;          // Second jump force (half of initial)
    public float maxJumpHeight = 3f;            // Maximum height before falling
    public float fallSpeed = 8f;                // Downward speed after peak
    public float jumpCooldown = 0.5f;           // Time before next jump

    [Header("Ground Hover Settings")]
    public float hoverBobAmount = 0.1f;
    public float hoverBobSpeed = 5f;
    public float hoverReturnSpeed = 8f;         // Smooth return to base position

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Item Grab Settings")]
    public Transform grabPoint;                 // Where the item will stick
    public float grabRange = 1f;                // How close you have to be
    public LayerMask itemLayer;                 // Layer for grabbable items

    [Header("Sounds")]
    public float footstepInterval = 1f;
    private bool playingFootsteps = false;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Ceiling Detection")]
    public Transform ceilingCheck;
    public float ceilingCheckRadius = 0.1f;
    
    [Header("Control Toggle")]
    public bool canControl = true;

    //private var (Note: Arrange later)
    private Rigidbody2D rb;
    private bool isGrounded;
    private float hoverBaseY;
    private bool wasGroundedLastFrame;
    private float lastHoverY;
    private float lastJumpTime;
    private bool isJumping;
    private float jumpStartY;
    private bool canDoubleJump;                 // Track if double jump is available
    private bool isTouchingCeiling;
    private GameObject heldItem = null;         // Currently held item
    private Rigidbody2D heldItemRb = null;      // Rigidbody of the held item

    private Animator animator;
    private bool isFacingRight = true;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0;
        hoverBaseY = transform.position.y;
        lastHoverY = 0f;
    }

    private void Update()
    { 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        isTouchingCeiling = Physics2D.OverlapCircle(ceilingCheck.position, groundCheckRadius, groundLayer);

        // Reset double jump when grounded
        if (isGrounded && !wasGroundedLastFrame)
        {
            hoverBaseY = transform.position.y - lastHoverY; 
            lastHoverY = 0f;
            isJumping = false;
            canDoubleJump = true;  // Reset double jump when landing
        }

        wasGroundedLastFrame = isGrounded;

        if (!canControl)
        {
            if (playingFootsteps)
                StopFootstepSounds(); // Stop sound properly when control is disabled

            if (isGrounded)
                rb.velocity = Vector2.zero;

            return;
        }

        // Start hover sound if not already playing
        if (!playingFootsteps)
            StartFootstepSounds();

        // Horizontal movement
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Animator updates
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

        //  Flip Sprite
        FlipSprite(move);

        // Jump logic
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && Time.time > lastJumpTime + jumpCooldown)
            {
                // Initial jump
                rb.velocity = new Vector2(rb.velocity.x, initialJumpForce);
                lastJumpTime = Time.time;
                isJumping = true;
                jumpStartY = transform.position.y;
                SoundEffectManager.Play("Jump");
                //animator.SetBool("isJumping", true);
            }
            else if (canDoubleJump && !isGrounded)
            {
                // Double jump (half power)
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                canDoubleJump = false;  // Used up double jump
                isJumping = true;
                jumpStartY = transform.position.y;  // Reset jump height measurement
                SoundEffectManager.Play("Jump");
                //animator.SetBool("isJumping", true);
            }
        }

        //grab item logic
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (heldItem == null)
            {
                AttemptGrab();
            }
            else
            {
                ReleaseItem();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            // Check if we've reached max height
            if (transform.position.y >= jumpStartY + maxJumpHeight || isTouchingCeiling)
            {
                isJumping = false;
                rb.velocity = new Vector2(rb.velocity.x, -fallSpeed);
            }
        }
        else if (!isGrounded)
        {
            // Apply constant downward force when not grounded and not jumping
            rb.velocity = new Vector2(rb.velocity.x, -fallSpeed);
        }

        // Hover logic
        if (isGrounded && !isJumping)
        {
            float hoverY = Mathf.Sin(Time.time * hoverBobSpeed) * hoverBobAmount;

            if (canControl)
            {
                rb.velocity = new Vector2(rb.velocity.x, (hoverBaseY + hoverY - transform.position.y) * hoverBobSpeed);
            }
            else
            {
                rb.velocity = new Vector2(0, (hoverBaseY - transform.position.y) * hoverBobSpeed);
            }

            lastHoverY = hoverY;
        }
    }

    private void FlipSprite(float moveInput)
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void AttemptGrab()
    {
        Collider2D itemCollider = Physics2D.OverlapCircle(transform.position, grabRange, itemLayer);

        if (itemCollider != null)
        {
            if (itemCollider.CompareTag("Item") || itemCollider.CompareTag("PickUp"))
            {
                heldItem = itemCollider.gameObject;
                heldItemRb = heldItem.GetComponent<Rigidbody2D>();

                if (heldItemRb != null)
                {
                    heldItemRb.isKinematic = true;  // Disable physics interactions
                    heldItemRb.velocity = Vector2.zero;  // Stop the item's velocity (no movement)

                    // Prevent the item from pushing the player by setting its position relative to the player
                    heldItem.transform.SetParent(grabPoint);  // Parent it to the player's grab point
                    heldItem.transform.localPosition = Vector3.zero;  // Position it at the grab point's position
                }
            }
        }
    }

    private void ReleaseItem()
    {
        if (heldItemRb != null)
        {
            heldItemRb.isKinematic = false;  // Restore physics interactions
            heldItemRb.velocity = rb.velocity;  // Let it inherit the character's velocity (if desired)

            heldItem.transform.SetParent(null);  // Remove the parent so it can fall or be thrown

            heldItem = null;
            heldItemRb = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void StartFootstepSounds()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstepSound), 0f, footstepInterval);
    }

    void StopFootstepSounds()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstepSound));
    }

    void PlayFootstepSound()
    {
        SoundEffectManager.Play("Hover");
    }

}