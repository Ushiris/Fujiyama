using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public bool isActive = false;
    public float speed = 0f;

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            transform.Rotate(0, 0, speed);
        }
    }
}
