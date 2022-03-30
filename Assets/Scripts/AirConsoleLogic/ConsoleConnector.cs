using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
// Produced by Damon
public class ConsoleConnector : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> mPlayers = new List<GameObject>();
    public Dictionary<int, int> mPlayerIDs = new Dictionary<int, int>();

    public SpawnSelection mSelector;
    // Prefabs
    public GameObject mHunterPrefab;
    public GameObject mGhostPrefab;
    public GameObject mNetPrefab;
	public GameObject mBooPrefab;
    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }
    /// <summary>
    /// Handles the OnMessage response data packet from AirConsole controller
    /// </summary>
    /// <param name="fromDeviceID">Unique ID of device</param>
    /// <param name="data">JSON Object containing data from the controller</param>
    void OnMessage(int fromDeviceID, JToken data)
    {
        GameObject _player = FindPlayer(fromDeviceID);

        // Set animation state at the end - bool indicator

        if (data["joystick_left"] != null && data["joystick_left"]["position"] != null)
        {
            float _xOffset = (float) data["joystick_left"]["position"]["x"];
            float _yOffset = (float) data["joystick_left"]["position"]["y"];
            if(_player != null)
            {
                _player.GetComponent<Controller>().UpdateDirectionalVector(new Vector2(_xOffset, -_yOffset));
            }

        }
        if(data["joystick_left"] != null && data["joystick_left"]["touch"] != null)
        {
            if (_player != null && (bool)data["joystick_left"]["touch"] == false)
            {
                _player.GetComponent<Controller>().UpdateDirectionalVector(new Vector2(0, 0));
            }
        }

        if(data["usebutton2"] != null)
        {
            // Button 'B' is pressed. Checks if time is paused to ensure effects cannot be created in pause state.
            if (data["usebutton2"]["use"] != null && data["usebutton2"]["use"].ToString() == "True" && !PAUSE.GamePaused)
            {
                if(_player.tag == "Ghost")
                {
                    if(_player.gameObject.GetComponent<Controller>().mCooldown == false)
                    {
                        // Create scare
                        GameObject _booSpawn = (GameObject)Instantiate(mBooPrefab, new Vector3(_player.transform.position.x, _player.transform.position.y + 2.5f, _player.transform.position.z), Quaternion.identity);
                        //Setup Cooldown
                        _player.gameObject.GetComponent<Controller>().mCooldown = true;
                        _player.gameObject.GetComponent<Controller>().Invoke("ResetCooldown", 5);
                    }
                    
                }
                else if(_player.tag == "Hunter")
                {
                    if (_player.gameObject.GetComponent<Controller>().mCooldown == false)
                    {
                        // Create projectile
                        Vector3 _forward = _player.transform.forward;
                        GameObject _net = (GameObject)Instantiate(mNetPrefab, _player.transform.position + new Vector3(0, 1.5f, 0) + (_forward * 2), Quaternion.identity);
                        _net.transform.rotation = _player.transform.rotation;
                        //Setup Cooldown
                        _player.gameObject.GetComponent<Controller>().mCooldown = true;
                        _player.gameObject.GetComponent<Controller>().Invoke("ResetCooldown", 5);
                    }
                }
            }
        }
        if (data["usebutton1"] != null)
        {
            if (data["usebutton1"]["use"] != null)
            {

            }
            // Button 'A' is pressed
        }
    }
    /// <summary>
    /// Handles methodology for when a controller is connected
    /// </summary>
    /// <param name="pDevice">Unique ID of device</param>
    private void OnConnect(int pDevice)
    {
        if (mPlayerIDs.ContainsKey(pDevice)) { return; }
        mPlayerIDs.Add(pDevice, mPlayerIDs.Count);
        // Create new player model and assign id - has to be console ID not player ID
        if(mPlayerIDs.Count % 2 == 0)
        {
            GameObject _spawnPosition = mSelector.GetHunterSpawn();
            GameObject _newPlayer = (GameObject)Instantiate(mHunterPrefab, _spawnPosition.transform.position, Quaternion.identity);
            _newPlayer.GetComponent<Controller>().mPlayerIndex = pDevice;
            mPlayers.Add(_newPlayer);
        }
        else
        {
            GameObject _spawnPosition = mSelector.GetGhostSpawn();
            GameObject _newPlayer = (GameObject)Instantiate(mGhostPrefab, _spawnPosition.transform.position, Quaternion.identity);
            _newPlayer.GetComponent<Controller>().mPlayerIndex = pDevice;
            mPlayers.Add(_newPlayer);
        }

    }
    /// <summary>
    /// Handle disconnects from the controller and tidy any reminiscent data
    /// </summary>
    /// <param name="pDevice">Unique ID of requesting device</param>
    private void OnDisconnect(int pDevice)
    {
        GameObject _player = null;
        foreach(GameObject _playerObject in mPlayers)
        {
            if(_playerObject.GetComponent<Controller>().mPlayerIndex == pDevice)
            {
                _player = _playerObject;
            }
        }
        if(_player != null)
        {
            mPlayerIDs.Remove(_player.GetComponent<Controller>().mPlayerIndex);
            mPlayers.Remove(_player);
            GameObject.Destroy(_player);
        }
    }

    private void OnDestroy()
    {
        if(AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
        }
    }
    /// <summary>
    /// Finds a player by their unique device index
    /// </summary>
    /// <param name="pDeviceIndex"></param>
    /// <returns></returns>
    private GameObject FindPlayer(int pDeviceIndex)
    {
        return mPlayers.Find((x) => x.GetComponent<Controller>().mPlayerIndex == pDeviceIndex);
    }
}
