using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionStatus : MonoBehaviour
{

    private bool detected = false;
    // Start is called before the first frame update
    
    public void SetDetected(bool value)
    {
        Debug.Log("Detected" + value);
        detected = value;
    }
}
