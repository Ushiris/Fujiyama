using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    public static float height = 0.3f;
    public static float duration = 3f;

    float t = 0f;
    Vector3 from;
    Vector3 to;

    // Start is called before the first frame update
    void Start()
    {
        from = transform.position;
        to = transform.position;
        to.y += height;
        if (duration <= 0)
        {
            duration = 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime/duration;
        t -= (t >= 1f) ? 2f : 0f;
        transform.position = Vector3.Lerp(from, to, Mathf.Abs(t));
    }
}
