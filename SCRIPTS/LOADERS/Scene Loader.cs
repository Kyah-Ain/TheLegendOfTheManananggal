using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1 : MonoBehaviour
{
    [Header("Script Reference")]
    public SceneNavigator sceneNavigatorScript; // Grants access to the Scene Management Functions

    private void OnCollisionEnter2D(Collision2D collision) // Triggers the code inside when a collision happens
    {
        if (collision.gameObject.CompareTag("Player")) // Filters the trigger to be trigger only when activated by a Player
        {
            sceneNavigatorScript.BtnLoadSceneSpecific(); // Loads the game to the next level
        }

        // Input below here more CompareTag that would call BTNLoadSceneSpecific to recall the scene, like when colliding with dangerous objects or more
    }
}
