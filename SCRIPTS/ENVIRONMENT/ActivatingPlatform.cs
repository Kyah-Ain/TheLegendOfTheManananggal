using System.Collections; // Grants access to collections and data structures like Arrays, Lists, etc.
using System.Collections.Generic; // Grants access to generic collections like List<T>, Dictionary<TKey, TValue>, etc.
using UnityEngine; // Grants access to Unity's core classes and functions, such as MonoBehaviour, GameObject, etc.

public class ActivatingPlatform : MonoBehaviour
{
    // ------------------------- VARIABLES -------------------------
    [Header("REFERENCES")]
    public GameObject characterParent; // The parent object of the character that will interact with the platform
    public Transform[] pointPaths; // The two points between which the platform will move
    public Vector3 nextPosition; // The next position goal of the platform after reaching pointA or pointB

    [Header("ATTRIBUTES")]
    public float platformSpeed; // The speed at which the platform moves
    public float gizmoSize; // The size of the gizmo that will be drawn in the editor for visualizing the platform's path

    [Header("REGULATOR")]
    public int pointCounter = 0; // The counter that regulates the path of the platform between path points
    public bool isActivated = false; // A boolean to check if the platform is activated or not

    [Header("AUDIO")]
    public AudioSource[] audioSource; // Reference to the AudioSource component

    // ------------------------- METHODS -------------------------
    private void Start() // called once before the first frame update
    {
        nextPosition = pointPaths[pointCounter].position; // Set the initial next position
    }

    private void FixedUpdate() // called every fixed frame update
    {
        if (isActivated) // Checks if the platform is activated
        {
            MovePlatform(); // Call the method to move the platform
        }
        else
        {
            ReturnPlatform(); // Call the method to return the platform to its original position
        }
    }

    private void OnCollisionEnter2D(Collision2D actor) // Triggered when a game object collides with the platform
    {
        if (actor.gameObject.CompareTag("Player")) // Only executes if the object colliding with the platform has the tag "Player"
        {
            actor.gameObject.transform.parent = transform; // Makes the player a child of the platform so it moves with the platform
            PlayAudio(); // Call the function to play audio
        }
    }

    private void OnCollisionExit2D(Collision2D actor) // Triggered when a game object collides with the platform
    {
        if (actor.gameObject.CompareTag("Player")) // Only executes if the object colliding with the platform has the tag "Player"
        {
            actor.gameObject.transform.parent = characterParent.transform; // Makes the player a child of a character parent game object so it no longer moves with the platform
            StopAudio(); // Call the function to stop audio
        }
    }

    private void MovePlatform() // Function to move the platform
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, platformSpeed * Time.deltaTime); // Moves the platform from its current position to the next position at the specified speed

        if (transform.position == nextPosition) // Checks if the platform has reached the next position
        {
            pointCounter ++; // Move to the next target point

            if (pointCounter < pointPaths.Length) // Checks if the pointCounter is within the bounds of the pointPaths array
            {
                nextPosition = pointPaths[pointCounter].position; // Update next target position    
            }
        }
    }

    private void ReturnPlatform() // Function to return the platform to its original position
    {
        transform.position = Vector3.MoveTowards(transform.position, pointPaths[0].position, platformSpeed * Time.deltaTime); // Moves the platform back to its original position
        pointCounter = 0; // Reset the point counter to 0
    }

    void PlayAudio() // Play audio files related
    {
        if (audioSource != null)
        {
            foreach (AudioSource audio in audioSource) // Loop through all audio sources
            {
                Debug.Log($"Playing {audio}"); // Debug message
                audio.Play(); // Plays audio clip
            }
        }
    }

    void StopAudio() // Play audio files related
    {
        if (audioSource != null)
        {
            foreach (AudioSource audio in audioSource) // Loop through all audio sources
            {
                audio.Stop(); // Plays audio clip
            }
        }
    }

    private void OnDrawGizmos() // Adds highlight markings to the invisible gizmo ranges and detectors in the editor (THIS TOOL IS JUST FOR VISUALIZING OR DEBUGGING)
    {
        // PEN ATTRIBUTES
        Gizmos.color = Color.red; // Sets the color of the gizmo pen to red

        // DRAWING COMMANDS
        for (int i = 0; i < pointPaths.Length; i++) // Loops through all the points in the pointPaths array
        {
            Transform current = pointPaths[i]; // Gets the current point in the path
            Transform next = pointPaths[(i + 1) % pointPaths.Length]; // Gets the next point in the path, wrapping around to the first point if at the end of the array

            Gizmos.DrawWireSphere(current.position, gizmoSize); // Draws a red wireframe sphere at the current point's position
            Gizmos.DrawLine(current.position, next.position); // Draws a red line wire starting at the current point's position up to the next point's position
        }
    }

}
