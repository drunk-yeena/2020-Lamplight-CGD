using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//Stephan
public class PAUSE : MonoBehaviour
{
    public static bool GamePaused = true;
    private bool GameStarted = false;


    public GameObject pauseMenuUI;
    public GameObject confirmMenuUI;
    public GameObject startMenuUI;
    public GameObject hunterWinUI;
    public GameObject ghostWinUI;

    /// <summary>
    /// Runs on scene start, ensures that the scene starts in a paused state.
    /// </summary>
    void Start()
    {
        GamePaused = true;
        GameStarted = false;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Called once per frame. This handles keyboard inputs in the game used to start and end the game.
    /// </summary>
    void Update()
    {
        //All continue scenarios are handled here.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Causes a prompt for the game to begin to appear.
            if (startMenuUI.activeInHierarchy)
            {
                startMenuUI.SetActive(false);
                confirmMenuUI.SetActive(true);
            }
            
            //Starts the game if the prompt is accepted.
            else if (confirmMenuUI.activeInHierarchy)
            {
                confirmMenuUI.SetActive(false);
                Time.timeScale = 1f;
                GamePaused = false;
                GameStarted = true;
            }

        }

        //All pause or return scenarios are handled here, from backing out of the game to pausing.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (confirmMenuUI.activeInHierarchy)
            {
                startMenuUI.SetActive(true);
                confirmMenuUI.SetActive(false);
            }

            else if (hunterWinUI.activeInHierarchy || ghostWinUI.activeInHierarchy)
            {
                QuitGame();
            }

            else if (!confirmMenuUI.activeInHierarchy)
            {
                if (startMenuUI.activeInHierarchy && !GameStarted)
                {
                    QuitGame();
                }
                else if (pauseMenuUI.activeInHierarchy)
                {
                    Resume();
                }
                else if (!pauseMenuUI.activeInHierarchy)
                {
                    Pause();
                }
            }



        }
    }

    /// <summary>
    /// Used to resume the game from a paused state. Deactivates the pause menu and continues the flow of time.
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    /// <summary>
    /// Used to pause the game. Activates the pause menu and stops game time.
    /// </summary>
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    /// <summary>
    /// Loads the user into the previous scene, dictated by the current scene index-1.
    /// </summary>
    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
