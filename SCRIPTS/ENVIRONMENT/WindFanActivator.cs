using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFanActivator : MonoBehaviour
{
    [Header("WIND SETTINGS")]
    public Vector2 windDirection = Vector2.up; // (0,1)=Up, (0,-1)=Down, (1,0)=Right, (-1,0)=Left
    public float windStrength = 5f; // How strong the wind pushes

    [Header("AUDIO")]
    public AudioSource[] audioSources; // Multiple audios if needed

    private HashSet<Rigidbody2D> affectedRigidbodies = new HashSet<Rigidbody2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayAudio();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                affectedRigidbodies.Add(rb);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null && affectedRigidbodies.Contains(rb))
            {
                rb.velocity = windDirection.normalized * windStrength;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopAudio();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null && affectedRigidbodies.Contains(rb))
            {
                affectedRigidbodies.Remove(rb);
                rb.velocity = Vector2.zero; // Reset velocity
            }
        }
    }

    void PlayAudio()
    {
        if (audioSources != null)
        {
            foreach (AudioSource audio in audioSources)
            {
                audio.Play();
            }
        }
    }

    void StopAudio()
    {
        if (audioSources != null)
        {
            foreach (AudioSource audio in audioSources)
            {
                audio.Stop();
            }
        }
    }
}
