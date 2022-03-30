using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Made by Damon
public class NetProjectile : MonoBehaviour
{
    public float mSpeed;
    public Vector3 mDirection;
    public float mDuration;
    public GameObject mNetObject;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += (mDirection * mSpeed * Time.deltaTime);
    }
    /// <summary>
    /// If projectile hits ghost, then call and produce net methods and prefab
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ghost")
        {
            // Add timer / immobolise / show graphical details
            // Add graphical details 
            if(other.gameObject.GetComponent<GhostCaptureLogic>() == null)
            {
                other.gameObject.AddComponent<GhostCaptureLogic>();
                other.gameObject.GetComponent<Controller>().mSpeed = 0;
                GameObject _netCanvas = (GameObject)Instantiate(mNetObject, other.gameObject.transform.position, Quaternion.identity);
                _netCanvas.transform.SetParent(other.gameObject.transform);
            }
            Destroy(this.gameObject);
        }
        else
        {
            // Destroy projectile
            Destroy(this.gameObject);
        }
    }
}
