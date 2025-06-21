using System; // Grants access to classes and functions that uses your system
using System.Collections; // Grants access to collections strcutures like ArrayList
using System.Collections.Generic; // Grants access to collections and data structures like List and Dictionary
using UnityEngine; // Grants access to Unity's core features

public class AllInOneController : MonoBehaviour
{
    [Header("Player's Rigidbody")]
    public Rigidbody2D rigidbody1; // Placeholder for your character object
    //public Animator animator1; // Placeholder for the character's animations
    public Rigidbody2D rigidbody2; // Placeholder for your character object
    //public Animator animator2; 
    private Rigidbody2D currentCharac; // Placeholder for the active character

    [Header("Environment & Interaction")]
    public Transform groundCheck; // Collider Checker
    public LayerMask groundLayer; // To specify what object touched/collide an object

    [Header("Atrributes")]
    public float speed; // For how fast the player could move
    public float jumpingPower; // For how high the player could jump

    [Header("Positions")]
    public float horizontal; // Character's placeholder for left and right displacement 
    public float vertical; // Character's placeholder for up and down displacement 

    [Header("Booleans")]
    public bool isWalking; // Character's placeholder to determine if animation should be executed to match the actions
    private bool isFacingRight = true; // For flipping the character based on which direction it's moving
    private bool isChangeCharacter = true; // For switching through characters

    void Start()
    {
        currentCharac = rigidbody1;
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        CharacterMovement();
        //HandleAnimation();
        SwitchChecker();
        Flip();
    }

    void CharacterMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Get's the key inputs from the Input Manager (Alternative method for "GetKey" functions)

        if (Input.GetButtonDown("Jump") && IsGrounded()) // Determines if collision and jump was true to proceed jumping ONLY WHEN THE CHARACTER IS AT GROUND
        {
            currentCharac.velocity = new Vector2(currentCharac.velocity.x, jumpingPower); // JUMP
        }

        //isWalking = horizontal != 0 ? true : false; // Shortest If-Else Statement (Shortcut Method)
    }

    //void HandleAnimation()
    //{
    //    animator.SetBool("isWalking", isWalking); // Animation's on/off switch in order for the computer to know when to trigger the animation and when to stop
    //}

    void Flip()
    {               // Flip to Left                    // Flip to Right
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            // Responsible for switching the value true or false (On and Off)
            isFacingRight = !isFacingRight;

            // Responsible for Flipping/Mirroring of the sprite
            Vector2 localScale = transform.localScale; // Creates a modifieable copy of transform from the inspector
            localScale.x *= -1f; // Updates the copy
            transform.localScale = localScale; // Applying the effect to the gameobject's transformScale
        }
    }

    void SwitchChecker() // Function for calling the "CharacterSwitch" function using a key in the keyboard
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Keycode for "Number 1" key on the top of your keyboard, below the F1 and so on keys.
        {
            CharacterSwitch(); 
        }
    } 

    private void CharacterSwitch() // Function for passing the controls from one character to another (this disables the controller for the another character)
    {
        isChangeCharacter = !isChangeCharacter; // Switching this variable value from true or false whenever "CharacterSwitch" function is called

        if (isChangeCharacter) // Determines hwere to pass on the control
        {
            currentCharac = rigidbody1; // Passing it to character 1
        } 
        else
        {
            currentCharac = rigidbody2; // Passing it to character 2
        }
    }

    void FixedUpdate() // Smoother Movement for Left and Right (in order to evenly distribute the milliseconds per frame)
    {
        if (currentCharac != null) // To prevent Unity from spamming the unreferenced error of something you don't need to use
        {
            currentCharac.velocity = new Vector2(horizontal * speed, currentCharac.velocity.y); 
        }
    }
        
    private bool IsGrounded() // Determines if there's a collision between the Character and the Ground
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer); // Returns true or false
    }

    void OnDrawGizmosSelected() // Adds highlight markings to whatever unseen system range detector (THIS TOOL IS JUST FOR DEBUGGING)
    {
        if (groundCheck != null) // To prevent Unity from spamming the unreferenced error of something you don't need to use
        {
            Gizmos.color = Color.red; // Color of the Wireframe
            Gizmos.DrawWireSphere(groundCheck.position, 0.5f); // Mimics the groundCheck.position in wireframes in order for you to see how large the detection to GROUND is
        }
        else
        {
            Debug.Log("Gizmos don't exists");
        }
    }

}
