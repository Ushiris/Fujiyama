using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateImage : MonoBehaviour
{
    public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed);
    }
}
