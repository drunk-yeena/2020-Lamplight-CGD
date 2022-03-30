using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
// Made by Damon
public class VictoryConditions : MonoBehaviour
{
    public GameObject hunterVictoryUI;
    public GameObject ghostVictoryUI;
    private static VictoryConditions mInstance;
    public static VictoryConditions GetInstance()
    {
        return mInstance;
    }

    public void Awake()
    {
        if(mInstance == null)
        {
            mInstance = this;
        }
    }
    public LampController mLampController;
    /// <summary>
    /// Used to determine whether hunters have successfully won the game or not
    /// </summary>
    public void CheckHunterVictoryCondition()
    {
        bool _victory = true;
        foreach (GameObject _lamp in mLampController.mActiveLamps)
        {
            if(!_lamp.GetComponent<Lamp>().mLit )
            {
                _victory = false;
            }
            if(_lamp.GetComponent<Lamp>().mCurrentProgress != 100)
            {
                _victory = false;
            }
        }
        if(_victory)
        {
            Time.timeScale = 0f;
            hunterVictoryUI.SetActive(true);
            // Displays message and ui, freezes time, hunters win!
        }
    }
    /// <summary>
    /// Used to determine whether ghosts have successfully won the game or not
    /// </summary>
    public void CheckGhostVictoryConditions()
    {
        bool _victory = true;
        foreach (GameObject _lamp in mLampController.mActiveLamps)
        {
            if (_lamp.GetComponent<Lamp>().mLit)
            {
                _victory = false;
            }
            if (_lamp.GetComponent<Lamp>().mCurrentProgress != 0)
            {
                _victory = false;
            }
        }
        if (_victory)
        {
            Time.timeScale = 0f;
            ghostVictoryUI.SetActive(true);
            
            // Displays message and ui, freezes time, ghosts win!
        }
    }
    /// <summary>
    /// Debug event to force hunter win
    /// </summary>
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (GameObject _object in VictoryConditions.GetInstance().mLampController.mActiveLamps)
            {
                _object.GetComponent<Lamp>().ForceCapture();
            }
            VictoryConditions.GetInstance().CheckHunterVictoryCondition();
        }*/
    }
}
