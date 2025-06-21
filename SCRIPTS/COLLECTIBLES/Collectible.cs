using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Assigned in the Inspector
    public enum CollectibleType { Coin, Gem, Key }
    public CollectibleType type;
    public string keyID; // Only for keys

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case CollectibleType.Coin:
                    CollectibleManager.Instance.AddCoin();
                    break;
                case CollectibleType.Gem:
                    CollectibleManager.Instance.AddGem();
                    break;
                case CollectibleType.Key:
                    CollectibleManager.Instance.AddKey(keyID); // Different key IDs open different gates
                    break;
            }
            Destroy(gameObject); // Remove collectible from scene
        }
    }
}
