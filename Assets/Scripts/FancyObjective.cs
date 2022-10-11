using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyObjective : MonoBehaviour
{
    public float speed;

    private float time = 0f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
        time += Time.deltaTime;
        if (time > 2 * Mathf.PI) time -= 2 * Mathf.PI;
        transform.localPosition = new Vector3(0, 1 + Mathf.Sin(time) * 0.25f, 0);
    }
}
