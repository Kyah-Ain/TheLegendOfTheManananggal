using System.Collections; // Grants access to collections and data structures like ArrayList
using System.Collections.Generic; // Grants access to collections and data structures like List and Dictionary
using UnityEngine; // Grants access to Unity's core features 
using UnityEngine.UI; // Allows the use of UI elements, such as Image components
using UnityEngine.SceneManagement; // Provides scene loading and management pre-built functions to use
using UnityEngine.Events; // Grants access to the pre-built custom event systems for triggering functions

public class SceneNavigator : MonoBehaviour
{
    // ------------------------- VARIABLES -------------------------
    [Header("Class Reference")]
    public SaveManager saveManager; // This is referenced to the SaveManager class for accessing save data

    [Header("Panel Reference")]
    public GameObject activeScene; // The currently active UI panel or scene
    public GameObject nextScene; // The next UI panel or scene to be displayed

    [Header("Scene Reference")]
    public string sceneToLoad; // Stores the name of the scene to load

    [Header("Button ID")]
    public int btnID; // Identifies different level load buttons dynamically

    // ------------------------- FUNCTIONS -------------------------

    // Resets the game progress on this PC and starts a new game
    public void BtnStartNew()
    {
        PlayerPrefs.SetInt("p_lastLevel", 0); // Resets the last played level to 0
        PlayerPrefs.SetInt("p_levelAt", 0); // Resets the highest unlocked level to 0
        BtnLoadSceneSpecific(); // Loads the initial scene
    }

    // Loads a specific scene stored in `sceneToLoad`
    public void BtnLoadSceneSpecific()
    {
        SceneManager.LoadScene(sceneToLoad); // Loads the scene by its name
    }

    // Checks if the player has a saved progress before starting
    public void BtnCheckStart()
    {
        if (saveManager.levelAt > 0) // If there is a saved progress, show a warning prompt
        {
            ButtonShowFloatingPanel();
        }
        else // If no progress, start the game directly
        {
            BtnLoadSceneSpecific();
            Debug.Log("Loading");
        }
    }

    // Navigates to the next scene (UI or game scene)
    public void ButtonNextScene()
    {
        if (activeScene != null) // Deactivates the current scene if assigned
        {
            activeScene.SetActive(false);
        }

        if (nextScene != null) // Activates the next scene if assigned
        {
            nextScene.SetActive(true);
        }
        else // Logs a message if no next scene is set
        {
            Debug.Log("Wala napong scene bossing. ; (");
        }
    }

    // Floating prompt to warn about overwriting saved data
    public void ButtonShowFloatingPanel() // Opens the prompt
    {
        nextScene.SetActive(true);
    }

    public void ButtonHideFloatingPanel() // Closes the prompt
    {
        activeScene.SetActive(false);
    }

    // Loads the last saved level from the SaveManager
    public void BtnLoadLast()
    {
        SceneManager.LoadScene("Level_" + saveManager.lastLevel);
    }

    // Loads a level based on the button ID (e.g., "Level_1", "Level_2", etc.)
    public void BtnLoadLevel()
    {
        SceneManager.LoadScene("Level_" + btnID);
    }

    public void BtnQuit()
    {
        Application.Quit();
    }
}
