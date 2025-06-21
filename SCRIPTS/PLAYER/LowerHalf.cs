using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerHalf : MonoBehaviour
{
    [Header("MOVEMENT")]
    public bool canControl = true;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("GROUNDCHECK")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    //ANIMATION
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isFacingRight = true;

    //AUDIO
    private bool playingFootsteps = false;
    public float footstepSpeed = 1f; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!canControl)
        {
            stopFootsteps();
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Animator updates
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isJumping", !isGrounded);

        // Flip Sprite
        FlipSprite(move);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Start Footsteps
        if(Mathf.Abs(rb.velocity.x) > 0.1f && isGrounded && !playingFootsteps)
        {
            startFootsteps();
        }
        else if (Mathf.Abs(rb.velocity.x) <= 0.1f || !isGrounded)
        {
            stopFootsteps();
        }
    }

    void startFootsteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(playFootsteps), 0f, footstepSpeed);
    }

    void stopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(playFootsteps));
    }

    void playFootsteps()
    {
        Debug.Log("Playing Footstep Sound");
        SoundEffectManager.Play("Footsteps");
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
}
