using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gondra : MonoBehaviour
{
    public Vector3 to;
    Vector3 from;

    // Start is called before the first frame update
    void Start()
    {
        from = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<PlayerController>().ForceMove()
        }
    }
}
