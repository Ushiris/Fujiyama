using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    float duration = 1.5f;
    Vector3 to;
    Vector3 from;
    bool isActive = false;
    float count;

    // Start is called before the first frame update
    void Start()
    {
        from = transform.position;
        to = transform.position;
        to.y += 2.5f;
        if(duration<=0f)
        {
            duration = 0.01f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            count += Time.deltaTime;
            transform.position = Vector3.Lerp(from, to, count / duration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Makaron")
        {
            isActive = true;
        }
    }
}
