using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Movement : MonoBehaviour
{
    private float playermovementSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 _directionVector = new Vector3(horizontal, 0, vertical);

        // Translate
        Vector3 _movement = _directionVector * playermovementSpeed * Time.fixedDeltaTime;
        transform.Translate(_movement, Space.World);

        // Rotate
        Quaternion _direction = Quaternion.LookRotation(_directionVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, _direction, Time.fixedDeltaTime * 8);
    }

    public float PlayerMovementSpeed
    {
        get { return playermovementSpeed; }
        set { playermovementSpeed = value; }
    }
}
