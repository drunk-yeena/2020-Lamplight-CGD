using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Damon
[RequireComponent(typeof(Light))]
public class LightController : MonoBehaviour
{
    public float mBaseIntensity = 0.0f;
    public float mMaxIntensity = 0.5f;
    public float mMinIntensity = 1f;
    public float mFrequency = 0.0f;
    public float mAmplitude = 0.0f;
    public Light mLightSource;

    private Color mOriginalColour;
    // Start is called before the first frame update
    void Start()
    {
        mOriginalColour = mLightSource.color;
    }

    // Update is called once per frame
    void Update()
    {
        mLightSource.color = mOriginalColour * FlickerAttenuation();
    }
    /// <summary>
    /// Applies light attenutation randomly using linear gradient to provide flickering to the light source
    /// </summary>
    /// <returns></returns>
    public float FlickerAttenuation()
    {
        float _offsetX = (Time.deltaTime * Random.Range(0.01f, 1f)) * mFrequency;
        _offsetX = _offsetX - Mathf.Floor(_offsetX);
        float _offsetY = Mathf.PerlinNoise(_offsetX, Random.value * 2);
        return (_offsetY * mAmplitude) + mBaseIntensity;
    }
}
