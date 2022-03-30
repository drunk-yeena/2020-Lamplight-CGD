using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Stephan
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Sends the user to the next scene in the build order, dictated by current scene index+1
    /// </summary>
    public void PlayGame() //Loads the game scene as per build index
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Quits the game. Displays a message into the debug log to ensure functionality is maintained during debug.
    /// </summary>
    public void QuitGame() //Quits the application
    {
        Debug.Log("Game tried to quit");
        Application.Quit();
    }
}
