using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;
    public int coins = 0;
    public int gems = 0;
    public HashSet<string> keys = new HashSet<string>();

    [Header("AUDIO")]
    public AudioSource[] audioSource; // Reference to the AudioSource component

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCoin()
    {
        coins++;
        PlayAudio(0); // Plays audio when coin is collected

        Debug.Log($"Coins: {coins}");
    }
    public void AddGem()
    {
        gems++;
        PlayAudio(1); // Plays audio when coin is collected

        Debug.Log($"Gems: {gems}");
    }
    public void AddKey(string keyID)
    {
        keys.Add(keyID);
        PlayAudio(2); // Plays audio when coin is collected

        Debug.Log($"Got key: {keyID}");
    }

    void PlayAudio(int index) // Play audio files related
    {
        if (audioSource != null)
        {
            audioSource[index].Play(); // Plays audio clip
        }
    }

    public bool HasKey(string keyID) => keys.Contains(keyID);
}
