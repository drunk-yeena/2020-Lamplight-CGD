using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Made by Damon
public class SpawnSelection : MonoBehaviour
{
    public List<GameObject> mHunterPositions = new List<GameObject>();
    public List<GameObject> mGhostPositions = new List<GameObject>();
    // Start is called before the first frame update
    private bool _hunterSpawnFlipper = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Selects a randomly assigned spawn point for Ghosts
    /// </summary>
    /// <returns></returns>
    public GameObject GetGhostSpawn()
    {
        int _spawnPoint = Random.Range(0, mGhostPositions.Count - 1);
        return mGhostPositions[_spawnPoint];
    }
    /// <summary>
    /// Selects either of the two hunter spawn points, flipping the selection choice evenly
    /// </summary>
    /// <returns></returns>
    public GameObject GetHunterSpawn()
    {
        if(!_hunterSpawnFlipper)
        {
            _hunterSpawnFlipper = true;
            return mHunterPositions[0];
        }
        else
        {
            _hunterSpawnFlipper = false;
            return mHunterPositions[1];
        }

    }
}
