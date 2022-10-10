using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionStatus : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> spottedBy = new List<GameObject>();
    // Start is called before the first frame update
    
    public void SetDetected(bool value, GameObject origin)
    {
        if (value && !spottedBy.Contains(origin)) spottedBy.Add(origin);
        else if (!value && spottedBy.Contains(origin)) spottedBy.Remove(origin);
    }

    public List<GameObject> GetDetections(){ return spottedBy; }
}
