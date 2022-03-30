using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter_Script : MonoBehaviour
{
    Basic_Movement movement;
    
    public GameObject netEmitter;
    public GameObject hunterNet;

    private float HunterSpeedDefault = 5;
    private bool slowdownStatus = false;
    private bool catchNetReady = true;
    EnumTypes.ObjectTypes objectType = EnumTypes.ObjectTypes.HUNTER;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Basic_Movement>();
        movement.PlayerMovementSpeed = HunterSpeedDefault;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Slowdown Status Effect Methods
    void SlowdownStatusEffect()
    {
        if (slowdownStatus == false)
        {
            slowdownStatus = true;
            movement.PlayerMovementSpeed *= 0.65f;
            Invoke("ReturnToNormalSpeed", 4);
        }
    }

    void ReturnToNormalSpeed()
    {
        slowdownStatus = false;
        movement.PlayerMovementSpeed = HunterSpeedDefault;
    }
    #endregion

    #region Hunter Net Catching
    void ThrowNet()
    {
        if (catchNetReady == true)
        {
            GameObject net;
            Quaternion netEmmitterRotation;
            netEmmitterRotation = netEmitter.transform.rotation;

            catchNetReady = false;
            net = Instantiate(hunterNet, netEmitter.transform.position, netEmmitterRotation);
            Invoke("NetCooldown",5);

        }
    }

    void NetCooldown()
    {
        catchNetReady = false;
    }
    #endregion


}
