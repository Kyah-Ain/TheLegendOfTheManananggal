using System; // Grants access to classes and functions that uses your system
using System.Collections; // Grants access to collections strcutures like ArrayList
using System.Collections.Generic; // Grants access to collections and data structures like List and Dictionary
using UnityEngine; // Grants access to Unity's core features

public class Player2 : MonoBehaviour
{
    [Header("Player's Components")]
    public Rigidbody2D player2; // Placeholder for your character object
    public Animator animator; // Placeholder for the character's animations

    [Header("Environment & Interaction")]
    public Transform groundCheck; // Collider Checker
    public LayerMask groundLayer; // To specify what object touched/collide an object

    [Header("Atributes")]
    public float speed; // For how fast the player could move
    public float jumpingPower; // For how high the player could jump

    [Header("Positions")]
    public float horizontal; // Character's placeholder for left and right displacement 
    public float vertical; // Character's placeholder for up and down displacement 

    [Header("Animations")]
    public bool isWalking; // Character's placeholder to determine if animation should be executed to match the actions

    [Header("States")]
    private bool isFacingRight = true; // For flipping the character based on which direction it's moving

    void Start() // Start is called before the first frame update
    {
        player2 = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }

    void Update() // Update is called once per frame
    {
        CharacterMovement();
        //HandleAnimation();
        Flip();
    }

    void FixedUpdate() // Smoother Movement for Left and Right (in order to evenly distribute the milliseconds per frame)
    {
        player2.velocity = new Vector2(horizontal * speed, player2.velocity.y); // Moves the player left and right
    }

    void CharacterMovement() // Function for the character's movement 
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Get's the key inputs from the Input Manager (Alternative method for "GetKey" functions)

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            player2.velocity = new Vector2(player2.velocity.y, -jumpingPower); // JUMP Downwards
        }

        isWalking = horizontal != 0 ? true : false; // Shortest If-Else Statement (Shortcut Method)
    }

    //void HandleAnimation()
    //{
    //    animator.SetBool("isWalking", isWalking); // Animation's on/off switch in order for the computer to know when to trigger the animation and when to stop
    //}

    void Flip() // Function for flipping the character's sprite
    {               // Flip to Left                    // Flip to Right
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            // Responsible for switching the value true or false (On and Off)
            isFacingRight = !isFacingRight;

            // Responsible for Flipping/Mirroring of the sprite
            Vector2 localScale = transform.localScale; // Creates a modifieable copy of transform from the inspector
            localScale.x *= -1f; // Updates the JUST the value
            transform.localScale = localScale; // Applying the effect to the gameobject
        }
    }

    private bool IsGrounded() // Function to check if the player is on the ground
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer); // Returns true or false
    }

    void OnDrawGizmosSelected() // Adds highlight markings to whatever unseen system range detector
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.5f);
        }
        else
        {
            Debug.Log("Gizmos don't exists");
        }
    }

}
