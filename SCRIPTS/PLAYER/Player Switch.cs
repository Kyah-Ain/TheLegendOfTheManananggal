using System; // Grants access to classes and functions that uses your system
using System.Collections; // Grants access to collections strcutures like ArrayList
using System.Collections.Generic; // Grants access to collections and data structures like List and Dictionary
using UnityEngine; // Grants access to Unity's core features

public class PlayerSwitch : MonoBehaviour
{
    [Header("Variables")]
    public Player1 player1Controller; // Contains an object with a Player1 Script
    public Player2 player2Controller; // Contains an object with a Player2 Script

    public bool player1IsActive = true; // Boolean to switch from one script to another

    void Start()
    {
        SwitchPlayer();
    }

    void Update() // Update is called once per frame
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Sets the number 1 key at the top of the alphabets on the keyboard to becomes a trigger
        {
            SwitchPlayer(); // Calls the SwitchPlayer functions 
        }
    }

    public void SwitchPlayer() // Function for switching between controllers in the scene
    {
        player1Controller.enabled = player1IsActive; // Disables the Player 1 Script
        player2Controller.enabled = !player1IsActive; // Disables the Player 2 Script
        player1IsActive = !player1IsActive; // Disables the Player 1 Script
    }
}
