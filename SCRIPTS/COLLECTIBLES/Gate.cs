using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public string neededKeyID;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CollectibleManager.Instance.HasKey(neededKeyID))
            {
                // Insert animation code here if available
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("You need the key!");
            }
        }
    }
}
