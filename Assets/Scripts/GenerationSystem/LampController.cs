using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Made by Damon
public class LampController : MonoBehaviour
{
    public int mMaximumActiveLamps;
    public List<GameObject> mLamps = new List<GameObject>();
    public List<GameObject> mActiveLamps = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        for(int i = 0; i < mLamps.Count; i++)
        {
            DisableLamp(mLamps[i]);
        }
    }
    /// <summary>
    /// Randomly choose maximum active lamps, randomly activing one for hunters
    /// </summary>
    void Start()
    {
        // Deactivate all lamps
        for(int i = 0; i < mMaximumActiveLamps; i++)
        {
            int _random = Random.Range(0, mLamps.Count - 1);
            ActivateLamp(mLamps[_random]);
        }
        //Provide hunters with 1 randomly active light source
        int _randomActive = Random.Range(0, mActiveLamps.Count - 1);
        mActiveLamps[_randomActive].GetComponent<Lamp>().ForceCapture();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Activates a lamp upon request, allowing scripts to function as intended
    /// </summary>
    /// <param name="pLamp"></param>
    private void ActivateLamp(GameObject pLamp)
    {
        mActiveLamps.Add(pLamp);
        mLamps.Remove(pLamp);
        pLamp.GetComponent<Lamp>().enabled = true;
        pLamp.GetComponentInChildren<Canvas>().enabled = true;
    }
    /// <summary>
    /// Disables the lamp and prevent it from operating as intended
    /// </summary>
    /// <param name="pLamp"></param>
    private void DisableLamp(GameObject pLamp)
    {
        // Need to disable lamps properly, keep visible but disable scripts
        pLamp.GetComponent<Lamp>().enabled = false;
        pLamp.GetComponentInChildren<Canvas>().enabled = false;
        pLamp.GetComponentInChildren<LightController>().enabled = false;
    }
}
