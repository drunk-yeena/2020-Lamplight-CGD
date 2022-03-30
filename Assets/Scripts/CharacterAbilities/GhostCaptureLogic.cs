using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Made by Damon
public class GhostCaptureLogic : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Begins capturing depending on entering party
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hunter" || other.tag == "Ghost")
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
    /// Stops capturing thread and begins reset method
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        mPlayers.Remove(other.gameObject);
        if (mPlayers.Count == 0 && mResetting != true)
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
    /// Debug function to force 100% capture
    /// </summary>
    public void ForceCapture()
    {
        mCurrentProgress = 100;
        UpdateUI();
    }
    /// <summary>
    /// Enumerator to reset point in event no party is present
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetPoint()
    {
        while (true)
        {
            if (mCurrentProgress <= 0)
            {
                StopCoroutine("ResetPoint");
                mResetting = false;
                mCurrentProgress = 0;
            }
            if (mCurrentProgress > 0)
            {
                mCurrentProgress--;
            }
            UpdateUI();
            yield return new WaitForSeconds(0.05f);
        }
    }
    /// <summary>
    /// Enumerator to handle capturing for either side, causing ticking increments till either captured or uncaptured
    /// </summary>
    /// <returns></returns>
    public IEnumerator CapturePoint()
    {
        while (true)
        {
            SetCapturePointOwner();
            if (mCapturingPlayerType == Enums.PlayerType.Hunter)
            {
                // Decrease counter every second
                if (mCurrentProgress != 0)
                {
                    mCurrentProgress--;
                }
                else
                {
                    mCapturing = false;
                    StopCoroutine("CapturePoint");
                }
            }
            if (mCapturingPlayerType == Enums.PlayerType.Ghost)
            {
                // Increase
                if (mCurrentProgress != 100)
                {
                    mCurrentProgress++;
                }
                else
                {
                    // Hunter captures it
                    mCapturing = false;
                    // Ghost is free - remove netting and begin to move
                    FreeGhost();
                    StopCoroutine("CapturePoint");
                }
            }
            UpdateUI();
            yield return new WaitForSeconds(0.05f);
        }
    }
    /// <summary>
    /// Re-enable ghost movement upon call
    /// </summary>
    private void FreeGhost()
    {
        if(gameObject.GetComponentInParent<Controller>() != null)
        {
            this.GetComponentInParent<Controller>().mSpeed = 10f;
            Destroy(this.gameObject.GetComponent<GhostCaptureLogic>());
            if(gameObject.transform.Find("Net(Clone)").gameObject != null)
            {
                GameObject _net = this.gameObject.transform.Find("Net(Clone)").gameObject;
                Destroy(_net);
            }
        }
    }
    /// <summary>
    /// Used to determine which quantity of player types are within the capturing zone
    /// </summary>
    /// <returns></returns>
    private int GetPlayerDisposition()
    {
        int _dispositionCounter = 0;
        foreach (GameObject mPlayer in mPlayers)
        {
            if (mPlayer.tag == "Hunter")
            {
                _dispositionCounter--;
            }
            if (mPlayer.tag == "Ghost")
            {
                _dispositionCounter++;
            }
        }
        return _dispositionCounter;
        // If > 0 = ghosts, if < 0 = hunters, if 0 equal of both or none
    }
    /// <summary>
    /// Sets the current capturing leader
    /// </summary>
    private void SetCapturePointOwner()
    {
        int _disposition = GetPlayerDisposition();
        if (_disposition > 0)
        {
            // Ghosts
            mCapturingPlayerType = Enums.PlayerType.Ghost;
        }
        if (_disposition < 0)
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
    /// Refreshes the UI
    /// </summary>
    private void UpdateUI()
    {
        if(mProgressBar != null && mProgressText != null)
        {
            mProgressBar.fillAmount = (float)mCurrentProgress / 100f;
            mProgressText.text = mCurrentProgress.ToString();
        }
    }
}
