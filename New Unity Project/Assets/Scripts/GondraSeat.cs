using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GondraSeat : MonoBehaviour
{
    gondra gondra_;
    // Start is called before the first frame update
    void Start()
    {
        gondra_ = GetComponentInParent<gondra>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!gondra_.IsKnowPL)
            {
                gondra_.player = other.GetComponent<PlayerController>();
                gondra_.IsKnowPL = true;
                gondra_.GetOn();
            }
        }
    }
}
