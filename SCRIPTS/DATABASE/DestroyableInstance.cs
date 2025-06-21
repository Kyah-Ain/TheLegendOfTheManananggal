using System.Collections; // Grants access to collections and data structures like ArrayList
using System.Collections.Generic; // Grants access to collections and data structures like List and Dictionary
using UnityEngine; // Grants access to Unity's core features 

public class AutoDestroy : MonoBehaviour
{
    void Start() // Start is called before the first frame update
    {
        Destroy(gameObject, 0.5f); // Destroys the game object this script is attached to after 0.5 seconds
    }
}
