using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Made by Olumide
public class BooScare : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject.GetComponent<SphereCollider>(), 0.65f);
        Destroy(this.gameObject, 0.85f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hunter")
        {
            if(other.gameObject.GetComponent<HunterScareBehaviour>() == null)
            {
                other.gameObject.AddComponent<HunterScareBehaviour>();
            }
        }
    }
}
