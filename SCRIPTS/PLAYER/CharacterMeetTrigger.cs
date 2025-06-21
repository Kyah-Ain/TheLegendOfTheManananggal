using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMeetTrigger : MonoBehaviour
{
    public GameObject upperHalf;
    public GameObject lowerHalf;
    public GameObject fusionManager; // Empty object with the Animator
    public AudioClip fusionSound;
    public float meetDistance = 1f;
    public bool missionComplete = false;
    public GameObject gameOverScreen; // UI for game over/completion

    private Animator fusionAnimator;
    private AudioSource audioSource;

    void Start()
    {
        fusionAnimator = fusionManager.GetComponent<Animator>();
        audioSource = fusionManager.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!missionComplete && upperHalf != null && lowerHalf != null)
        {
            float distance = Vector2.Distance(upperHalf.transform.position, lowerHalf.transform.position);

            if (distance <= meetDistance)
            {
                missionComplete = true;
                Debug.Log("Mission Complete! Characters have met!");

                // Disable the characters' SpriteRenderers
                upperHalf.GetComponent<SpriteRenderer>().enabled = false;
                lowerHalf.GetComponent<SpriteRenderer>().enabled = false;

                // Play Fusion Animation
                fusionAnimator.SetTrigger("Fusion"); // Assuming "Fusion" trigger is set in the Animator

                // Play fusion sound effect
                if (fusionSound != null)
                {
                    audioSource.PlayOneShot(fusionSound); // Play the sound once
                }

                // Wait for the fusion animation to complete, then show Game Over screen
                StartCoroutine(ShowGameOverScreen());
            }
        }
    }

    private IEnumerator ShowGameOverScreen()
    {
        // Wait for a specific time (based on your animation length) before showing the screen
        yield return new WaitForSeconds(3f); // Adjust based on your animation length

        // Activate the game over screen (UI)
        gameOverScreen.SetActive(true);
    }
}
