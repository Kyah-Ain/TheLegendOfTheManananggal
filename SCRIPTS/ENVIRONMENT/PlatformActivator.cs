using System.Collections; // Grants access to collections and data structures like Arrays, Lists, etc.
using System.Collections.Generic; // Grants access to generic collections like List<T>, Dictionary<TKey, TValue>, etc.
using UnityEngine; // Grants access to Unity's core classes and functions, such as MonoBehaviour, GameObject, etc.

public class PlatformActivator : MonoBehaviour
{
    // ------------------------- VARIABLES -------------------------
    [Header("REFERENCES")]
    public ActivatingPlatform activatingPlatformScript; // Reference to the MovingPlatform.cs script

    [Header("AUDIO")]
    public AudioSource[] audioSource; // Reference to the AudioSource component

    // ------------------------- METHODS -------------------------
    private void OnCollisionEnter2D(Collision2D actor)
    {
        if (actor.gameObject.CompareTag("Player")) // Only executes if the object colliding with the platform has the tag "Player"
        {
            PlayAudio(); // Call the function to play audio
        }
    }

    private void OnCollisionStay2D(Collision2D actor) // Triggered when a game object collides with the platform
    {
        if (actor.gameObject.CompareTag("Player") || actor.gameObject.CompareTag("Item")) // Only executes if the object colliding with the platform has the tag "Player"
        {
            activatingPlatformScript.isActivated = true; // Toggle the activation state of the platform
        }
    }

    private void OnCollisionExit2D(Collision2D actor) // Triggered when a game object collides with the platform
    {
        if (actor.gameObject.CompareTag("Player") || actor.gameObject.CompareTag("Item")) // Only executes if the object colliding with the platform has the tag "Player"
        {
            activatingPlatformScript.isActivated = false; // Toggle the activation state of the platform
            StopAudio(); // Call the function to stop audio
        }
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

}
