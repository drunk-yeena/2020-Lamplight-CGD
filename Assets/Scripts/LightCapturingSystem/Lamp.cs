using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Made by Damon
public class Lamp : MonoBehaviour
{
    public bool mLit;
    public int mCurrentProgress;
    public int mMaxProgress;
    private bool mCapturing;
    private bool mResetting;
    public Enums.PlayerType mCapturingPlayerType;
    private List<GameObject> mPlayers = new List<GameObject>();

    // UI
    [Header("UI Control")]
    public Image mProgressBar;
    public TextMeshProUGUI mProgressText;

    // Visuals
    [Header("Visual Control")]
    public Light mLightControl;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mCurrentProgress == 0) { this.gameObject.GetComponentInChildren<Canvas>().enabled = false; }
        else { this.gameObject.GetComponentInChildren<Canvas>().enabled = true; }
    }
    /// <summary>
    /// If hunter or ghost player enters, add them into actively competing players list
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hunter" || other.tag == "Ghost")
        {
            mPlayers.Add(other.gameObject);
        }
        if (mPlayers.Count - 1 >= 0 && mCapturing != true)
        {
            // Stop reset
            StopCoroutine("ResetPoint");
            mResetting = false;

            StartCoroutine("CapturePoint");
            mCapturing = true;
        }
    }
    /// <summary>
    /// If player leaves, remove them from the list. If no players a present, begin to reset point
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        mPlayers.Remove(other.gameObject);
        if(mPlayers.Count == 0 && mResetting != true)
        {
            // Stop capturing
            StopCoroutine("CapturePoint");
            mCapturing = false;
            // Return back to previous value
            StartCoroutine("ResetPoint");
            mResetting = true;
        }
    }
    /// <summary>
    /// Force captures the lamp upon request
    /// </summary>
    public void ForceCapture()
    {
        mCurrentProgress = 100;
        mLit = true;
        UpdateUI();
        UpdateVisuals();
    }
    /// <summary>
    /// Begins to reset point back to original capture state
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetPoint()
    {
        while(true)
        {
            if(mLit == true)
            {
                mCurrentProgress++;
                if(mCurrentProgress >= 100)
                {
                    StopCoroutine("ResetPoint");
                    mResetting = false;
                    mCurrentProgress = 100;
                }
                if(mCurrentProgress == 100)
                {
                    VictoryConditions.GetInstance().CheckHunterVictoryCondition();
                }
            }
            else
            {
                mCurrentProgress--;
                if(mCurrentProgress <= 0)
                {
                    StopCoroutine("ResetPoint");
                    mResetting = false;
                    mCurrentProgress = 0;
                }
                if (mCurrentProgress == 0)
                {
                    VictoryConditions.GetInstance().CheckGhostVictoryConditions();
                }
            }
            UpdateUI();
            UpdateVisuals();
            yield return new WaitForSeconds(0.5f);
        }
    }
    /// <summary>
    /// Begins to capture point to new contesting party after calculating which player type has overwhelming numbers
    /// </summary>
    /// <returns></returns>
    public IEnumerator CapturePoint()
    {
        while(true)
        {
            SetCapturePointOwner();
            if(mCapturingPlayerType == Enums.PlayerType.Ghost)
            {
                // Decrease counter every second
                if(mCurrentProgress != 0)
                {
                    mCurrentProgress--;
                }
                else
                {
                    // Ghosts capture it or hunters didn't fully capture and light goes out
                    mLit = false;
                    mCapturing = false;
                    VictoryConditions.GetInstance().CheckGhostVictoryConditions();
                    StopCoroutine("CapturePoint");
                }
            }
            if(mCapturingPlayerType == Enums.PlayerType.Hunter)
            {
                // Increase
                if(mCurrentProgress != 100)
                {
                    mCurrentProgress++;
                }
                else
                {
                    // Hunter captures it
                    mLit = true;
                    mCapturing = false;
                    VictoryConditions.GetInstance().CheckHunterVictoryCondition();
                    StopCoroutine("CapturePoint");
                }
            }
            UpdateUI();
            UpdateVisuals();
            yield return new WaitForSeconds(0.5f);
            // If ghosts or nobody is capturing it, reduce to 0
            // If hunter is capturing it and it isn't 100 already, increase
            // If stalemate, keep the same
        }
    }
    /// <summary>
    /// Used to determine how many of each player are within the area of influence, and returns the leading party
    /// </summary>
    /// <returns></returns>
    private int GetPlayerDisposition()
    {
        int _dispositionCounter = 0;
        foreach(GameObject mPlayer in mPlayers)
        {
            if(mPlayer.tag == "Hunter")
            {
                _dispositionCounter--;
            }
            if(mPlayer.tag == "Ghost")
            {
                _dispositionCounter++;
            }
        }
        return _dispositionCounter;
        // If > 0 = ghosts, if < 0 = hunters, if 0 equal of both or none
    }
    /// <summary>
    /// Sets the capturing party to the side with most players within the circle of influence
    /// </summary>
    private void SetCapturePointOwner()
    {
        int _disposition = GetPlayerDisposition();
        if(_disposition > 0)
        {
            // Ghosts
            mCapturingPlayerType = Enums.PlayerType.Ghost;
        }
        if(_disposition < 0)
        {
            // Hunters
            mCapturingPlayerType = Enums.PlayerType.Hunter;
        }
        else if (_disposition == 0)
        {
            mCapturingPlayerType = Enums.PlayerType.None;
        }
    }
    /// <summary>
    /// Refreshes the UI to show progress changes
    /// </summary>
    private void UpdateUI()
    {
        mProgressBar.fillAmount = (float)mCurrentProgress / 100f;
        mProgressText.text = mCurrentProgress.ToString();
    }
    /// <summary>
    /// Changes light intensity to reflect capture rate
    /// </summary>
    private void UpdateVisuals()
    {
        mLightControl.intensity = ((float)mCurrentProgress / 100f) * 10;
    }
}
