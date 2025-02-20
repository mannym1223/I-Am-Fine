using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceDebugUtils : MonoBehaviour
{
    public bool resetFps;
    private float lowestFrameRate = 9999f;

    // Update is called once per frame
    void Update()
    {
        if(resetFps)
        {
            lowestFrameRate = 9999f;
        }
        lowestFrameRate = Mathf.Min(1f / Time.deltaTime, lowestFrameRate);
        Debug.Log("Lowest FPS: " + lowestFrameRate);
        Debug.Log("-----Current FPS: " + 1f / Time.deltaTime);
    }
}
