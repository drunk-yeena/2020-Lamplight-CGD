using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Olumide
public class HunterScareBehaviour : MonoBehaviour
{
    [SerializeField]
    public float modifiedSpeed = 3;
    [SerializeField]
    public float slowtimer = 5;
    private float mSpeedCopy;
    private bool scared;

    private void Start()
    {
        ChangeHunterSpeed();
    }
    /// <summary>
    /// Decreases hunter speed upon recieving ghost scare
    /// </summary>
    void ChangeHunterSpeed()
    {
        if(scared == false)
        {
            scared = true;
            mSpeedCopy = this.gameObject.GetComponent<Controller>().mSpeed;
            this.gameObject.GetComponent<Controller>().mSpeed = modifiedSpeed;
            Invoke("RevertHunterSpeed", slowtimer);
        }
    }
    /// <summary>
    /// Reverts the hunter speed back to normal
    /// </summary>
    void RevertHunterSpeed()
    {
        this.gameObject.GetComponent<Controller>().mSpeed = mSpeedCopy;
        scared = false;
        Destroy(this.gameObject.GetComponent<HunterScareBehaviour>());
    }
}
