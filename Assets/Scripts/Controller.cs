using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Damon
[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pPlayer;
    public Vector3 mLastLook;
    public Vector3 mCurrentLook;
    private Vector3 mDirectionVector;
    // Update is called once per frame
    [SerializeField]
    public float mSpeed = 5;
    public bool mCooldown = false;
    private Rigidbody mRigidBody;
    private Vector3 mOldPosition;

    // Lamp Specific
    public GameObject mLamp;
    public List<Light> mSources = new List<Light>();

    // Animations
    Animator mAnimator;

    //AirConsole
    public int mPlayerIndex;

    /// <summary>
    /// Create fundamental variables and references upon startup
    /// </summary>
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();
        mAnimator = GetComponent<Animator>();
        mOldPosition = pPlayer.transform.position;
    }
    /// <summary>
    /// Every frame translate and rotate the player model and light sources upon request
    /// </summary>
    void Update()
    {
        // Translate
        Vector3 _movement = mDirectionVector.normalized * mSpeed * Time.deltaTime;
        mRigidBody.MovePosition(transform.position + _movement);
        
        // Rotate
        if (_movement != Vector3.zero)
        {
            mCurrentLook = mDirectionVector;
        }
        Quaternion _direction = Quaternion.LookRotation(mCurrentLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, _direction, Time.deltaTime * 8);
        
        UpdateLightSources();
        mLastLook = mCurrentLook;
    }
    /// <summary>
    /// Used to begin animation state set within the player model
    /// </summary>
    /// <param name="pID"></param>
    public void StartAnimation(int pID)
    {
        mAnimator.SetInteger("condition", pID);
    }
    /// <summary>
    /// Determines whether the player has moved from direction not != 0
    /// </summary>
    /// <param name="pDirection"></param>
    public void UpdateDirectionalVector(Vector2 pDirection)
    {
        mDirectionVector = new Vector3(pDirection.x, 0, pDirection.y);
        if(pDirection == Vector2.zero)
        {
            StartAnimation(0);
        }
        else
        {
            StartAnimation(1);
        }

    }
    /// <summary>
    /// Updates all attached lightsources to reflect the players orientation and position
    /// </summary>
    private void UpdateLightSources()
    {
        foreach(Light _sources in mSources)
        {
            _sources.transform.position = mLamp.transform.localPosition + this.gameObject.transform.position;
            _sources.transform.rotation = this.gameObject.transform.rotation;
        }
    }
    /// <summary>
    /// Used to reset the cooldown of abilities
    /// </summary>
    void ResetCooldown()
    {
        mCooldown = false;
    }
}
