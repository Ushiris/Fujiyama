using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
    public float MinOpenTime = 1f;
    float time;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if ((time >= MinOpenTime) && Input.anyKey)
        {
            GameDirector.OpenScene("Title");
        }
    }
}
