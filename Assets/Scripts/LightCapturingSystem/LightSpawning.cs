using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Made by Damon
public class LightSpawning : MonoBehaviour
{
    public List<GameObject> mPotentialLampLocations = new List<GameObject>();
    public List<GameObject> mPotentialGhostSpawnLocations = new List<GameObject>();
    public List<GameObject> mChosenLamps;
    public int mNumberOfLamps;
    // Start is called before the first frame update
    void Start()
    {
        SpawnAllLamps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Activates all chosen lamps upon game creation
    /// </summary>
    private void SpawnAllLamps()
    {
        for (int i = 0; i < mNumberOfLamps; i++)
        {
            int _chosenIndex = Random.Range(0, mPotentialLampLocations.Count);
            bool _alreadyChosen = mPotentialLampLocations[_chosenIndex].active;
            while (_alreadyChosen == true)
            {
                // Prevents duplicates being chosen
                _chosenIndex = Random.Range(0, mPotentialLampLocations.Count);
                _alreadyChosen = mPotentialLampLocations[_chosenIndex].active;
            }
            // Activate the chosen lamp
            mPotentialLampLocations[_chosenIndex].SetActive(true);
            mChosenLamps.Add(mPotentialLampLocations[_chosenIndex]);
        }
    }
}
