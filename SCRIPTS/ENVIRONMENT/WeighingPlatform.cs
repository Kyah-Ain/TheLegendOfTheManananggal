using System.Collections; // Grants access to collections and data structures like Arrays, Lists, etc.
using System.Collections.Generic; // Grants access to generic collections like List<T>, Dictionary<TKey, TValue>, etc.
using UnityEngine; // Grants access to Unity's core classes and functions, such as MonoBehaviour, GameObject, etc.

public class WeighingPlatform : MonoBehaviour
{
    // ------------------------- VARIABLES -------------------------
    [Header("REFERENCES")]
    public GameObject characterParent; // The parent object of the character that will interact with the platform

    [Header("AUDIO")]
    public AudioSource[] audioSource; // Reference to the AudioSource component

    // ------------------------- METHODS -------------------------
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
